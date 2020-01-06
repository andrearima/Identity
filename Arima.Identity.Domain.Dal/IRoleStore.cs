using Microsoft.AspNetCore.Identity;
using System;

namespace Arima.Identity.Domain.Dal
{
    public interface IRoleStore : IRoleStore<Domain.Role>
    {
    }
}
