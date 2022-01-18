using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Host.Api.Models;

public class SetViewAutoRefreshDisabledRequest
{
    
    /// <summary>
    /// The View Id to Freeze
    /// </summary>
    [Required]
    public string ViewId { get; set; } = "";

    /// <summary>
    /// The owner Id of the View
    /// </summary>
    public string OwnerId { get; set; } = "";
}