using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MicromarinCase.Dtos
{
    public class CreateDto
    {
        [BindNever]
        public Dictionary<string, object> DynamicObject { get; set; }
        [BindNever]
        public List<Dictionary<string, object>> DynamicSubObject { get; set; }
    }
}
