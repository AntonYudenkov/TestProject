using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BL.Helpers;
using BL.Interfaces;
using BL.Models;
using Newtonsoft.Json;

namespace BL.Services
{
    public class FileModificationService
    {
        public void Modification(string fileName, List<IUser> users, List<IUser> usersFromFile)
        {
            var usersForSave = usersFromFile.Where(x => !users.Select(u => u.UserId).Contains(x.UserId)).ToList();

            if (usersForSave.Any())
            {
               var file = ObjectToString(usersForSave);
               SaveFile(fileName, file);
                return;
            }

            DeleteFile(fileName);
        }

        public void CopyFile(string path)
        {
            string destPath = ConfigHelper.GetPathByFileName(path);
            File.Copy(path, destPath, true);
        }

        public void SaveReport(ResultDataModels model)
        {
            var file = SerializeJson(model);
            SaveFile("report.json", file);
        }
        
        private void SaveFile(string fileName, string data)
        {
            string destPath = ConfigHelper.GetPathByFileName(fileName);
            File.WriteAllText(destPath, data);
        }

        private void DeleteFile(string fileName)
        {
            string path = ConfigHelper.GetPathByFileName(fileName);
            File.Delete(path);
        }

        private string ObjectToString(List<IUser> users)
        {
            switch (users.First().GetType().Name)
            {
                case nameof(Card):
                    return SerializeXml(users);
                case nameof(CsvDataModel):
                    return CreateCsv(users);
                default:
                    return string.Empty;
            }
        }

        private string SerializeXml(List<IUser> users)
        {
            XmlDataModel model = new XmlDataModel()
            {
                Cards = users.Select(x => (Card)x).ToList()
            };
            
            XmlSerializer xsSubmit = new XmlSerializer(typeof(XmlDataModel));

            using(var sww = new StringWriter())
            {
                using(XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, model);
                    return sww.ToString();
                }
            }
        }

        private string CreateCsv(List<IUser> users)
        {
            List<CsvDataModel> models = users.Select(x => (CsvDataModel) x).ToList();
            
            StringBuilder sb = new StringBuilder($"UserId;Name;SecondName;Number{Environment.NewLine}");

            foreach (var model in models)
            {
                sb.Append($"{model.UserId};{model.Name};{model.SecondName};{model.Number}{Environment.NewLine}");
            }

            return sb.ToString();
        }

        private string SerializeJson(ResultDataModels model)
        {
           return JsonConvert.SerializeObject(model);
        }
    }
}