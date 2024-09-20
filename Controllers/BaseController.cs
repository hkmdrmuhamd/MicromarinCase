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

        [HttpGet]
        public async Task<IActionResult> GetAllDynamicObject()
        {
            try
            {
                var result = await _dynamicObjectService.GettAllDynamicObjectAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDynamicObject(CreateDto createDto)
        {
            try
            {
                await _dynamicObjectService.CreateDynamicObjectAsync(createDto);
                return Ok("Dynamic object successfully added");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDynamicObject(UpdateDto updateDto)
        {
            try
            {
                await _dynamicObjectService.UpdateDynamicObjectAsync(updateDto);
                return Ok("Dynamic object successfully updated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDynamicObject(int Id)
        {
            try
            {
                await _dynamicObjectService.DeleteOrderAsync(Id);
                return Ok("Dynamic object successfully deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
