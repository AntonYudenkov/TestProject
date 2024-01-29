using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BL.Helpers;
using BL.Interfaces;
using BL.Models;

namespace BL.Services
{
    internal class ReceivingDataService
    {
        internal IEnumerable<IUser> GetDataFromFile(string fileName)
        {
            string path = ConfigHelper.GetPathByFileName(fileName);
            
            switch (Path.GetExtension(fileName))
            {
                case ConfigHelper._xml:
                    return GetDataFromXml(path);
                case ConfigHelper._csv:
                   return GetDataFromCsv(path); 
                default: return null;
            }
        }

        internal List<string> GetPathsAllFilesByExt(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            ext = ext == ConfigHelper._csv ? ConfigHelper._xml : ConfigHelper._csv;
            var dir = ConfigHelper.GetPathByExt(ext);
            var files = Directory.GetFiles(dir, $"*{ext}");

            return files.ToList();
        }

        private IEnumerable<IUser> GetDataFromXml(string path)
        {
            var serializer = new XmlSerializer(typeof(XmlDataModel));

            using (var reader = new StreamReader(path))
            {
               var cards = (XmlDataModel)serializer.Deserialize(reader);
                reader.Close();
                return cards.Cards;
            }
        }

        private IEnumerable<IUser> GetDataFromCsv(string path)
        {
            IEnumerable<CsvDataModel> result = from line in File.ReadAllLines(path).Skip(1)
                let columns = line.Split(';')
                select new CsvDataModel
                {
                    UserId = int.Parse(columns[0]),
                    Name = columns[1],
                    SecondName = columns[2],
                    Number = columns[3]
                };

            return result;
        }
    }
}