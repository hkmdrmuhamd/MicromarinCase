using MicromarinCase.Context;
using MicromarinCase.Dtos;
using MicromarinCase.Entities;
using MicromarinCase.Entities.Order;
using MicromarinCase.Entities.Product;
using Newtonsoft.Json;
using System.Text.Json;

namespace MicromarinCase.Services
{
    public class DynamicObjectService : IDynamicObjectService
    {
        private readonly DatabaseContext _context;
        public DynamicObjectService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(CreateOrderDto createDynamicObjectDto)
        {
            var order = new OrderEntity(createDynamicObjectDto.OrderData);
            var products = new List<ProductEntity>();

            foreach (var productData in createDynamicObjectDto.Products)
            {
                var product = new ProductEntity(productData);
                products.Add(product);
            }
            var jsonData = new Dictionary<string, object>
            {
                { "OrderData", order.DynamicFields.ToDictionary(
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
                { "Products", products.Select(p => new
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
    }
}
