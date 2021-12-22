using System;
using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Fixtures;

public class CustomDataContract : DataContract
{
    public DateTime TimeStamp { get; set; }
}