using MicromarinCase.Dtos;
using MicromarinCase.Services;
using Microsoft.AspNetCore.Mvc;

namespace MicromarinCase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly IDynamicObjectService _dynamicObjectService;
        public BaseController(IDynamicObjectService dynamicObjectService)
        {
            _dynamicObjectService = dynamicObjectService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDynamicObject(CreateDto createOrderDto)
        {
            try
            {
                await _dynamicObjectService.CreateOrderAsync(createOrderDto);
                return Ok("Dynamic object successfully added");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
