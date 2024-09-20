namespace MicromarinCase.Dtos
{
    public class CreateOrderDto
    {
        public Dictionary<string, object> OrderData { get; set; }
        public List<Dictionary<string, object>> Products { get; set; }
    }
}
