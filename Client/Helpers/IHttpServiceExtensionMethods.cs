using BlazorMovies.Shared.Dtos;
using System.Diagnostics;

namespace BlazorMovies.Client.Helpers
{
    public static class IHttpServiceExtensionMethods
    {
        public static async Task<T> GetHelper<T>(this IHttpService httpService, string url)
        {
            var response = await httpService.Get<T>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public static async Task<PaginatedResponse<T>> GetHelper<T>(this IHttpService httpService, string url, PaginationDto paginationDto)
        {
            string newUrl = "";

            if (url.Contains("?"))
            {
                newUrl = $"{url}&page={paginationDto.Page}&recordsPerPage={paginationDto.RecordsPerPage}";
            }
            else
            {
                newUrl = $"{url}?page={paginationDto.Page}&recordsPerPage={paginationDto.RecordsPerPage}";
            }
            var httpResponse = await httpService.Get<T>(newUrl);
            if (!httpResponse.Success)
            {
                throw new ApplicationException(await httpResponse.GetBody());
            }

            var totalAmountOfPages = int.Parse(httpResponse.HttpResponseMessage.Headers.GetValues("totalAmountPages").FirstOrDefault());

            var paginatedReponse = new PaginatedResponse<T>
            {
                Response = httpResponse.Response,
                TotalAmountPages = totalAmountOfPages
            };
            return paginatedReponse;
        }
    }


}
