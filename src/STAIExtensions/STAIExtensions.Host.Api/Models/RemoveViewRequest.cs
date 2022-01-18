using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Host.Api.Models;

public class RemoveViewRequest
{
    
    /// <summary>
    /// The view Id to remove from the collections
    /// </summary>
    [Required]
    public string ViewId { get; set; } = "";

}