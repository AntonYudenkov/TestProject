using System.Collections.Generic;
using System.IO;
using System.Linq;
using BL.Interfaces;
using BL.Models;

namespace BL.Services
{
    public class DataComparisonService
    {
        private readonly ReceivingDataService _receivingDataService = new ReceivingDataService();
        private readonly FileModificationService _fileModification = new FileModificationService();
        
        private static Dictionary<string, List<IUser>> _matchesFound;
        private static string _fileName;

        public int Compare(string path)
        {
            _fileName = Path.GetFileName(path);
            _matchesFound = new Dictionary<string, List<IUser>>();

            var files = _receivingDataService.GetPathsAllFilesByExt(_fileName);

            if (!files.Any()) return 0;

            var newFileData = _receivingDataService.GetDataFromFile(_fileName);

            foreach (var file in files)
            {
                List<IUser> matchedUsers = new List<IUser>();

                var users = _receivingDataService.GetDataFromFile(file);
                matchedUsers.AddRange(users.Where(x => newFileData.Select(n => n.UserId).Contains(x.UserId)));
                matchedUsers.AddRange(newFileData.Where(x => matchedUsers.Select(m => m.UserId).Contains(x.UserId)));

                _matchesFound.Add(file, matchedUsers);
            }

            return _matchesFound.SelectMany(x => x.Value.Select(s => s.UserId)).Distinct().Count();
        }

        public void GenerateReport()
        {
            var resultDataModels = new ResultDataModels()
            {
                Records = new List<ResultDataModel>()
            };

            foreach (var files in _matchesFound)
            {
                foreach (var user in files.Value)
                {
                    var resultDataModel =
                        resultDataModels.Records.FirstOrDefault(x => x.UserId == user.UserId);
                    resultDataModel = resultDataModel ?? new ResultDataModel();

                    DataFusion(resultDataModel, user);

                    if (resultDataModels.Records.All(x => x.UserId != user.UserId))
                        resultDataModels.Records.Add(resultDataModel);
                }

                var users = _receivingDataService.GetDataFromFile(files.Key).ToList();
                _fileModification.Modification(files.Key, files.Value, users);
                
                users = _receivingDataService.GetDataFromFile(_fileName).ToList();
                _fileModification.Modification(_fileName, files.Value, users);
            }

            _fileModification.SaveReport(resultDataModels);
        }

        private static void DataFusion(ResultDataModel resultDataModel, IUser file)
        {
            resultDataModel.UserId = file.UserId;
            
            if (file.GetType() == typeof(CsvDataModel))
            {
                var fileCsv = (CsvDataModel)file;
                resultDataModel.FirstName = fileCsv.Name;
                resultDataModel.LastName = fileCsv.SecondName;
                resultDataModel.Phone = fileCsv.Number;
            }

            if (file.GetType() != typeof(Card)) return;
            
            var fileXml = (Card)file;
            resultDataModel.Pan = fileXml.Pan;
            resultDataModel.ExpDate = fileXml.ExpDate;
        }
    }
}