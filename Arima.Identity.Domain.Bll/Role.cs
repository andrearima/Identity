using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Arima.Identity.Domain.Bll
{
    public class Role
    {
        public bool InserirRole(Domain.Role role)
        {
            CancellationToken token = new CancellationToken();
            Infra.Dados.MySql.RoleStore roleStore = new Infra.Dados.MySql.RoleStore();
            try
            {
                roleStore.CreateAsync(role, token);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public List<Domain.Role> ObterRoles()
        {
            CancellationToken token = new CancellationToken();
            Infra.Dados.MySql.RoleStore roleStore = new Infra.Dados.MySql.RoleStore();
            return roleStore.ObterRoles();
        }
        public List<Domain.Role> ObterRolesModel()
        {
            Infra.Dados.MySql.RoleStore roleStore = new Infra.Dados.MySql.RoleStore();
            return roleStore.ObterRoles();
        }
    }
}
