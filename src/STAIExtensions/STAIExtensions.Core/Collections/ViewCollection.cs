using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Collections;

public class ViewCollection : Abstractions.Collections.IViewCollection
{

    #region Members

    private List<IDataSetView> _dataSetViewCollection = new();

    #endregion

    #region Properties

    public bool UseStrictUserSession { get; set; } = false;

    #endregion

    #region Methods
    public IDataSetView? GetView(string id, string userSessionId)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentNullException(nameof(id));

        if (UseStrictUserSession)
        {
            if (string.IsNullOrEmpty(userSessionId))
                throw new ArgumentNullException(nameof(userSessionId));

            return _dataSetViewCollection.FirstOrDefault(dsv =>
                string.Equals(dsv.Id, id, StringComparison.CurrentCultureIgnoreCase) && string.Equals(dsv.OwnerId,
                    userSessionId, StringComparison.CurrentCultureIgnoreCase));
        }
        
        return _dataSetViewCollection.FirstOrDefault(dsv => string.Equals(dsv.Id, id, StringComparison.CurrentCultureIgnoreCase));
    }

    public IDataSetView CreateView(string viewTypeName, string ownerId)
    {
        // Fully qualified type name
        var type = Type.GetType(viewTypeName);

        if (type == null)
            throw new Exception($"Unable to create view from type {viewTypeName}, View type not found");

        var resolvedInstance = Abstractions.DependencyExtensions.ServiceProvider?.GetService(type);
        if (resolvedInstance == null)
        {
            resolvedInstance = Activator.CreateInstance(type);
        }
        
        if (resolvedInstance == null)
            throw new Exception($"Unable to create view from type {viewTypeName}");

        IDataSetView instance = (IDataSetView) resolvedInstance;
        instance.OwnerId = ownerId;

        this._dataSetViewCollection.Add(instance);
    
        return instance;
    }
    #endregion
    
    
}