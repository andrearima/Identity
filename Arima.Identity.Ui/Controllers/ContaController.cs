using Arima.Identity.Domain;
using Arima.Identity.Ui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Arima.Identity.Ui.Controllers
{
    public class ContaController : Controller
    {
        private readonly UserManager<Domain.User> _userManager;
        private readonly RoleManager<Domain.Role> _roleManager;
        private readonly SignInManager<Domain.User> _signInManager;
        private readonly ILogger _logger;
        public ContaController(
            UserManager<Domain.User> userManager,
            RoleManager<Domain.Role> roleManager,
            SignInManager<Domain.User> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<ContaController>();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registrar(ContaRegistrarViewModel modelo)
        {
            if (ModelState.IsValid)
            {

                var validacao = new Domain.Bll.ValidacaoSenha()
                {
                    ObrigatorioCaracteresEspeciais = true,
                    ObrigatorioDigitos = true,
                    ObrigatorioLowerCase = true,
                    ObrigatorioUpperCase = true,
                    TamanhoRequerido = 10
                };

                Domain.User user = new Domain.User();
                user.UserName = modelo.UserName;
                user.Email = modelo.Email;

                var passHelper = new PasswordHasher<Domain.User>();
                user.PasswordHash = passHelper.HashPassword(user, modelo.Senha);
                user.NormalizedUserName = modelo.NomeCompleto;

                var result = _userManager.CreateAsync(user, modelo.Senha).Result;

                // var result = await userBll.CreateAsync(user, token);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Identity");
                else
                {
                    AdicionarErros(result);
                }
            }

            return View(modelo);
        }


        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Registrar");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(ContaLoginViewModel modelo)
        {
            var User = await _userManager.FindByEmailAsync(modelo.Email);

            var result = await _signInManager.
                PasswordSignInAsync(User.UserName, modelo.Senha, true, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View("Registrar");
        }
       
        public async Task<ActionResult> Deslogar()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult AcessoNegado()
        {
            return View();
        }

        private void AdicionarErros(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }
    }
}
