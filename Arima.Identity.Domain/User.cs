namespace Arima.Identity.Domain
{
    using Microsoft.AspNetCore.Identity;
    using System;

    public class User : IdentityUser<Guid>
    {
        public override string NormalizedUserName 
        { 
            get => UserName.ToUpper().Trim(); 
            set => base.NormalizedUserName = UserName.ToUpper().Trim(); 
        }
        public override string NormalizedEmail 
        { 
            get => Email.ToUpper().Trim(); 
            set => base.NormalizedEmail = Email.ToUpper().Trim(); 
        }        
    }
}