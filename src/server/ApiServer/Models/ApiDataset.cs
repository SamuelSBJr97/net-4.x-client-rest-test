using Microsoft.EntityFrameworkCore;

namespace ApiServer.Models
{
    public class ApiDataset
    {
        public string Key { get; set; }
        public DateTime? Date { get; set; }
        public string Group { get; set; }
        public string Value { get; set; }
    }
}
