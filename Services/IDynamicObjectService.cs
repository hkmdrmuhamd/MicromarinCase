using MicromarinCase.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MicromarinCase.Services
{
    public interface IDynamicObjectService
    {
        Task<List<ResultDto>> GettAllDynamicObjectAsync();
        Task CreateDynamicObjectAsync(CreateDto createDto);
        Task UpdateDynamicObjectAsync(UpdateDto updateDto);
        Task DeleteOrderAsync(int Id);
    }
}
