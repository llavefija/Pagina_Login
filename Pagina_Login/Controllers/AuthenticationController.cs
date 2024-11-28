using Microsoft.AspNetCore.Mvc;
using Pagina_Login.DAL;
using Pagina_Login.Models;
using Pagina_Login.Models.ViewModels;

namespace Pagina_Login.Controllers
{
    public class AuthenticationController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                UsuarioDAL dal = new UsuarioDAL();
                UsuarioModel usuario = dal.GetUsuarioLogin(model.Username, model.Password);

                //Validar usuario
                if (usuario != null)
                {

                    HttpContext.Session.SetString("Username", model.Username);

                    // Autenticacion exitosa
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            }
            return View(model);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                UsuarioDAL dal = new UsuarioDAL();
                UsuarioModel usuario = new UsuarioModel();

                usuario.UserName = model.Username;
                usuario.Passwd = model.Password;

                dal.CreateUsuario(usuario);

                UsuarioModel validarCreacion = dal.GetUsuarioLogin(model.Username, model.Password);

                // Validar Usuario
                if (validarCreacion != null)
                {
                    HttpContext.Session.SetString("Username", usuario.UserName);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "No se ha podido crear el usuario.");
            }
            return View(model);
        }
    }
}
