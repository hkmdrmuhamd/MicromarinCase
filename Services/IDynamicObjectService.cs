using MicromarinCase.Dtos;

namespace MicromarinCase.Services
{
    public interface IDynamicObjectService
    {
        Task<List<ResultDto>> GettAllDynamicObjectAsync();
        Task CreateDynamicObjectAsync(CreateDto createDynamicObjectDto);
        Task UpdateDynamicObjectAsync(UpdateDto updateDynamicObjectDto);
        Task DeleteOrderAsync(int Id);
    }
}
