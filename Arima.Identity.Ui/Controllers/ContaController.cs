using Arima.Identity.Domain;
using Arima.Identity.Ui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Arima.Identity.Ui.Controllers
{
    public class ContaController : Controller
    {
        private readonly UserManager<Domain.User> _userManager;
        private readonly SignInManager<Domain.User> _signInManager;
        private readonly ILogger _logger;
        public ContaController(
            UserManager<Domain.User> userManager,
            SignInManager<Domain.User> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<ContaController>();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Registrar(ContaRegistrarViewModel modelo)
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
                Domain.Bll.User userBll = new Domain.Bll.User();
                Domain.User user = new Domain.User();
                user.UserName = modelo.UserName;
                user.Email = modelo.Email;

                var passHelper = new PasswordHasher<Domain.User>();
                user.PasswordHash =  passHelper.HashPassword(user, modelo.Senha);
                user.NormalizedUserName = modelo.NomeCompleto;
                CancellationToken token = new CancellationToken();
                var validasenha = await validacao.ValidateAsync(user);
                if (!validasenha.Succeeded)
                {
                    AdicionarErros(validasenha);
                    return View(modelo);
                }

                var result = await userBll.CreateAsync(user, token);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Identity");
                else
                {
                    AdicionarErros(result);
                }
            }

            return View(modelo);
        }


        public async Task<ActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Registrar");
            }
            return View();
        }
        //[HttpPost]
        //public async Task<ActionResult> Login(ContaLoginViewModel modelo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        CancellationToken token = new CancellationToken();
        //        Domain.Bll.User userBll = new Domain.Bll.User();
        //        Domain.User user = await userBll.FindByEmailAsync(modelo.Email, token);

        //        if (user != null)
        //        {
        //            //if(modelo.Senha == user.PasswordHash)
        //        }
        //    }
        //    return View();
        //}
        [HttpPost]
        public async Task<ActionResult> Login(ContaLoginViewModel modelo)
        {
            Domain.Bll.User userBll = new Domain.Bll.User();
            CancellationToken cancelationToken = new CancellationToken();
            var User = await userBll.FindByEmailAsync(modelo.Email, cancelationToken);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, modelo.Email),
                new Claim(ClaimTypes.Role, "Usuario_Comum")
            };

            var identidadeDeUsuario = new ClaimsIdentity(claims, "Login");
            ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(identidadeDeUsuario);

            var propriedadesDeAutenticacao = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.Now.ToLocalTime().AddHours(2),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, propriedadesDeAutenticacao);
            var result = await _signInManager.
                PasswordSignInAsync(User.UserName, modelo.Senha, true, false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Identity");

            return View("Registrar");
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
