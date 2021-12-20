using System;
using System.Collections.Generic;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Core.Tests.Fixtures;

public class FixtureException : IException
{
    public ICustomDimension? CustomDimensions { get; set; }
    public string? AppId { get; set; }
    public string? ApplicationVersion { get; set; }
    public string? AppName { get; set; }
    public string? ClientBrowser { get; set; }
    public string? ClientCity { get; set; }
    public string? ClientCountryOrRegion { get; set; }
    public string? ClientIP { get; set; }
    public string? ClientModel { get; set; }
    public string? ClientOS { get; set; }
    public string? ClientStateOrProvince { get; set; }
    public string? ClientType { get; set; }
    public string? ClientRoleInstance { get; set; }
    public string? ClientRoleName { get; set; }
    public string? iKey { get; set; }
    public string? ItemId { get; set; }
    public string? ItemType { get; set; }
    public string? OperationId { get; set; }
    public string? OperationName { get; set; }
    public string? OperationParentId { get; set; }
    public string? OperationSyntheticSource { get; set; }
    public string? SDKVersion { get; set; }
    public string? SessionId { get; set; }
    public DateTime TimeStamp { get; set; }
    public string? UserAccountId { get; set; }
    public string? UserAuthenticatedId { get; set; }
    public string? UserId { get; set; }
    public ICustomMeasurement? CustomMeasurements { get; set; }
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
}