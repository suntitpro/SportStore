using System.Web.Mvc;
using SportsStore.Domain.Entities;


namespace SportsStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string SessionKey = "Cart";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //get the Cart from session

            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)
            {
                cart = (Cart) controllerContext.HttpContext.Session[SessionKey];
            }

            //create the Cart if there wasn't one in session data
            if (cart != null) return cart;
            cart = new Cart();
            if (controllerContext.HttpContext.Session != null)
            {
                controllerContext.HttpContext.Session[SessionKey] = cart;
            }

            return cart;
        }
    }
}