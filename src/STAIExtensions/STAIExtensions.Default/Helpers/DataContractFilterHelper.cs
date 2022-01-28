using System.Collections;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Default.Helpers;

public static class DataContractFullFilterHelper
{

    public static IEnumerable<T> FilterCloudRoleInstance<T>(this IEnumerable<T> input, List<string>? filterValues) where T : DataContractFull
    {
        if (filterValues == null) return input;
        return input.Where(record => filterValues.Contains(record.CloudRoleInstance ?? ""));
    }
    
    public static IEnumerable<T> FilterCloudRoleName<T>(this IEnumerable<T> input, List<string>? filterValues) where T : DataContractFull
    {
        if (filterValues == null) return input;
        return input.Where(record => filterValues.Contains(record.CloudRoleName ?? ""));
    }
    
    public static IEnumerable<T> FilterTimeStamp<T>(this IEnumerable<T> input, DateTime? startDateTime, DateTime? endDateTime) where T : DataContractFull
    {
        if (startDateTime == null && endDateTime == null) return input;
        
        if (startDateTime != null && endDateTime != null)
            return input.Where(record => record.TimeStamp >= startDateTime.Value && record.TimeStamp <= endDateTime.Value);
        
        if (startDateTime != null)
            return input.Where(record => record.TimeStamp >= startDateTime.Value);
        
        return endDateTime != null ? input.Where(record => record.TimeStamp <= endDateTime.Value) : input;
    }
    

}