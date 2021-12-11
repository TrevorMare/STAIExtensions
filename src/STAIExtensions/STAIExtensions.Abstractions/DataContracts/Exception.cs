using System.Collections.Generic;
using STAIExtensions.Abstractions.Attributes;

namespace STAIExtensions.Abstractions.DataContracts
{
    public class Exception : DataContract, Interfaces.IHasCustomMeasurement
    {

        #region Properties

        [DataContractField("assembly")]
        public string Assembly { get; set; }
        
        [DataContractField("customMeasurements")]
        public CustomMeasurement CustomMeasurements { get; set; }
        
        [DataContractField("details")]
        public List<ExceptionParsedStack> Details { get; set; }

        [DataContractField("handleAt")]
        public string HandleAt { get; set; }        

        [DataContractField("innermostAssembly")]
        public string InnermostAssembly { get; set; }
        
        [DataContractField("innermostMessage")]
        public string InnermostMessage { get; set; }

        [DataContractField("innermostMethod")]
        public string InnermostMethod { get; set; }

        [DataContractField("innermostType")]
        public string InnermostType { get; set; }

        [DataContractField("itemCount")]
        public int? ItemCount { get; set; }

        [DataContractField("message")]
        public string Message { get; set; }

        [DataContractField("method")]
        public string Method { get; set; }

        [DataContractField("outerAssembly")]
        public string OuterAssembly { get; set; }

        [DataContractField("outerMessage")]
        public string OuterMessage { get; set; }

        [DataContractField("outerMethod")]
        public string OuterMethod { get; set; }

        [DataContractField("outerType")]
        public string OuterType { get; set; }
        
        [DataContractField("problemId")]
        public string ProblemId { get; set; }

        [DataContractField("severityLevel")]
        public int? SeverityLevel { get; set; }
        
        [DataContractField("type")]
        public string Type { get; set; }
        #endregion
        
    }
}