using System.Collections.Generic;

namespace WordSearcher.Models
{
    public class FileProcessed
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public Dictionary<string, int> Dictionary { get; set; }
    }
}