namespace STAIExtensions.Default.Interfaces;

public interface IRegisteredDataSet
{
    Type DataSetType { get; }
   
    Delegate UpdateAction { get; } 
}