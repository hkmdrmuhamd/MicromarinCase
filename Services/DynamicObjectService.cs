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

        public async Task CreateDynamicObjectAsync(CreateDto createDto)
        {
            var processedJsonData = ProcessJsonElement(createDto.DynamicObject, createDto.DynamicSubObject);
            ValidateJson(processedJsonData);
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
            var processedJsonData = ProcessJsonElement(updateDto.DynamicObject, updateDto.DynamicSubObject);
            ValidateJson(processedJsonData);
            data.JsonData = JsonConvert.SerializeObject(processedJsonData);
            await _context.SaveChangesAsync();
        }

        private object ProcessJsonElement(Dictionary<string, object> dynamicObject, List<Dictionary<string, object>> dynamicSubObject)
        {
            var result = new Dictionary<string, object>();
            if (dynamicObject != null)
            {
                foreach (var property in dynamicObject)
                {
                    result[property.Key] = ProcessJsonElement(property.Value);
                }
            }

            if (dynamicSubObject != null)
            {
                foreach (var item in dynamicSubObject)
                {
                    var processedItem = ProcessJsonElement(item) as Dictionary<string, object>;
                    if (processedItem != null)
                    {
                        foreach (var keyValuePair in processedItem)
                        {
                            result[keyValuePair.Key] = keyValuePair.Value;
                        }
                    }
                }
            }

            return result;
        }

        private object ProcessJsonElement(object element)
        {
            if (element is JsonElement jsonElement)
            {
                switch (jsonElement.ValueKind)
                {
                    case JsonValueKind.Object:
                        var dictionary = new Dictionary<string, object>();
                        foreach (var property in jsonElement.EnumerateObject())
                        {
                            dictionary[property.Name] = ProcessJsonElement(property.Value);
                        }
                        return dictionary;

                    case JsonValueKind.Array:
                        var list = new List<object>();
                        foreach (var item in jsonElement.EnumerateArray())
                        {
                            list.Add(ProcessJsonElement(item));
                        }
                        return list;

                    case JsonValueKind.String:
                        return jsonElement.GetString();

                    case JsonValueKind.Number:
                        return jsonElement.TryGetInt64(out var l) ? l : (object)jsonElement.GetDecimal();

                    case JsonValueKind.True:
                        return true;

                    case JsonValueKind.False:
                        return false;

                    case JsonValueKind.Null:
                        return null;

                    default:
                        return jsonElement.ToString();
                }
            }
            // Eğer element zaten Dictionary<string, object> ise:
            if (element is Dictionary<string, object> dictionaryElement)
            {
                var processedDictionary = new Dictionary<string, object>();
                foreach (var kvp in dictionaryElement)
                {
                    processedDictionary[kvp.Key] = ProcessJsonElement(kvp.Value);
                }
                return processedDictionary;
            }
            // Eğer element List<Dictionary<string, object>> tipinde ise:
            if (element is List<Dictionary<string, object>> listElement)
            {
                var processedList = new List<object>();
                foreach (var item in listElement)
                {
                    processedList.Add(ProcessJsonElement(item));
                }
                return processedList;
            }

            return element;
        }

        private void ValidateJson(object jsonData)
        {
            if (jsonData is not IDictionary<string, object> jsonDictionary)
            {
                throw new ArgumentException("Invalid JSON data format.");
            }

            if (jsonDictionary.ContainsKey("Customer"))
            {
                var customerData = jsonDictionary["Customer"] as IDictionary<string, object>;
                if (customerData == null)
                {
                    throw new ArgumentException("The Customer field is not in a valid JSON format.");
                }

                var requiredFields = new List<string> { "CustomerId", "Name", "Password" };
                foreach (var field in requiredFields)
                {
                    if (!customerData.ContainsKey(field))
                    {
                        throw new ArgumentException($"Customer field is missing: ‘{field}’ is required.");
                    }
                }
            }

            if (jsonDictionary.ContainsKey("Product"))
            {
                var productData = jsonDictionary["Product"] as List<object>;
                if (productData == null)
                {
                    throw new ArgumentException("The Product field is not in a valid JSON format.");
                }

                foreach (var product in productData)
                {
                    var productFields = product as IDictionary<string, object>;
                    if (productFields == null)
                    {
                        throw new ArgumentException("The elements in Product are not in a valid JSON format.");
                    }

                    var requiredFields = new List<string> { "Name", "Price" };
                    foreach (var field in requiredFields)
                    {
                        if (!productFields.ContainsKey(field))
                        {
                            throw new ArgumentException($"Product field missing: ‘{field}’ is required.");
                        }
                    }
                }
            }
        }
    }
}