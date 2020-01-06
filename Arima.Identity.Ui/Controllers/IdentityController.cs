using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arima.Identity.Domain;
using Arima.Identity.Domain.Bll;
using Arima.Identity.Ui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Arima.Identity.Ui.Controllers
{
    public class IdentityController : Controller
    {
        // GET: Identity
        [Authorize]
        public ActionResult Index()
        {
            IdentityViewModel model = new IdentityViewModel();
            Domain.Bll.Role roleBll = new Domain.Bll.Role();
            model.roles = new List<Domain.Role>();
            model.roles = roleBll.ObterRoles();
            Domain.Bll.User userll = new Domain.Bll.User();
            model.users = new List<Domain.User>();
            model.users = userll.ObterUsers();
            return View(model);
        }

        // GET: Identity/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
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
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Domain.Role role = new Domain.Role();
                collection.TryGetValue("Id", out Microsoft.Extensions.Primitives.StringValues idValue);
                collection.TryGetValue("Name", out Microsoft.Extensions.Primitives.StringValues nameValue);
                collection.TryGetValue("NormalizedName", out Microsoft.Extensions.Primitives.StringValues normalizedNameValue);
                role.Id = Guid.NewGuid();
                role.Name = nameValue;
                role.NormalizedName = normalizedNameValue;

                Domain.Bll.Role roleBll = new Domain.Bll.Role();
                roleBll.InserirRole(role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Identity/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Identity/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Identity/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Identity/Delete/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}