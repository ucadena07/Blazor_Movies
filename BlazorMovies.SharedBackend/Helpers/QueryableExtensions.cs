using BlazorMovies.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlazorMovies.SharedBackend.Helpers
{
    public static class QueryableExtensions
    {
        public async static Task<PaginatedResponse<List<T>>> GetPaginatedResponse<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
        {
            double count = await queryable.CountAsync();
            var totalAmountOfPages = (int)Math.Ceiling(count / paginationDto.RecordsPerPage);
            var records = await queryable.Paginate(paginationDto).ToListAsync();
            var response = new PaginatedResponse<List<T>>();
            response.TotalAmountPages = totalAmountOfPages;
            response.Response = records;
            return response;
        }
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
        {
            return queryable.Skip((paginationDto.Page - 1) * paginationDto.RecordsPerPage)
                .Take(paginationDto.RecordsPerPage);
        }
    }
}
