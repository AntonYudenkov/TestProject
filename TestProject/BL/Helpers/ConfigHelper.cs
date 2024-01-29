using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helpers
{
    public static class ConfigHelper
    {
        public const string _xml = ".xml";
        public const string _csv = ".csv";
        public const string _json = ".json";
        
        private static string _csvFilesPath;
        private static string _xmlFilesPath;
        private static string _jsonFilesPath;

        public static string CsvFilesPath =>
            _csvFilesPath ?? (_csvFilesPath = ConfigurationSettings.AppSettings["csv"]);
        
        public static string XmlFilesPath =>   
            _xmlFilesPath ?? (_xmlFilesPath = ConfigurationSettings.AppSettings["xml"]);
        
        public static string JsonFilePath => 
            _jsonFilesPath ??  (_jsonFilesPath = ConfigurationSettings.AppSettings["json"]);

        public static string GetPathByExt(string ext)
        {
            switch (ext)
            {
                case _xml:
                    return XmlFilesPath;
                case _csv:
                    return CsvFilesPath;
                case _json:
                    return JsonFilePath;
                default:
                    return String.Empty;
            }
        }
        
        public static string GetPathByFileName(string fileName)
        {
            string path = GetPathByExt(Path.GetExtension(fileName));
            return Path.Combine(path, Path.GetFileName(fileName));
        }
    }
}
