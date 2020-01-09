using Microsoft.AspNetCore.Identity;
using System;

namespace Arima.Identity.Domain
{
    public class Role : IdentityRole<Guid>
    {
        
    }

    public class RoleClaim : IdentityRoleClaim<Guid>
    {

    }
}
