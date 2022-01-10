## STAIExtensions Abstractions

This library is the root that defines the interfaces for queries, datasets, dataset views, Application Insights data contract models, the collection management interfaces
and lastly the CQRS pattern to update the objects. The rest of the projects rely heavily on this
project for the dataflow and shared structures.

It also defines the basic general interaction with the interfaces via the MediatR package required to keep the collections in sync and register for UI interactions. All projects
using this library and the core library should use the MediatR object for commands and queries. For this to work, the MediatR package
needs to be registered in the service collection. If you are using this library on a direct reference without the Core library, you would need
to register the services by calling the **DependencyExtensions.UseSTAIExtensionsAbstractions** method.

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)
