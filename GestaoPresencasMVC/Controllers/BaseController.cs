using GestaoPresencasMVC.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    protected readonly UserManager<gpUser> _userManager;

    public BaseController()
    {
    }
    public BaseController(UserManager<gpUser> userManager)
    {
        _userManager = userManager;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        gpUser user = await _userManager.GetUserAsync(User);

        if (user != null)
        {
            ViewBag.CurrentUserName = user.UserName;
            ViewBag.DocenteId = user.DocenteId;
        }
        else
        {
            ViewBag.CurrentUserName = "Aluno";
            ViewBag.DocenteId = null;
        }

        await base.OnActionExecutionAsync(context, next);
    }
}
