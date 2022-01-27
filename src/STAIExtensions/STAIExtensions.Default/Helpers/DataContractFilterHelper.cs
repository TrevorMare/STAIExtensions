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
    

}