using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Host.Api.Models;

public class CreateViewRequest
{
    
    /// <summary>
    /// The fully qualified name of the view to create
    /// </summary>
    [Required]
    public string ViewType { get; set; } = "";

    /// <summary>
    /// The owner Id of the view
    /// </summary>
    public string OwnerId { get; set; } = "";


}