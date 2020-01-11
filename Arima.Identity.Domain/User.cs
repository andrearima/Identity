namespace Arima.Identity.Domain
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser<Guid>
    {
        [Display(Name = "Username Normalizado")]
        public override string NormalizedUserName 
        { 
            get => UserName.ToUpper().Trim(); 
            set => base.NormalizedUserName = UserName.ToUpper().Trim(); 
        }
        [Display(Name = "Email Normalizado")]
        public override string NormalizedEmail 
        { 
            get => Email.ToUpper().Trim(); 
            set => base.NormalizedEmail = Email.ToUpper().Trim(); 
        }

        [Display(Name = "Id")]
        public override Guid Id 
        { 
            get => base.Id; 
            set => base.Id = value; 
        }

        [Display(Name = "Username")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public override string Email { get => base.Email; set => base.Email = value; }
        [Display(Name = "Email Confirmado")]
        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }
        [Display(Name = "Hash da Senha")]
        [DataType(DataType.Password)]
        public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
        public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }
        public override string ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }
        [Display(Name = "Telefone")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
        [Display(Name = "Telefone Confirmado")]
        public override bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; set => base.PhoneNumberConfirmed = value; }
        [Display(Name = "Two Factor")]
        public override bool TwoFactorEnabled { get => base.TwoFactorEnabled; set => base.TwoFactorEnabled = value; }
        [Display(Name = "Data de termino do Bloqueio")]
        public override DateTimeOffset? LockoutEnd { get => base.LockoutEnd; set => base.LockoutEnd = value; }
        [Display(Name = "Lockout Enable")]
        public override bool LockoutEnabled { get => base.LockoutEnabled; set => base.LockoutEnabled = value; }
        [Display(Name = "Contador de Acessos Errados")]
        public override int AccessFailedCount { get => base.AccessFailedCount; set => base.AccessFailedCount = value; }

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
}