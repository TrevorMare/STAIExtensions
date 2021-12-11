using System.Collections.Generic;
using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts
{
    public class ExceptionParsedStack
    {
        #region Properties
        [DataContractField("id")] 
        public long? Id { get; set; }

        [DataContractField("message")]
        public string Message { get; set; }
        
        [DataContractField("outerId")] 
        public long? OuterId { get; set; }

        [DataContractField("parsedStack")] 
        public List<ExceptionParsedStack> ParsedStack { get; set; } = new List<ExceptionParsedStack>();
        
        public string Type { get; set; }
        #endregion
    }
}