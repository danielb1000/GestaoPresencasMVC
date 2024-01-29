//using GestaoPresencasMVC.Data;
//using GestaoPresencasMVC.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace GestaoPresencasMVC.Controllers.Accounts
//{
//    // AccountController.cs
//    public class AccountController : Controller
//    {
//        private readonly gpContext _identityContext;
//        private readonly TentativaDb4Context _appContext;

//        public AccountController(gpContext identityContext, TentativaDb4Context appContext)
//        {
//            _identityContext = identityContext;
//            _appContext = appContext;
//        }

//        private async Task<int?> GetLoggedinUserDocenteIdAsync()
//        {
//            var userId = _identityContext.Users
//                .Where(u => u.UserName == User.Identity.Name)
//                .Select(u => u.Id)
//                .FirstOrDefault();

//            // Now, use the userId to retrieve the DocenteId from your application's context
//            var docenteId = await _appContext.Docentes
//                .Where(d => d.Id == userId)
//                .Select(d => d.DocenteId)
//                .FirstOrDefaultAsync();

//            return docenteId;
//        }


//    }

//}
