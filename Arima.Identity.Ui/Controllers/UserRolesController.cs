using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arima.Identity.Ui.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Arima.Identity.Ui.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly UserManager<Domain.User> _userManager;
        private readonly RoleManager<Domain.Role> _roleManager;
        private readonly SignInManager<Domain.User> _signInManager;
        private readonly ILogger _logger;
        public UserRolesController(
            UserManager<Domain.User> userManager,
            RoleManager<Domain.Role> roleManager,
            SignInManager<Domain.User> signInManager,
            ILoggerFactory loggerFactory)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<ContaController>();
        }
        // GET: UserRoles
        public ActionResult Index()
        {
            return View(_userManager.Users);
        }

        // GET: UserRoles/Details/5
        public ActionResult Details(Guid id)
        {
            UserRoleViewModel model = new UserRoleViewModel();
            model.user = new Domain.User();
            model.user = _userManager.FindByIdAsync(id.ToString()).Result;
            model.roles = new List<string>();

            model.roles = _userManager.GetRolesAsync(model.user).Result;
                            
            return View(model);
        }
        
        // GET: UserRoles/Edit/5
        public ActionResult Edit(Guid id)
        {
            UserRoleViewModel model = new UserRoleViewModel();
            model.user = new Domain.User();
            model.user = _userManager.FindByIdAsync(id.ToString()).Result;
            model.roles = new List<string>();
            model.rolesParaAdicionar = new List<Domain.Role>();
            
            model.roles = _userManager.GetRolesAsync(model.user).Result;

            model.rolesParaAdicionar = _roleManager.Roles.ToList();
            
            return View(model);
        }

        public ActionResult Adicionar(Guid id, Guid UserId)
        {
            try
            {
                // TODO: Add update logic here
                Domain.User user = _userManager.FindByIdAsync(UserId.ToString()).Result;
                string name = _roleManager.GetRoleNameAsync(_roleManager.FindByIdAsync(id.ToString()).Result).Result;
                _userManager.AddToRoleAsync(user, name);
                return Edit(UserId);
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Remover(Guid id, Guid UserId)
        {
            try
            {
                // TODO: Add update logic here
                Domain.User user = _userManager.FindByIdAsync(UserId.ToString()).Result;
                string name = _roleManager.GetRoleNameAsync(_roleManager.FindByIdAsync(id.ToString()).Result).Result;
                _userManager.RemoveFromRoleAsync(user, name);
                return Edit(UserId);
            }
            catch
            {
                return Edit(UserId);
            }
        }
    }
}