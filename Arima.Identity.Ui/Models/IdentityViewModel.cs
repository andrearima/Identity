using Arima.Identity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arima.Identity.Ui.Models
{
    public class IdentityViewModel
    {
        public List<Role> roles { get; set; } 
        public List<User> users { get; set; }
    }
}
