﻿using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.Views;

public interface IDataSetView : IDisposable
{
    
    DateTime? ExpiryDate { get; }

    TimeSpan SlidingExpiration { get; set; }

    event EventHandler OnDisposing;
    
    event EventHandler OnViewUpdated;

    Task OnDataSetUpdated(IDataSet dataset);

    void SetExpiryDate();

    void SetExpiryDate(DateTime value);
}