using System.Collections.Generic;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
    public class EntityProductRepository : IProductRepository
    {
        private EntityDbContext context = new EntityDbContext();

        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }
    }
}