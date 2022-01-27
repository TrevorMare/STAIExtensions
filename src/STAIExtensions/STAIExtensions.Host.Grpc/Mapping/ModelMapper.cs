using System.Text.Json;
using Google.Protobuf.WellKnownTypes;

namespace STAIExtensions.Host.Grpc.Mapping;

internal static class ModelMapper
{

    public static string ConvertViewToJson(Abstractions.Views.IDataSetView? view)
    {
        if (view == null) return "";

        return JsonSerializer.Serialize(view);
    }

    public static IDataSetView ConvertViewToDataSetViewResponse(Abstractions.Views.IDataSetView? view)
    {
        if (view == null) return new IDataSetView();
        
        var result = new IDataSetView()
        {
            Id = view.Id,
            OwnerId = view.OwnerId,
            RefreshEnabled = view.RefreshEnabled,
            SlidingExpiration = Duration.FromTimeSpan(view.SlidingExpiration),
            ViewParameterDescriptors = { ConvertViewParameters(view.ViewParameterDescriptors) },
            ViewTypeName = view.ViewTypeName,
            FriendlyViewTypeName = view.FriendlyViewTypeName
        };
        
        if (view.ExpiryDate.HasValue)
            result.ExpiryDate = Timestamp.FromDateTime(view.ExpiryDate.Value.ToUniversalTime());

        if (view.LastUpdate.HasValue)
            result.LastUpdate = Timestamp.FromDateTime(view.LastUpdate.Value.ToUniversalTime());
        
        return result;
    }

    public static GetRegisteredViewsResponse ConvertViewInformationRegisteredViewsResponse(
        IEnumerable<Abstractions.Common.ViewInformation>? viewInformation)
    {
        if (viewInformation == null) return new GetRegisteredViewsResponse();
        
        return new GetRegisteredViewsResponse()
        {
            Items =
            {
                viewInformation.Select(x => new DataSetViewInformation()
                {
                    ViewName = x.ViewName,
                    ViewTypeName = x.ViewTypeName,
                    FriendlyViewTypeName = x.FriendlyViewTypeName,
                    DataSetViewParameterDescriptors = { ConvertViewParameters(x.DataSetViewParameterDescriptors) }
                })
            }
        };

    }
    
    public static IEnumerable<DataSetViewParameterDescriptor> ConvertViewParameters(
        IEnumerable<Abstractions.Views.DataSetViewParameterDescriptor>? viewParameters)
    {
        if (viewParameters == null) return new List<DataSetViewParameterDescriptor>();

        var result = viewParameters.Select(x => new DataSetViewParameterDescriptor()
        {
            Description = x.Description,
            Name = x.Name,
            Required = x.Required,
            Type = x.Type
        });

        return result;
    }

    public static ListDataSetsResponse ConvertDataSetInformation(
        IEnumerable<Abstractions.Common.DataSetInformation>? dataSetInformation)
    {
        if (dataSetInformation == null) return new ListDataSetsResponse();

        return new ListDataSetsResponse()
        {
            Items =
            {
                dataSetInformation.Select(x => new DataSetInformation()
                {
                    DataSetId = x.DataSetId,
                    DataSetName = x.DataSetName,
                    DataSetType = x.DataSetType,
                    FriendlyDataSetType = x.FriendlyDataSetType
                })
            }
        };
    }


}