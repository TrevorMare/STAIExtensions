using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Tests.Fixtures;

public class DataSetViewFixture : Core.Views.DataSetView
{

    public Dictionary<string, object>? FixtureParameters => ViewParameters;

}