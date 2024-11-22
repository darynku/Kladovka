using Kladovka.Contracts.Handlers;
using Kladovka.Domain;
using Kladovka.Infrastructure.Repositories;
namespace Kladovka.Features.Create
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, int>
    {
        private readonly IRepository<Product> _repository;

        public CreateProductCommandHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
            };

            await _repository.AddAsync(product, cancellationToken);

            return product.Id;
        }
    }
}
