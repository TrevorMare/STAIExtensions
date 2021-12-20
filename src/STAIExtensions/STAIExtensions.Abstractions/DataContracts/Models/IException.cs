﻿
namespace STAIExtensions.Abstractions.DataContracts.Models
{
    public interface IException : IHasDefaultFields, IHasCustomMeasurement
    {

        #region Properties

        public string? Assembly { get; set; }
        
        public List<IExceptionParsedStack>? Details { get; set; }

        public string? HandleAt { get; set; }        

        public string? InnermostAssembly { get; set; }
        
        public string? InnermostMessage { get; set; }

        public string? InnermostMethod { get; set; }

        public string? InnermostType { get; set; }

        public int? ItemCount { get; set; }

        public string? Message { get; set; }

        public string? Method { get; set; }

        public string? OuterAssembly { get; set; }

        public string? OuterMessage { get; set; }

        public string? OuterMethod { get; set; }

        public string? OuterType { get; set; }
        
        public string? ProblemId { get; set; }

        public int? SeverityLevel { get; set; }
        
        public string? Type { get; set; }
        #endregion
        
    }
}