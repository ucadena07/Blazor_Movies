using Microsoft.JSInterop;

namespace BlazorMovies.SharedComponents
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.


    public static class ExampleJsInterop
    {

        public static async ValueTask<string> Prompt(this IJSRuntime js, string message)
        {
            return await js.InvokeAsync<string>("showPrompt", message);
        }


    }
}