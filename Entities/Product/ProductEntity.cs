using Newtonsoft.Json;

namespace MicromarinCase.Entities.Product
{
    public class ProductEntity : BaseEntity
    {
        public ProductEntity()
        {
        }
        public ProductEntity(Dictionary<string, object> dynamicFields)
        {
            foreach (var field in dynamicFields)
            {
                DynamicFields.Add(field.Key, field.Value);
            }
        }

        public string DynamicFieldsJson
        {
            get => JsonConvert.SerializeObject(DynamicFields);
            set => DynamicFields = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);
        }
    }
}
