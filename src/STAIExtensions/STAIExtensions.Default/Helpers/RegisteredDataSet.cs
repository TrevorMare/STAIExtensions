using STAIExtensions.Abstractions.Data;
using STAIExtensions.Default.Interfaces;

namespace STAIExtensions.Default.Helpers;

public class RegisteredDataSet<T> : IRegisteredDataSet where T : IDataSet
{
    public Type DataSetType { get; private set; }
    
    public Delegate UpdateAction { get; private set; } 

    public RegisteredDataSet(Action<T> updateAction)
    {
        this.DataSetType = typeof(T);
        this.UpdateAction = updateAction;
    }
    
}
