using System.ComponentModel.DataAnnotations;

namespace Kladovka.Domain
{
    public abstract class EntityBase<TKey> where TKey : struct
    {
        [Key]
        public virtual TKey Id { get; set; }
    }
}
