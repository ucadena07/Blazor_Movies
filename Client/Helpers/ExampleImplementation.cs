using BlazorMovies.SharedComponents;

namespace BlazorMovies.Client.Helpers
{
    public class ExampleImplementation : IExampleInterface
    {
        public string GetValue()
        {
            return "from web assembly";
        }
    }
}
