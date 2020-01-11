using Microsoft.AspNetCore.Identity;

namespace Arima.Identity.Domain.Dal
{
    public interface IUserStore<TUser> :
        IUserEmailStore<Domain.User>,
        IUserPhoneNumberStore<Domain.User>,
        IUserTwoFactorStore<Domain.User>,
        IUserPasswordStore<Domain.User>,
        IUserRoleStore<Domain.User>,
        IUserLoginStore<Domain.User>,
        IQueryableUserStore<Domain.User>,
        IUserValidator<Domain.User>
        //IUserClaimsPrincipalFactory<Domain.User>
    {
    }
}
