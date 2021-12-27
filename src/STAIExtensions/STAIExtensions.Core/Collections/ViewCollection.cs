using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Collections;

public class ViewCollection : Abstractions.Collections.IViewCollection
{

    #region Members

    private readonly List<IDataSetView> _dataSetViewCollection = new();
    private readonly ILogger<ViewCollection>? _logger;
    
    #endregion

    #region Properties

    public bool UseStrictUserSession { get; set; } = false;

    #endregion

    #region ctor

    public ViewCollection()
    {
        _logger = DependencyExtensions.CreateLogger<ViewCollection>();
    }

    #endregion
    
    #region Methods
    public IDataSetView? GetView(string id, string userSessionId)
    {
        try
        {
            IDataSetView? result = null;
            
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            if (UseStrictUserSession)
            {
                if (string.IsNullOrEmpty(userSessionId))
                    throw new ArgumentNullException(nameof(userSessionId));

                result = _dataSetViewCollection.FirstOrDefault(dsv =>
                    string.Equals(dsv.Id, id, StringComparison.CurrentCultureIgnoreCase) && string.Equals(dsv.OwnerId,
                        userSessionId, StringComparison.CurrentCultureIgnoreCase));
                result?.SetExpiryDate();
                return result;
            }
        
            result = _dataSetViewCollection.FirstOrDefault(dsv => string.Equals(dsv.Id, id, StringComparison.CurrentCultureIgnoreCase));
            result?.SetExpiryDate();
            return result;
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "An error occured retrieving view. {Error}", e);
            throw;
        }
    }

    public IDataSetView CreateView(string viewTypeName, string ownerId)
    {
        
        try
        {
            _logger?.LogInformation("Attempting to create view of type {ViewType} for owner {OwnerId}", 
                viewTypeName, ownerId);
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
        catch (Exception e)
        {
            _logger?.LogError(e, "An error occured creating the view. {Error}", e);
            throw;
        }
    }

    public IDataSetView? GetViewForUpdate(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
        
            return _dataSetViewCollection.FirstOrDefault(dsv => string.Equals(dsv.Id, id, StringComparison.CurrentCultureIgnoreCase));
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "An error occured retrieving view. {Error}", e);
            throw;
        }
    }

    public IEnumerable<IDataSetView> GetExpiredViews()
    {
        return _dataSetViewCollection.Where(vw => vw.ExpiryDate.HasValue && vw.ExpiryDate < DateTime.Now);
    }

    public void RemoveView(IDataSetView? expiredView)
    {
        if (expiredView == null) return;
        if (this._dataSetViewCollection.Contains(expiredView))
            this._dataSetViewCollection.Remove(expiredView);
    }

    public void RemoveView(string viewId)
    {
        throw new NotImplementedException();
    }

    #endregion
    
}