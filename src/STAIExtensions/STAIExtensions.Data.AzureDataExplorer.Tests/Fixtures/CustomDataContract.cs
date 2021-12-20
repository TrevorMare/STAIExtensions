﻿using System;
using STAIExtensions.Abstractions.DataContracts;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Fixtures;

public record CustomDataContract : IDataContract
{
    public DateTime TimeStamp { get; set; }
}