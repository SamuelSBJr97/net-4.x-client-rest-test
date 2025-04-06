using Microsoft.EntityFrameworkCore;

namespace ApiServer.Models
{
    public class ApiDataset
    {
        public Guid Guid { get; set; }
        public DateTime Date { get; set; }
        public string Group { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
