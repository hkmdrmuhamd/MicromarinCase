using MicromarinCase.Dtos;

namespace MicromarinCase.Services
{
    public interface IDynamicObjectService
    {
        Task CreateOrderAsync(CreateOrderDto createDynamicObjectDto);
    }
}
