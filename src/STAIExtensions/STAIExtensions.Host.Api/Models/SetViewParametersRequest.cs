using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Host.Api.Models;

public class SetViewParametersRequest
{
    [Required]
    public string ViewId { get; set; } = "";

    public string OwnerId { get; set; } = "";

    public Dictionary<string, object>? ViewParameters { get; set; }
}