using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository _repository;

        public CartController(IProductRepository repository)
        {
            _repository = repository;
        }


        public RedirectToRouteResult AddToCart(int id, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                GetCart().AddItem(product, 1);
            }

            return RedirectToAction("Index", new {returnUrl});
        }

        public RedirectToRouteResult RemoveFromCart(int id, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                GetCart().RemoveDetail(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            var cart = (Cart) Session["Cart"];
            if (cart != null) return cart;
            cart = new Cart();
            Session["Cart"] = cart;
            return cart;
        }

        public ViewResult Index(string returnurl)
        {
            return View(new CartIndexViewModel {Cart = GetCart(), ReturnUrl = returnurl});
        }
    }
}