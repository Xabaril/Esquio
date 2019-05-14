using Esquio.Model;
using System.Collections.Generic;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<FeatureEntity> Features { get; set; } = new List<FeatureEntity>();

        public Product To()
        {
            return new Product(Name, Description);
        }

        public void CopyFrom(Product product)
        {
            Name = product.Name;
            Description = product.Description;
        }
    }
}
