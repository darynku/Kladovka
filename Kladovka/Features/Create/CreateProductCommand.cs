using Kladovka.Contracts.Abstract;

namespace Kladovka.Features.Create
{
    public class CreateProductCommand : Command<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }  
    }
}
