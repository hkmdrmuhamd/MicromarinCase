using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MicromarinCase.Dtos
{
    public class UpdateDto
    {
        public int Id { get; set; }
        public Dictionary<string, object> DynamicObject { get; set; }
        public List<Dictionary<string, object>> DynamicSubObject { get; set; }
    }
}
