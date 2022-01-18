using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Host.Api.Models;


public class AttachViewToDatasetRequest
{
    /// <summary>
    /// The View Id to attach to the dataset 
    /// </summary>
    [Required]
    public string ViewId { get; set; } = "";
    
    /// <summary>
    /// The Data Set Id to attach the view to
    /// </summary>
    [Required]
    public string DataSetId { get; set; } = "";

    /// <summary>
    /// The Owner Id of the View
    /// </summary>
    public string OwnerId { get; set; } = "";
}