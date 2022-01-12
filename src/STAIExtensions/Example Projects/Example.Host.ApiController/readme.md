## Examples: Host Api Controller

This is an example project that hosts the Api interface Controllers to interact with the DataSets and user Views. 

***WARNING***: The current configuration where the datasets retrieve information from is set up as the STAIExtensions Host and
may be removed at any time.

To run this example, build the project and run it. Test on the included swagger UI.

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/TrevorMare/STAIExtensions/.NET?style=for-the-badge)
![License](https://img.shields.io/github/license/trevormare/staiextensions?style=for-the-badge)


## Usage

For this example, the authorization is turned off and should be enabled when moving to production or anywhere where the service is exposed.

```c#
builder.Services.UseSTAIExtensionsApiHost(() => new ApiOptions() { UseAuthorization = false });
```