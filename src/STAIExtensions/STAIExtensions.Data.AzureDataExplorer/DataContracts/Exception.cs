using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Data.AzureDataExplorer.Attributes;
using STAIExtensions.Data.AzureDataExplorer.Serialization;

namespace STAIExtensions.Data.AzureDataExplorer.DataContracts
{
    public class Exception : DataContract, IException
    {

        #region Properties

        [DataContractField("assembly")]
        public string? Assembly { get; set; }
        
        [DataContractField("customMeasurements", typeof(Serialization.CustomMeasurementSeriliazation))]
        public ICustomMeasurement? CustomMeasurements { get; set; }

        [DataContractField("details", typeof(ExceptionParsedStackSerialization))]
        public List<IExceptionParsedStack>? Details { get; set; } = new List<IExceptionParsedStack>();

        [DataContractField("handleAt")]
        public string? HandleAt { get; set; }        

        [DataContractField("innermostAssembly")]
        public string? InnermostAssembly { get; set; }
        
        [DataContractField("innermostMessage")]
        public string? InnermostMessage { get; set; }

        [DataContractField("innermostMethod")]
        public string? InnermostMethod { get; set; }

        [DataContractField("innermostType")]
        public string? InnermostType { get; set; }

        [DataContractField("itemCount")]
        public int? ItemCount { get; set; }

        [DataContractField("message")]
        public string? Message { get; set; }

        [DataContractField("method")]
        public string? Method { get; set; }

        [DataContractField("outerAssembly")]
        public string? OuterAssembly { get; set; }

        [DataContractField("outerMessage")]
        public string? OuterMessage { get; set; }

        [DataContractField("outerMethod")]
        public string? OuterMethod { get; set; }

        [DataContractField("outerType")]
        public string? OuterType { get; set; }
        
        [DataContractField("problemId")]
        public string? ProblemId { get; set; }

        [DataContractField("severityLevel")]
        public int? SeverityLevel { get; set; }
        
        [DataContractField("type")]
        public string? Type { get; set; }
        #endregion
        
    }
}