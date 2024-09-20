using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MicromarinCase.Dtos
{
    public class UpdateDto
    {
        public int Id { get; set; }
        [BindNever]
        public Dictionary<string, object> DynamicObject { get; set; }
        [BindNever]
        public List<Dictionary<string, object>> DynamicSubObject { get; set; }
    }
}
