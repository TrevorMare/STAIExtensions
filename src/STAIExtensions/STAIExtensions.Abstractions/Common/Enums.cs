using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Abstractions.Common;

public enum AzureApiDataContractSource
{
    [Display(Name = "availabilityResults")]
    Availability = 0,
    
    [Display(Name = "browserTimings")]
    BrowserTiming = 1,
    
    [Display(Name = "customEvents")]
    CustomEvent = 2,
    
    [Display(Name = "customMetrics")]
    CustomMetric = 3,
    
    [Display(Name = "dependencies")]
    Dependency = 4,
    
    [Display(Name = "exceptions")]
    Exception = 5,
    
    [Display(Name = "pageViews")]
    PageViews = 6,
    
    [Display(Name = "performanceCounters")]
    PerformanceCounter = 7,
    
    [Display(Name = "requests")]
    Request = 8,
    
    [Display(Name = "traces")]
    Trace = 9,

}
