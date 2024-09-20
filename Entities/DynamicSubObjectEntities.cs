using Newtonsoft.Json;

namespace MicromarinCase.Entities
{
    public class DynamicSubObjectEntities : BaseEntity
    {
        public DynamicSubObjectEntities()
        {
        }
        public DynamicSubObjectEntities(Dictionary<string, object> dynamicFields)
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
