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

        public bool Equals(ApiDataset other)
        {
            if (other == null)
                return false;
            return Key.Equals(other.Key) &&
                   Date == other.Date &&
                   Group.Equals(other.Group) &&
                   Value.Equals(other.Value);
        }
    }
}