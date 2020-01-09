using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arima.Identity.Ui.Models
{
    public class UserRoleViewModel
    {
        public Domain.User user { get; set; }
        public IList<string> roles { get; set; }
        public List<Domain.Role> rolesParaAdicionar { get; set; }
    }
}
