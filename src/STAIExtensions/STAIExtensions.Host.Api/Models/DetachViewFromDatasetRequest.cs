using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Host.Api.Models;

public class DetachViewFromDatasetRequest
{
    
    /// <summary>
    /// The View Id to detach from the data set
    /// </summary>
    [Required]
    public string ViewId { get; set; } = "";
    
    /// <summary>
    /// The Data Set to detach from
    /// </summary>
    [Required]
    public string DataSetId { get; set; } = "";

    /// <summary>
    /// The owner Id of the Data Set
    /// </summary>
    public string OwnerId { get; set; } = "";
}