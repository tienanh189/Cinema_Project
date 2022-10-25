using CinemaAPI.Models.Dto;

namespace CinemaAPI.Respositories.Interface
{
    public interface ICategoryMovieRespository
    {
        public Task<List<CategoryMovieDto>> GetAll();
        public Task<CategoryMovieDto> GetById(Guid id);
        public Task<CategoryMovieDto> Create(CategoryMovieDto dto);
        public Task<CategoryMovieDto> Update(Guid id, CategoryMovieDto dto);
        public Task<bool> Delete(Guid id);
    }
}
