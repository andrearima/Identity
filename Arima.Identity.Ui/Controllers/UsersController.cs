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
    public class UsersController : Controller
    {
        private readonly UserManager<Domain.User> _userManager;
        private readonly SignInManager<Domain.User> _signInManager;
        private readonly ILogger _logger;
        public UsersController(
            UserManager<Domain.User> userManager,
            SignInManager<Domain.User> signInManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<UsersController>();
        }
        
        
        
        
        
        
        [Authorize]
        public ActionResult Index()
        {
            return View(_userManager.Users);
        }

        // GET: Users/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            return View(_userManager.FindByIdAsync(id));
        }

        // GET: Users/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Domain.User user = new Domain.User();
                collection.TryGetValue("UserName", out Microsoft.Extensions.Primitives.StringValues UserName);
                collection.TryGetValue("Email", out Microsoft.Extensions.Primitives.StringValues Email);
                collection.TryGetValue("PasswordHash", out Microsoft.Extensions.Primitives.StringValues PasswordHash);
                user.Id = Guid.NewGuid();
                user.UserName = UserName;
                user.Email = Email;

                _userManager.CreateAsync(user, PasswordHash);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            return View(_userManager.FindByIdAsync(id));
        }

        // POST: Users/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                _userManager.UpdateAsync(_userManager.FindByIdAsync(id).Result);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            return View(_userManager.FindByIdAsync(id));
        }

        // POST: Users/Delete/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                _userManager.DeleteAsync(_userManager.FindByIdAsync(id).Result);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}