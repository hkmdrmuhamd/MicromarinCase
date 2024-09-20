using Newtonsoft.Json;

namespace MicromarinCase.Entities
{
    public class DynamicObjectEntities : BaseEntity
    {
        public DynamicObjectEntities()
        {
        }
        public DynamicObjectEntities(Dictionary<string, object> dynamicFields)
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
