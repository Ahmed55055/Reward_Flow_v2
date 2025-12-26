using Microsoft.AspNetCore.Mvc;

namespace Reward_Flow_v2.User.UserManagementService;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
