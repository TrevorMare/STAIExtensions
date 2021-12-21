using System.Collections.Generic;

namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public class ExceptionParsedStack
    {
        
        #region Properties
        public string? Id { get; set; }

        public string? Message { get; set; }
        
        public string? OuterId { get; set; }

        public List<ExceptionParsedStack> ParsedStack { get; set; } = new List<ExceptionParsedStack>();
        
        public string? Type { get; set; }

        public string? Assembly { get; set; }

        public string? Method { get; set; }
        
        public string? FileName { get; set; }
        
        public int? Level { get; set; }
        
        public int? Line { get; set; }
        #endregion
        
    }
}