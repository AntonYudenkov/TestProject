using System;
using System.Collections.Generic;

namespace BL.Models
{
    public class ResultDataModels
    {
        public List<ResultDataModel> Records { get; set; } 
    }
    public class ResultDataModel
    {
        public int UserId { get; set; }
        public string Pan { get; set; }
        public string ExpDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}