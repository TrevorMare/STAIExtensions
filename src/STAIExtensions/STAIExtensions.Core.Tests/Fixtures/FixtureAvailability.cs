using System;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Core.Tests.Fixtures;

public class FixtureAvailability : IAvailability
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
    public double? Duration { get; set; }
    public string? Id { get; set; }
    public int? ItemCount { get; set; }
    public string? Location { get; set; }
    public string? Message { get; set; }
    public string? Name { get; set; }
    public string? PerformanceBucket { get; set; }
    public double? Size { get; set; }
    public string? Success { get; set; }
}