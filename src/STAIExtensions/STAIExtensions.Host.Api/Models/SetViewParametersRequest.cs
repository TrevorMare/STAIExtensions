using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Host.Api.Models;

public class SetViewParametersRequest
{
    /// <summary>
    /// The View Id to set the parameters on
    /// </summary>
    [Required]
    public string ViewId { get; set; } = "";

    /// <summary>
    /// The owner Id of the View
    /// </summary>
    public string OwnerId { get; set; } = "";

    /// <summary>
    /// The parameter values to set on the view
    /// </summary>
    public Dictionary<string, object>? ViewParameters { get; set; }
}