using Kladovka.Contracts.Abstract;

namespace Kladovka.Applications.Handlers.Products.Create
{
    public class CreateProductCommand : Command<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Sales { get; set; }
        public decimal Discount { get; set; }
    }
}
