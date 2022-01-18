using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Host.Api.Models;

public class SetViewAutoRefreshEnabledRequest
{
    
    /// <summary>
    /// The view Id to un-freeze
    /// </summary>
    [Required]
    public string ViewId { get; set; } = "";

    /// <summary>
    /// The owner Id of the view
    /// </summary>
    public string OwnerId { get; set; } = "";
}