using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Common;
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

    public int ViewCount => _dataSetViewCollection.Count;
    
    public event IViewCollection.OnDataSetViewUpdatedHandler? OnDataSetViewUpdated;
    
    public bool ViewsExpire => _options.ViewsExpire;

    public bool UseStrictViews => _options.UseStrictViews ?? false;

    public int? MaximumViews => _options.MaximumViews;

    public TimeSpan? DefaultSlidingExpiryTimeSpan => _options.SlidingExpirationTimeSpan;
    #endregion
    
    #region Methods
    public IDataSetView? GetView(string id, string? ownerId)
    {
        try
        {
            IDataSetView? result = null;
            
            if (string.IsNullOrEmpty(id) || id.Trim() == "")
                throw new ArgumentNullException(nameof(id));

            if (UseStrictViews == true && (string.IsNullOrEmpty(ownerId) || ownerId.Trim() == ""))
                throw new ArgumentNullException(nameof(ownerId));

            if (_options.UseStrictViews == true)
            {
                result = _dataSetViewCollection.FirstOrDefault(dsv =>
                    string.Equals(dsv.Id.Trim(), id.Trim(), StringComparison.CurrentCultureIgnoreCase) && string.Equals(dsv.OwnerId.Trim(),
                        ownerId.Trim(), StringComparison.CurrentCultureIgnoreCase));
                result?.SetExpiryDate();
                return result;
            }
        
            result = _dataSetViewCollection.FirstOrDefault(dsv => string.Equals(dsv.Id.Trim(), id.Trim(), StringComparison.CurrentCultureIgnoreCase));
            result?.SetExpiryDate();
            return result;
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "An error occured retrieving view. {Error}", e);
            return null;
        }
    }

    public IDataSetView CreateView(string viewTypeName, string? ownerId)
    {
        
        try
        {
            if (string.IsNullOrEmpty(viewTypeName) || viewTypeName.Trim() == "")
                throw new ArgumentNullException(nameof(viewTypeName));
            
            if (UseStrictViews == true && string.IsNullOrEmpty(ownerId) || ownerId?.Trim() == "")
                throw new ArgumentNullException(nameof(ownerId));


            if (MaximumViews.HasValue && _dataSetViewCollection.Count >= MaximumViews.Value)
                throw new Exception($"Maximum number of allowed ({MaximumViews.Value}) views reached");

            viewTypeName = viewTypeName.Trim();
            ownerId = ownerId?.Trim();

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
            if (DefaultSlidingExpiryTimeSpan.HasValue)
            {
                instance.SlidingExpiration = DefaultSlidingExpiryTimeSpan.Value;
            }
            instance.SetExpiryDate();

            instance.OnViewUpdated += OnDataSetViewObjectUpdated;
            
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
            if (string.IsNullOrEmpty(id) || id.Trim() == "")
                throw new ArgumentNullException(nameof(id));
        
            return _dataSetViewCollection.FirstOrDefault(dsv => string.Equals(dsv.Id.Trim(), id.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "An error occured retrieving view. {Error}", e);
            throw;
        }
    }

    public IEnumerable<IDataSetView> GetExpiredViews()
    {
        return ViewsExpire == false
            ? new List<IDataSetView>()
            : _dataSetViewCollection.Where(vw =>
                vw.ExpiryDate.HasValue && vw.ExpiryDate < DateTime.Now && vw.RefreshEnabled == true);
    }

    public void RemoveView(IDataSetView? expiredView)
    {
        if (expiredView == null) 
            throw new ArgumentNullException(nameof(expiredView));

        _logger?.LogInformation("Removing view with Id {ViewId}", expiredView.Id);

        if (!this._dataSetViewCollection.Contains(expiredView)) return;
        
        lock (_dataSetViewCollection)
        {
            expiredView.OnViewUpdated -= OnDataSetViewObjectUpdated;
            this._dataSetViewCollection.Remove(expiredView);
        }
    }

    public void RemoveView(string viewId)
    {
        if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
            throw new ArgumentNullException(nameof(viewId));
        
        var view = this._dataSetViewCollection.FirstOrDefault(x =>
            string.Equals(x.Id.Trim(), viewId.Trim(), StringComparison.OrdinalIgnoreCase));
        
        RemoveView(view);
    }

    public void SetViewParameters(string viewId, string? ownerId,
        Dictionary<string, object>? viewParameters)
    {
        var view = this.GetView(viewId, ownerId);

        if (view == null) return;
        
        view.SetViewParameters(viewParameters);
    }

    public IEnumerable<MyViewInformation> GetMyViews(string ownerId)
    {
        return this._dataSetViewCollection
            .Where(vw => string.Equals(ownerId.Trim(), vw.Id.Trim(), StringComparison.OrdinalIgnoreCase))
            .Select(x => new MyViewInformation(x.Id, x.ViewTypeName));
    }
    #endregion

    #region Private Methods

    private void OnDataSetViewObjectUpdated(object? sender, EventArgs e)
    {
        if (sender is not IDataSetView dataSet)
            return;
        
        OnDataSetViewUpdated?.Invoke(dataSet, EventArgs.Empty);
    }

    #endregion
}