using Esquio.EntityFrameworkCore.Store.Entities;
using Esquio.Model;

namespace Esquio.Model
{
    static class ProductExtensions
    {
        public static ProductEntity To(this Product product)
        {
            return new ProductEntity
            {
                Name = product.Name,
                Description = product.Description
            };
        }
    }
}
