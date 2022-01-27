using System.Reflection;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.Collections;

/// <summary>
/// Default implementation of the View Collection. This class manages all the views
/// that are created within the system
/// </summary>
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

    /// <summary>
    /// Gets the current number of active views in the collection
    /// </summary>
    public int ViewCount => _dataSetViewCollection.Count;
    
    /// <summary>
    /// Event that is fired when a DataSetView object is updated
    /// </summary>
    public event IViewCollection.OnDataSetViewUpdatedHandler? OnDataSetViewUpdated;
    
    /// <summary>
    /// Gets a value from the options indicating if Views should expire after a period. The expiration of
    /// a view is reset each time the GetView <see cref="GetView"/> is called. 
    /// </summary>
    public bool ViewsExpire => _options.ViewsExpire;

    /// <summary>
    /// Gets a value from the options indicating if the owner Id of a view can be blank or empty. This will allow
    /// for shared views between users but might have some complications when shared parameters are set.
    /// </summary>
    public bool UseStrictViews => _options.UseStrictViews ?? false;

    /// <summary>
    /// Gets a value from the options indicating the total number of allowed views that can be created globally
    /// </summary>
    public int? MaximumViews => _options.MaximumViews;

    /// <summary>
    /// Gets or sets the sliding expiration timespan on the View that will be added on each View Retrieval <see cref="GetView"/>
    /// The view is initialised with from the value passed in the options. 
    /// </summary>
    public TimeSpan? DefaultSlidingExpiryTimeSpan => _options.SlidingExpirationTimeSpan;
    #endregion
    
    #region Methods
    /// <summary>
    /// Retrieves an active view in it's current state. This method will reset the expiration date and will have to be called
    /// within the time frame specified to keep the view alive.
    /// </summary>
    /// <param name="id">The unique Id of the View</param>
    /// <param name="ownerId">The optional owner identifier that created the view</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
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

    /// <summary>
    /// Creates a new user instance view of the specified type 
    /// </summary>
    /// <param name="viewTypeName">The fully qualified type name of of the view to create an instance of</param>
    /// <param name="ownerId">The optional user/owner Id of this view</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
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
            {
                _logger?.LogInformation("View type {ViewType} not found in fully qualified namespace. Attempting to resolve with find", 
                    viewTypeName, ownerId);

                var assemblyTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .Where(t => string.Equals(t.Name, viewTypeName, StringComparison.OrdinalIgnoreCase)).ToArray();
                
                if (!assemblyTypes.Any())
                    throw new Exception($"Unable to create view from type {viewTypeName}, View type not found");
                if (assemblyTypes.Count() > 1)
                    throw new Exception($"Multiple types found for type {viewTypeName}, please use fully qualified name");
                
                type = assemblyTypes[0];
            }

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

    /// <summary>
    /// Internal method that will retrieve a view without the Owner id specified. This
    /// should only be used to retrieve a view from the collection for update logic 
    /// </summary>
    /// <param name="id">The View Id that requires an update</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
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

    /// <summary>
    /// Gets a list of views that has expired and needs to be removed from the collection
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IDataSetView> GetExpiredViews()
    {
        return ViewsExpire == false
            ? new List<IDataSetView>()
            : _dataSetViewCollection.Where(vw =>
                vw.ExpiryDate.HasValue && vw.ExpiryDate < DateTime.Now && vw.RefreshEnabled == true);
    }

    /// <summary>
    /// Removes a view from the collection
    /// </summary>
    /// <param name="expiredView">The view to remove</param>
    /// <exception cref="ArgumentNullException"></exception>
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

    /// <summary>
    /// Removes a view from the collection
    /// </summary>
    /// <param name="viewId">The unique view Id to remove</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void RemoveView(string viewId)
    {
        if (string.IsNullOrEmpty(viewId) || viewId.Trim() == "")
            throw new ArgumentNullException(nameof(viewId));
        
        var view = this._dataSetViewCollection.FirstOrDefault(x =>
            string.Equals(x.Id.Trim(), viewId.Trim(), StringComparison.OrdinalIgnoreCase));
        
        RemoveView(view);
    }

    /// <summary>
    /// Sets the View Parameters.
    /// </summary>
    /// <param name="viewId">The unique View Id to set the parameters on</param>
    /// <param name="ownerId">The owner of the view</param>
    /// <param name="viewParameters">The parameter values to set on the view</param>
    public void SetViewParameters(string viewId, string? ownerId,
        Dictionary<string, object>? viewParameters)
    {
        var view = this.GetView(viewId, ownerId);

        if (view == null) return;
        
        view.SetViewParameters(viewParameters);
    }

    /// <summary>
    /// Gets a list of views from the internal collection by Owner id
    /// </summary>
    /// <param name="ownerId">The owner Id to find the views for</param>
    /// <returns></returns>
    public IEnumerable<MyViewInformation> GetMyViews(string ownerId)
    {
        return this._dataSetViewCollection
            .Where(vw => string.Equals(ownerId.Trim(), vw.Id.Trim(), StringComparison.OrdinalIgnoreCase))
            .Select(x => new MyViewInformation(x.Id, x.ViewTypeName, x.FriendlyViewTypeName));
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