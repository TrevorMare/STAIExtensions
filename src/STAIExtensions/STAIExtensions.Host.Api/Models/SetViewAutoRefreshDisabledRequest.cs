using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Host.Api.Models;

public class SetViewAutoRefreshDisabledRequest
{
    [Required]
    public string ViewId { get; set; } = "";

    public string OwnerId { get; set; } = "";
}