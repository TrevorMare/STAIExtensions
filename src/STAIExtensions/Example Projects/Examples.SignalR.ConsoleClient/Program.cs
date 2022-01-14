// See https://aka.ms/new-console-template for more information

using STAIExtensions.Host.SignalR.Client;

Console.WriteLine("Hello, SignalR World!");

var clientManager =
    new SignalRClientManaged(new SignalRClientManagedOptions("https://localhost:5001/STAIExtensionsHub",
        "Trevor Mare")
    {
        AuthBearerToken = "1a99436ef0e79d26ada7bb20e675a27d3fe13d91156624e9f50ec428d71e8495", 
        UseDefaultAuthorization = true
    });

try
{
    var viewToCreateName = "STAIExtensions.Default.Views.BrowserTimingsView, STAIExtensions.Default, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
    

    clientManager.OnDataSetUpdated += (s, e) =>
    {
        Console.WriteLine($"DataSet with Id {e} updated.");
    };
    
    clientManager.OnDataViewUpdated += (s, e) =>
    {
        Console.WriteLine($"View Data: {e.FullJson}");
    };

    await clientManager.ConnectAsync();
    
    
    var dataSets = await clientManager.ListDataSets();
    
    if (dataSets == null) throw new Exception("Could not locate any DataSets");
    
    var view = await clientManager.CreateView(viewToCreateName);
    
    if (view == null) Console.WriteLine("View Could not be created");

    await clientManager.AttachViewToDataset(view.View.Id, dataSets.ToList()[0].DataSetId);


}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

Console.ReadLine();
