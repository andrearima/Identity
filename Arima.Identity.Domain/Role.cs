using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Arima.Identity.Domain
{
    public class Role : IdentityRole<Guid>
    {
        [Display(Name="Id")]
        public override Guid Id
        {
            get => base.Id;
            set => base.Id = value;
        }
        [Display(Name = "Nome")]
        public override string Name 
        { 
            get => base.Name; 
            set => base.Name = value; 
        }
        [Display(Name = "Nome Normalizado")]
        public override string NormalizedName 
        { 
            get => base.NormalizedName; 
            set => base.NormalizedName = value; 
        }
        public override string ConcurrencyStamp 
        { 
            get => base.ConcurrencyStamp; 
            set => base.ConcurrencyStamp = value; 
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class RoleClaim : IdentityRoleClaim<Guid>
    {

    }
}
