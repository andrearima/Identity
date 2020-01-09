using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Arima.Identity.Ui.Controllers
{
    public class ClaimsController : Controller
    {
        private UserManager<Domain.User> _userManager;
        private SignInManager<Domain.User> _signManager;
        private ILogger _logger;
        public ClaimsController(
            UserManager<Domain.User> userManager,
            SignInManager<Domain.User> signManager,
            ILoggerFactory logger)
        {
            _userManager = userManager;
            _signManager = signManager;
            _logger = logger.CreateLogger<Domain.User>();
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Claims/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
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

        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Claims/Delete/5
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