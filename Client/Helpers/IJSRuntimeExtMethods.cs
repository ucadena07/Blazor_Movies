using Microsoft.JSInterop;

namespace BlazorMovies.Client.Helpers
{
    public static class IJSRuntimeExtMethods
    {
        public static async ValueTask<bool> Confirm(this IJSRuntime js, string message)
        {
            return await js.InvokeAsync<bool>("confirm",message);
        }

        public static async ValueTask MyFunction(this IJSRuntime js, string message)
        {
            await js.InvokeVoidAsync("my_function", message);
        }

        public static async ValueTask ArrowFun(this IJSRuntime js, string message)
        {
            await js.InvokeVoidAsync("arrow_function", message);
        }

    }
}
