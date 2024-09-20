namespace MicromarinCase.Dtos
{
    public class CreateDto
    {
        public Dictionary<string, object> DynamicObject { get; set; }
        public List<Dictionary<string, object>> DynamicSubObject { get; set; }
    }
}
