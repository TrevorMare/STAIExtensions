namespace STAIExtensions.Default.Views.BrowserTimingModels;

public class StatisticValues
{

    #region Properties
    public double? Minimum { get; set; }
    
    public double? Maximum { get; set; }
    
    public double? Average { get; set; }
    #endregion

    #region ctor

    public StatisticValues()
    {
        
    }

    public StatisticValues(List<double?>? items)
    {
        if (items == null) return;
        this.Minimum = items.Min();
        this.Maximum = items.Max();
        this.Average = items.Average();
    }
    #endregion
    
}