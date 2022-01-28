using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Default.Helpers;

internal class PeriodBoundary<T> where T : DataContractFull 
{
    public DateTime StartDate { get; set; } 
    
    public DateTime EndDate { get; set; }

    public PeriodBoundary(DateTime startDate, DateTime endDate)
    {
        this.StartDate = startDate;
        this.EndDate = endDate;
    }
    
    public IEnumerable<T> GetItems(IEnumerable<T>? source)
    {
        return source?.Where(s => s.TimeStamp >= this.StartDate && s.TimeStamp < this.EndDate) ?? new List<T>();
    }
    
}

