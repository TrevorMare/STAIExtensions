using System;
using STAIExtensions.Abstractions.DataContracts;

namespace STAIExtensions.Core.Tests.Fixtures;

public class FixtureMyModel : IDataContract
{
    public DateTime TimeStamp { get; set; }
}