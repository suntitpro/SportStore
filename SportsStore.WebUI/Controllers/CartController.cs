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
        private IOrderProcessor _orderProcessor;

        public CartController(IProductRepository repository, IOrderProcessor orderProcessor)
        {
            _repository = repository;
            _orderProcessor = orderProcessor;
        }


        public RedirectToRouteResult AddToCart(Cart cart, int id, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new {returnUrl});
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int id, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                cart.RemoveDetail(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(Cart cart, string returnurl)
        {
            return View(new CartIndexViewModel {ReturnUrl = returnurl, Cart = cart});
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ActionResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (!cart.Details.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty");
            }

            if (ModelState.IsValid)
            {
                _orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            return View(shippingDetails);
        }
    }
}