using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arima.Identity.Domain;
using Arima.Identity.Domain.Bll;
using Arima.Identity.Ui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Arima.Identity.Ui.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<Domain.Role> _roleManager;
        private readonly SignInManager<Domain.User> _signInManager;
        private readonly ILogger _logger;
        public RolesController(
            RoleManager<Domain.Role> roleManager,
            SignInManager<Domain.User> signInManager,
            ILoggerFactory loggerFactory)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<ContaController>();
        }
        // GET: Identity
        [Authorize]
        public ActionResult Index()
        {
            return View(_roleManager.Roles.AsEnumerable());
        }

        // GET: Identity/Details/5
        [Authorize]
        public ActionResult Details(Guid id)
        {
            return View(_roleManager.FindByIdAsync(id.ToString()).Result);
        }

        // GET: Identity/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Identity/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                Domain.Role role = new Domain.Role();
                collection.TryGetValue("Id", out Microsoft.Extensions.Primitives.StringValues idValue);
                collection.TryGetValue("Name", out Microsoft.Extensions.Primitives.StringValues nameValue);
                collection.TryGetValue("ConcurrencyStamp", out Microsoft.Extensions.Primitives.StringValues concurrencyStamp);
                role.Id = Guid.NewGuid();
                role.Name = nameValue;
                role.NormalizedName = _roleManager.KeyNormalizer.NormalizeName(nameValue);
                //role.ConcurrencyStamp = concurrencyStamp;


                IdentityResult x = await _roleManager.CreateAsync(role);

                if (x.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                    return Create();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Identity/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            return View(_roleManager.FindByIdAsync(id.ToString()).Result);
        }

        // POST: Identity/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                Domain.Role role = new Domain.Role();
                collection.TryGetValue("Name", out Microsoft.Extensions.Primitives.StringValues nameValue);
                role.Id = id;
                role.Name = nameValue;

                if (_roleManager.UpdateAsync(role).Result.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                    return Edit(id);
            }
            catch
            {
                return View();
            }
        }

        // GET: Identity/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            return View(_roleManager.FindByIdAsync(id.ToString()).Result);
        }

        // POST: Identity/Delete/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                var role = _roleManager.FindByIdAsync(id.ToString()).Result;
                if (_roleManager.DeleteAsync(role).Result.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                    return Delete(id);
            }
            catch
            {
                return View();
            }
        }
    }
}