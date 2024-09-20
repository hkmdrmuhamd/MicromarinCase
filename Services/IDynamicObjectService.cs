using MicromarinCase.Dtos;

namespace MicromarinCase.Services
{
    public interface IDynamicObjectService
    {
        Task CreateOrderAsync(CreateDto createDynamicObjectDto);
    }
}
