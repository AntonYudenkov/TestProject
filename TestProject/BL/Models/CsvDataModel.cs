using BL.Interfaces;

namespace BL.Models
{
    public class CsvDataModel : IUser
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Number { get; set; }
    }
}