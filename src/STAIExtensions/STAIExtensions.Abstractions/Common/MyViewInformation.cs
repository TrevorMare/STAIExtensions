﻿namespace STAIExtensions.Abstractions.Common;

/// <summary>
/// Structure that is returned with the My Views information
/// </summary>
/// <param name="ViewId">The unique View Id</param>
/// <param name="ViewTypeName">The fully qualified view type</param>
public record class MyViewInformation(string ViewId, string ViewTypeName, string FriendlyViewTypeName)
{
    /// <summary>
    /// The unique View Id
    /// </summary>
    public string ViewId { get; } = ViewId;
    
    /// <summary>
    /// The fully qualified view type
    /// </summary>
    public string ViewTypeName { get; } = ViewTypeName;
    
    /// <summary>
    /// Gets or sets the friendly view type name
    /// </summary>
    public string FriendlyViewTypeName { get; } = FriendlyViewTypeName;
}