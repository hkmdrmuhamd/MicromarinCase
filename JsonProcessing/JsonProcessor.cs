using System.Text.Json;

namespace MicromarinCase.JsonProcessing
{
    public class JsonProcessor
    {
        public object ProcessJsonElement(Dictionary<string, object> dynamicObject, List<Dictionary<string, object>> dynamicSubObject)
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

        public object ProcessJsonElement(object element)
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
    }
}
