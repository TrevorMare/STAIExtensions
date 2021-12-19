using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Data.AzureDataExplorer.DataContracts
{
    public class ExceptionParsedStack : IExceptionParsedStack
    {
        
        #region Properties
        public string? Id { get; set; }

        public string? Message { get; set; }
        
        public string? OuterId { get; set; }

        public List<IExceptionParsedStack> ParsedStack { get; set; } = new List<IExceptionParsedStack>();
         
        public string? Type { get; set; }

        public string? Assembly { get; set; }

        public string? Method { get; set; }
        
        public string? FileName { get; set; }
        
        public int? Level { get; set; }
        
        public int? Line { get; set; }
        #endregion
        
    }
}