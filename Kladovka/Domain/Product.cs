using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kladovka.Domain
{
    public class Product : EntityBase<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Sales { get; set; }
        public decimal Discount { get; set; }

        public void SetDiscount(decimal discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
                throw new ArgumentException("Скидка должна быть в диапазоне от 0 до 100.");

            Discount = discountPercentage;
            Sales = CalculateSalesPrice();
        }

        private decimal CalculateSalesPrice()
        {
            return Math.Round(Price * (1 - Discount / 100), 2);
        }


    }
}
