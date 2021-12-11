using System.ComponentModel.DataAnnotations;

namespace STAIExtensions.Abstractions.Common;

[Flags]
public enum AzureApiDataContractSource
{
    [Display(Name = "availabilityResults")]
    Availability = 0x0,
    
    [Display(Name = "browserTimings")]
    BrowserTiming = 0x1,
    
    [Display(Name = "customEvents")]
    CustomEvent = 0x2,
    
    [Display(Name = "customMetrics")]
    CustomMetric = 0x4,
    
    [Display(Name = "dependencies")]
    Dependency = 0x8,
    
    [Display(Name = "exceptions")]
    Exception = 0x16,
    
    [Display(Name = "pageViews")]
    PageViews = 0x32,
    
    [Display(Name = "performanceCounters")]
    PerformanceCounter = 0x64,
    
    [Display(Name = "requests")]
    Request = 0x128,
    
    [Display(Name = "traces")]
    Trace = 0x256,
    
    [Display(Name = "all")]
    All = 0x512,

}
