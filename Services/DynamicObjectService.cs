using MicromarinCase.Context;
using MicromarinCase.Dtos;
using MicromarinCase.Entities;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace MicromarinCase.Services
{
    public class DynamicObjectService : IDynamicObjectService
    {
        private readonly DatabaseContext _context;
        public DynamicObjectService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateDynamicObjectAsync(CreateDto createDynamicObjectDto)
        {
            var dynamicObject = new DynamicObjectEntities(createDynamicObjectDto.DynamicObject);
            var dynamicSubObjects = new List<DynamicSubObjectEntities>();

            foreach (var dynamicSubObjectsData in createDynamicObjectDto.DynamicSubObject)
            {
                var dynamicSubObject = new DynamicSubObjectEntities(dynamicSubObjectsData);
                dynamicSubObjects.Add(dynamicSubObject);
            }
            var jsonData = new Dictionary<string, object>
            {
                { "DynamicObject", dynamicObject.DynamicFields.ToDictionary(
                    field => field.Key,
                    field => field.Value is JsonElement jsonElement ?
                        jsonElement.ValueKind switch
                        {
                            JsonValueKind.String => jsonElement.GetString(),
                            JsonValueKind.Number => jsonElement.TryGetInt32(out var intValue) ? (object)intValue : jsonElement.GetDecimal(),
                            JsonValueKind.True => true,
                            JsonValueKind.False => false,
                            JsonValueKind.Null => null,
                            _ => jsonElement.ToString()
                        }
                    : field.Value
                )},
                { "DynamicSubObject", dynamicSubObjects.Select(p => new
                    {
                        p.Id,
                        DynamicFields = p.DynamicFields.ToDictionary(
                            field => field.Key,
                            field => field.Value is JsonElement jsonElement ?
                                jsonElement.ValueKind switch
                                {
                                    JsonValueKind.String => jsonElement.GetString(),
                                    JsonValueKind.Number => jsonElement.TryGetInt32(out var intValue) ? (object)intValue : jsonElement.GetDecimal(),
                                    JsonValueKind.True => true,
                                    JsonValueKind.False => false,
                                    JsonValueKind.Null => null,
                                    _ => jsonElement.ToString()
                                }
                            : field.Value
                        )
                    }).ToList()
                }
            };

            await _context.Datas.AddAsync(new Data
            {
                JsonData = JsonConvert.SerializeObject(jsonData)
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

        public async Task UpdateDynamicObjectAsync(UpdateDto updateDynamicObjectDto)
        {
            var data = await _context.Datas.FindAsync(updateDynamicObjectDto.Id);
            if (data == null)
            {
                throw new Exception("Data not found");
            }

            var dynamicObject = new DynamicObjectEntities(updateDynamicObjectDto.DynamicObject);
            var dynamicSubObjects = new List<DynamicSubObjectEntities>();

            foreach (var dynamicSubObjectsData in updateDynamicObjectDto.DynamicSubObject)
            {
                var dynamicSubObject = new DynamicSubObjectEntities(dynamicSubObjectsData);
                dynamicSubObjects.Add(dynamicSubObject);
            }
            var jsonData = new Dictionary<string, object>
            {
                { "DynamicObject", dynamicObject.DynamicFields.ToDictionary(
                    field => field.Key,
                    field => field.Value is JsonElement jsonElement ?
                        jsonElement.ValueKind switch
                        {
                            JsonValueKind.String => jsonElement.GetString(),
                            JsonValueKind.Number => jsonElement.TryGetInt32(out var intValue) ? (object)intValue : jsonElement.GetDecimal(),
                            JsonValueKind.True => true,
                            JsonValueKind.False => false,
                            JsonValueKind.Null => null,
                            _ => jsonElement.ToString()
                        }
                    : field.Value
                    )},
                { "DynamicSubObject", dynamicSubObjects.Select(p => new
                    {
                        p.Id,
                        DynamicFields = p.DynamicFields.ToDictionary(
                            field => field.Key,
                            field => field.Value is JsonElement jsonElement ?
                                jsonElement.ValueKind switch
                                {
                                    JsonValueKind.String => jsonElement.GetString(),
                                    JsonValueKind.Number => jsonElement.TryGetInt32(out var intValue) ? (object)intValue : jsonElement.GetDecimal(),
                                    JsonValueKind.True => true,
                                    JsonValueKind.False => false,
                                    JsonValueKind.Null => null,
                                    _ => jsonElement.ToString()
                                }
                            : field.Value
                        )
                    }).ToList()
                }
            };
            data.JsonData = JsonConvert.SerializeObject(jsonData);
            await _context.SaveChangesAsync();
        }
    }
}
