using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository _repository;

        public NavController(IProductRepository repository)
        {
            _repository = repository;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            var categories = _repository.Products
                .Select(c => c.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}