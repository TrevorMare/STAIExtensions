using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Host.Api.Models;

public class DetachViewFromDatasetRequest
{
    [Required]
    public string ViewId { get; set; } = "";
    
    [Required]
    public string DataSetId { get; set; } = "";

    public string OwnerId { get; set; } = "";
}