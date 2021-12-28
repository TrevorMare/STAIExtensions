using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Host.Api.Models;

public class RemoveViewRequest
{
    [Required]
    public string ViewId { get; set; } = "";

}