using System.Reflection;

namespace STAIExtensions.Abstractions.Common;

public static class EnumExtensions
{
    
    public static string DisplayName(this Enum enumValue) 
    {
        var displayAttribute = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>();
        if (displayAttribute == null || string.IsNullOrEmpty(displayAttribute?.Name))
            return enumValue.ToString();
        return displayAttribute.Name;
    }
    
    public static IEnumerable<Enum> GetFlags(this Enum enumValue)
    {
        foreach (Enum value in Enum.GetValues(enumValue.GetType()))
        {
            if (enumValue.HasFlag(value))
                yield return value;
        }
    }
    
}