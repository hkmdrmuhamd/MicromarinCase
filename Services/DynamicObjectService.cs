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

        private object ProcessJsonElement(Dictionary<string, object> dynamicObject, List<Dictionary<string, object>> dynamicSubObject)
        {
            var result = new Dictionary<string, object>();

            if (dynamicObject != null)
            {
                var processedDynamicObject = new Dictionary<string, object>();
                foreach (var property in dynamicObject)
                {
                    processedDynamicObject[property.Key] = ProcessJsonElement(property.Value);
                }
                result["DynamicObject"] = processedDynamicObject; 
            }

            if (dynamicSubObject != null)
            {
                var subObjectsList = new List<object>();
                foreach (var item in dynamicSubObject)
                {
                    var processedItem = ProcessJsonElement(item) as Dictionary<string, object>;
                    if (processedItem != null)
                    {
                        subObjectsList.Add(processedItem);
                    }
                }
                result["DynamicSubObject"] = subObjectsList;
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

            if (element is Dictionary<string, object> dictionaryElement)
            {
                var processedDictionary = new Dictionary<string, object>();
                foreach (var kvp in dictionaryElement)
                {
                    processedDictionary[kvp.Key] = ProcessJsonElement(kvp.Value);
                }
                return processedDictionary;
            }

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

            if (jsonDictionary.ContainsKey("DynamicObject"))
            {
                var dynamicObject = jsonDictionary["DynamicObject"] as IDictionary<string, object>;
                if (dynamicObject.Count == 0)
                {
                    throw new ArgumentException("The DynamicObject field is not in a valid JSON format.");
                }

                if (dynamicObject.ContainsKey("Customer"))
                {
                    var customerData = dynamicObject["Customer"] as IDictionary<string, object>;
                    if (customerData == null)
                    {
                        throw new ArgumentException("The Customer field is not in a valid JSON format.");
                    }

                    var requiredCustomerFields = new List<string> { "CustomerId", "Name", "Password" };
                    foreach (var field in requiredCustomerFields)
                    {
                        if (!customerData.ContainsKey(field))
                        {
                            throw new ArgumentException($"Customer field is missing: '{field}' is required.");
                        }
                    }

                    if (jsonDictionary.ContainsKey("DynamicSubObject"))
                    {
                        var dynamicSubObject = jsonDictionary["DynamicSubObject"] as List<object>;
                        if (dynamicSubObject.Count == 0)
                        {
                            throw new ArgumentException("The DynamicSubObject field is not in a valid JSON format.");
                        }

                        foreach (var subObject in dynamicSubObject)
                        {
                            var orderData = (subObject as IDictionary<string, object>)?["Order"] as IDictionary<string, object>;
                            if (orderData == null)
                            {
                                throw new ArgumentException("Each DynamicSubObject must contain a valid Order field.");
                            }

                            var requiredOrderFields = new List<string> { "CustomerId" };
                            foreach (var field in requiredOrderFields)
                            {
                                if (!orderData.ContainsKey(field))
                                {
                                    throw new ArgumentException($"Order field is missing: '{field}' is required.");
                                }
                            }

                            if (orderData.ContainsKey("Product"))
                            {
                                var productData = orderData["Product"] as List<object>;
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

                                    var requiredProductFields = new List<string> { "Name", "Price", "Quantity" };
                                    foreach (var field in requiredProductFields)
                                    {
                                        if (!productFields.ContainsKey(field))
                                        {
                                            throw new ArgumentException($"Product field missing: '{field}' is required.");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                throw new ArgumentException("Each Order must contain at least one Product field.");
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("The DynamicObject containing a Customer must contain a DynamicSubObject containing at least an Order and a Product.");
                    }
                }
                

                if (dynamicObject.ContainsKey("Order"))
                {
                    var orderData = dynamicObject["Order"] as IDictionary<string, object>;
                    if (orderData == null)
                    {
                        throw new ArgumentException("The Order field is not in a valid JSON format.");
                    }

                    if (!orderData.ContainsKey("CustomerId"))
                    {
                        throw new ArgumentException("Order field must contain 'CustomerId'.");
                    }

                    if (jsonDictionary.ContainsKey("DynamicSubObject"))
                    {
                        var dynamicSubObject = jsonDictionary["DynamicSubObject"] as List<object>;
                        if (dynamicSubObject.Count == 0)
                        {
                            throw new ArgumentException("The DynamicSubObject field is not in a valid JSON format.");
                        }

                        foreach (var subObject in dynamicSubObject)
                        {
                            var productData = (subObject as IDictionary<string, object>)?["Product"] as List<object>;
                            if (productData == null)
                            {
                                throw new ArgumentException("DynamicSubObject must contain a valid Product field.");
                            }

                            foreach (var product in productData)
                            {
                                var productFields = product as IDictionary<string, object>;
                                if (productFields == null)
                                {
                                    throw new ArgumentException("The elements in Product are not in a valid JSON format.");
                                }

                                var requiredProductFields = new List<string> { "Name", "Price", "Quantity" };
                                foreach (var field in requiredProductFields)
                                {
                                    if (!productFields.ContainsKey(field))
                                    {
                                        throw new ArgumentException($"Product field missing: '{field}' is required.");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("DynamicObject with an Order must contain a DynamicSubObject with at least one Product.");
                    }
                }
            }
            else
            {
                throw new ArgumentException("JSON must contain a DynamicObject field.");
            }
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
    }
}