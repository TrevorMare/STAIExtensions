using System.Collections.Generic;
using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public class ExceptionParsedStack
    {
        
        #region Properties
        [DataContractField("id")] 
        public string? Id { get; set; }

        [DataContractField("message")]
        public string? Message { get; set; }
        
        [DataContractField("outerId")] 
        public string? OuterId { get; set; }

        [DataContractField("parsedStack")] 
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