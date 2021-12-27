using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Host.Api.Models;

public class CreateViewRequest
{
    
    [Required]
    public string ViewType { get; set; } = "";

    public string OwnerId { get; set; } = "";


}