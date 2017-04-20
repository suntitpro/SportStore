using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        private List<CartDetail> detailCollection = new List<CartDetail>();

        public void AddItem(Product product, int quantity)
        {
            var detail = detailCollection
                .FirstOrDefault(p => p.Product.Id == product.Id);

            if (detail == null)
            {
                 detailCollection.Add(new CartDetail {Product = product, Quantity = quantity});
            }
            else
            {
                detail.Quantity += quantity;
            }
        }

        public void RemoveDetail(Product product)
        {
            detailCollection.RemoveAll(r => r.Product.Id == product.Id);
        }

        public decimal CalculateTotalValue()
        {
            return detailCollection.Sum(e => e.Product.Price * e.Quantity);
        }

        public void Clear()
        {
            detailCollection.Clear();
        }

        public IEnumerable<CartDetail> Details { get { return detailCollection; } }
    }

    public class CartDetail
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}