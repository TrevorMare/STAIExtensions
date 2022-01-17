using System.Reflection;

namespace STAIExtensions.Abstractions.Common;

/// <summary>
/// Helper methods for Enums
/// </summary>
public static class EnumExtensions
{
    
    /// <summary>
    /// Retrieves a Display attribute value from an Enum object
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Retrieves the flags from Enum value
    /// </summary>
    /// <param name="enumValue">The input Enum Flags</param>
    /// <returns></returns>
    public static IEnumerable<Enum?> GetFlags(this Enum enumValue)
    {
        return Enum.GetValues(enumValue.GetType()).Cast<Enum?>()
            .Where(value => value != null && enumValue.HasFlag(value));
    }
    
}