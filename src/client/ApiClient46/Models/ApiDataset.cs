using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiClient46.Models.Services
{
    public class ApiDataset
    {
        public string Key { get; set; }
        public DateTime? Date { get; set; }
        public string Group { get; set; }
        public string Value { get; set; }
    }
}