using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Core.Collections;

namespace STAIExtensions.Core;

public static class StartupExtensions
{

    public static IServiceCollection UseSTAIExtensions(this IServiceCollection services,
        Func<Abstractions.Collections.DataSetCollectionOptions>? builderDatasetOptions = default,
        Func<Abstractions.Collections.ViewCollectionOptions>? builderViewCollectionOptions = default)
    {

        services.AddSingleton<Abstractions.Collections.IDataSetCollection, Collections.DataSetCollection>((s) =>
            new Collections.DataSetCollection(builderDatasetOptions?.Invoke()));

        services.AddSingleton<Abstractions.Collections.IViewCollection, Collections.ViewCollection>((s) =>
            new ViewCollection(builderViewCollectionOptions?.Invoke()));

        Abstractions.DependencyExtensions.UseSTAIExtensionsAbstractions(services);

        return services;
    }

}