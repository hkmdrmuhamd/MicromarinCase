using MicromarinCase.Context;
using MicromarinCase.Dtos;
using MicromarinCase.Entities;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using MicromarinCase.JsonProcessing;

namespace MicromarinCase.Services
{
    public class DynamicObjectService : IDynamicObjectService
    {
        private readonly DatabaseContext _context;
        private readonly JsonProcessor _jsonProcessor;
        private readonly JsonValidator _jsonValidator;
        
        public DynamicObjectService(DatabaseContext context, JsonProcessor jsonProcessor, JsonValidator jsonValidator)
        {
            _context = context;
            _jsonProcessor = jsonProcessor;
            _jsonValidator = jsonValidator;
        }

        public async Task CreateDynamicObjectAsync(CreateDto createDto)
        {
            var processedJsonData = _jsonProcessor.ProcessJsonElement(createDto.DynamicObject, createDto.DynamicSubObject);
            _jsonValidator.ValidateJson(processedJsonData);
            await _context.Datas.AddAsync(new Data
            {
                JsonData = JsonConvert.SerializeObject(processedJsonData)
            });
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int Id)
        {
            var data = await _context.Datas.FindAsync(Id);
            if (data == null)
            {
                throw new Exception("Data not found");
            }
            _context.Datas.Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ResultDto>> GettAllDynamicObjectAsync()
        {
            var datas = await _context.Datas.ToListAsync();
            return datas.Select(p => new ResultDto
            {
                Id = p.Id,
                JsonData = p.JsonData
            }).ToList();
        }

        public async Task UpdateDynamicObjectAsync(UpdateDto updateDto)
        {
            var data = await _context.Datas.FindAsync(updateDto.Id);
            if (data == null)
            {
                throw new Exception("Data not found");
            }
            var processedJsonData = _jsonProcessor.ProcessJsonElement(updateDto.DynamicObject, updateDto.DynamicSubObject);
            _jsonValidator.ValidateJson(processedJsonData);
            data.JsonData = JsonConvert.SerializeObject(processedJsonData);
            await _context.SaveChangesAsync();
        }
    }
}