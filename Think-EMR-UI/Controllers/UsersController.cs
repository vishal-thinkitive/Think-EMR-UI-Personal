using Microsoft.AspNetCore.Mvc;

namespace Think_EMR_UI.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
