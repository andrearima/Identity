using Microsoft.AspNetCore.Identity;

namespace Arima.Identity.Domain.Dal
{
    public interface IRoleStorage<TRole> : 
        IRoleStore<Domain.Role>, 
        IQueryableRoleStore<Domain.Role>, 
        IRoleValidator<Domain.Role>
    {

    }
}
