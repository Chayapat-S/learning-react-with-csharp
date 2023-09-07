using System.Collections.Generic;

namespace api.Infrastructure
{
    public class AppSettings  
    {
        public string ConnectionString { get; set; }
        public Dictionary<string,  string> AppConfig { get; set; }
    }
}
