using MathNet.Numerics.Statistics;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BlazorMovies.Server.Helpers
{
    public static class HttpContextExtensions
    {
        public static async Task InsertPaginationParametersInResponse<T>(this HttpContext httpContext, IQueryable<T> queryable, int recordsPerPage)
        {
            if(httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            double count = await queryable.CountAsync();
            double totalAmountOfPages = Math.Ceiling(count / recordsPerPage);
            httpContext.Response.Headers.Add("totalAmountPages", totalAmountOfPages.ToString());
        }
    }
}
