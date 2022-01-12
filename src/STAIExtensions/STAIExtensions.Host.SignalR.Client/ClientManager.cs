using Microsoft.AspNetCore.SignalR.Client;

namespace STAIExtensions.Host.SignalR.Client;

public class ClientManager
{

    #region Members

    private readonly HubConnection? _connection;
    private readonly string _ownerId;

    #endregion

    #region ctor

    public ClientManager()
    {
        _ownerId = Guid.NewGuid().ToString();
        
        var connection = new HubConnectionBuilder()
            .WithUrl("https://example.com/chathub", options =>
            { 
                options.AccessTokenProvider = () => Task.FromResult("_myAccessToken");
            })
            .WithAutomaticReconnect()
            .Build();
    }

    #endregion

    public async Task CreateView(string viewType)
    {
        var callbackId = Guid.NewGuid().ToString();

        await _connection?.SendCoreAsync(nameof(CreateView), new []{ viewType, _ownerId, callbackId } );

        _connection.On("", () => { });
    }
    
    #region Methods

    private void Test()
    {
        _connection?.StartAsync().ConfigureAwait(false);
        

        
        

    }
    

    #endregion
    
    
}