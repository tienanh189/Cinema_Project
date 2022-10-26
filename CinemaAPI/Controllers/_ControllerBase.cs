using CinemaAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CinemaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class _ControllerBase : ControllerBase
    {
        protected async Task<PaginatedResponse<T>> GetPaginatedResponse<T>(IQueryable<T> query, Pagination pagination)
        {
            if (pagination == null) pagination = new Pagination();

            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pagination.rowsPerPage);

            pagination.page = pagination.page < 1 ? 0 : pagination.page - 1;

            var results =(pagination.rowsPerPage <= 0
                ? query.AsEnumerable()
                : query.Skip(pagination.rowsPerPage * pagination.page)
                    .Take(pagination.rowsPerPage)
                    .AsEnumerable());

            var paginationHeader = new Pagination()
            {
                page = pagination.page,
                rowsPerPage = pagination.rowsPerPage,
                records = results.ToList().Count,
                totalItems = totalRecords,
                totalPages = totalPages
            };
            HttpContext.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

            return new PaginatedResponse<T>()
            {
                Pagination = paginationHeader,
                Data = results.ToList()
            };
        }
    }
}
