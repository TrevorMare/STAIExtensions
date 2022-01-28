using STAIExtensions.Abstractions.Common;

namespace STAIExtensions.Abstractions.DataContracts.Models;

/// <summary>
/// Base model for any Data Contract returned by the Telemetry Loader
/// </summary>
public class DataContract
{

    /// <summary>
    /// Gets or sets the record state, if this was not previously processed will be New
    /// </summary>
    public RecordState RecordState { get; set; } = RecordState.New;

}