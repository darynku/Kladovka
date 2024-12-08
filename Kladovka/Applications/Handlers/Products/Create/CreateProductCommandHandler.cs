using Kladovka.Contracts.Handlers;
using Kladovka.Domain;
using Kladovka.Infrastructure.Repositories;

namespace Kladovka.Applications.Handlers.Products.Create
{
    public class CreateProductCommandHandler(IRepository<Product> repository) : ICommandHandler<CreateProductCommand, int>
    {
        private readonly IRepository<Product> _repository = repository;

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Sales = request.Sales,
                Discount = request.Discount,
            };
            await _repository.AddAsync(product, cancellationToken);
            return product.Id;
        }
    }
}
