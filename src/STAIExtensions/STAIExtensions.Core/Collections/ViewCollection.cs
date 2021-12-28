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
    private readonly ViewCollectionOptions _options;
    
    #endregion

    #region ctor

    public ViewCollection(ViewCollectionOptions? options)
    {
        _logger = DependencyExtensions.CreateLogger<ViewCollection>();
        _options = options ?? new ViewCollectionOptions();
    }
    #endregion

    #region Properties

    public bool ViewsExpire => _options.ViewsExpire;

    #endregion
    
    #region Methods
    public IDataSetView? GetView(string id, string ownerId)
    {
        try
        {
            IDataSetView? result = null;
            
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            if (_options.UseStrictViews == true && string.IsNullOrEmpty(ownerId))
                throw new ArgumentNullException(nameof(ownerId));

            if (_options.UseStrictViews == true)
            {
                if (string.IsNullOrEmpty(ownerId))
                    throw new ArgumentNullException(nameof(ownerId));

                result = _dataSetViewCollection.FirstOrDefault(dsv =>
                    string.Equals(dsv.Id, id, StringComparison.CurrentCultureIgnoreCase) && string.Equals(dsv.OwnerId,
                        ownerId, StringComparison.CurrentCultureIgnoreCase));
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
            if (_options.UseStrictViews == true && string.IsNullOrEmpty(ownerId))
                throw new ArgumentNullException(nameof(ownerId));

            if (_options.MaximumViews.HasValue && _dataSetViewCollection.Count >= _options.MaximumViews.Value)
                throw new Exception($"Maximum number of allowed ({_options.MaximumViews.Value}) views reached");
            
            
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
            if (_options.DefaultViewExpiryDate.HasValue)
            {
                instance.SlidingExpiration = _options.DefaultViewExpiryDate.Value;
            }

            lock (_dataSetViewCollection)
            {
                this._dataSetViewCollection.Add(instance);    
            }
    
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
        return ViewsExpire == false ? new List<IDataSetView>() : _dataSetViewCollection.Where(vw => vw.ExpiryDate.HasValue && vw.ExpiryDate < DateTime.Now);
    }

    public void RemoveView(IDataSetView? expiredView)
    {
        if (expiredView == null) return;

        _logger?.LogInformation("Removing view with Id {ViewId}", expiredView.Id);
        
        lock (_dataSetViewCollection)
        {
            if (this._dataSetViewCollection.Contains(expiredView))
                this._dataSetViewCollection.Remove(expiredView);
        }
    }

    public void RemoveView(string viewId)
    {
        if (string.IsNullOrEmpty(viewId))
            return;
        var view = this._dataSetViewCollection.FirstOrDefault(x =>
            string.Equals(x.Id, viewId, StringComparison.OrdinalIgnoreCase));
        RemoveView(view);
    }

    #endregion
    
}