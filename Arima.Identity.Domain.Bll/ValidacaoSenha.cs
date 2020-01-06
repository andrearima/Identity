using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arima.Identity.Domain.Bll
{
    public class ValidacaoSenha
    {
        public int TamanhoRequerido { get; set; }
        public bool ObrigatorioCaracteresEspeciais { get; set; }
        public bool ObrigatorioLowerCase { get; set; }
        public bool ObrigatorioUpperCase { get; set; }
        public bool ObrigatorioDigitos { get; set; }

        private bool VerificaTamanhoRequerido(string senha) =>
            senha?.Length >= TamanhoRequerido;

        private bool VerificaCaracteresEspeciais(string senha) =>
            Regex.IsMatch(senha, @"[~`!@#$%^&*()+=|\\{}':;.,<>/?[\]""_-]");

        private bool VerificaLowercase(string senha) =>
            senha.Any(char.IsLower);

        private bool VerificaUppercase(string senha) =>
            senha.Any(char.IsUpper);

        private bool VerificaDigito(string senha) =>
            senha.Any(char.IsDigit);

        
        public Task<Microsoft.AspNetCore.Identity.IdentityResult> ValidateAsync(Domain.User item)
        {
            var erros = new List<string>();
            if (ObrigatorioCaracteresEspeciais && !VerificaCaracteresEspeciais(item.PasswordHash))
                erros.Add("A senha deve conter caracteres especiais!");

            if (!VerificaTamanhoRequerido(item.PasswordHash))
                erros.Add($"A senha deve conter no mínimo {TamanhoRequerido} caracteres.");

            if (ObrigatorioLowerCase && !VerificaLowercase(item.PasswordHash))
                erros.Add($"A senha deve conter no mínimo uma letra minúscula.");

            if (ObrigatorioUpperCase && !VerificaUppercase(item.PasswordHash))
                erros.Add($"A senha deve conter no mínimo uma letra maiúscula.");

            if (ObrigatorioDigitos && !VerificaDigito(item.PasswordHash))
                erros.Add($"A senha deve conter no mínimo um dígito.");

            if (erros.Any())
            {
                List<IdentityError> idErros = new List<IdentityError>();
                foreach (var er in erros)
                {
                    IdentityError msg = new IdentityError();
                    msg.Description = er;
                    idErros.Add(msg);
                }
                return Task.FromResult(IdentityResult.Failed(idErros.ToArray()));
            }
            else
                return Task.FromResult(IdentityResult.Success);
        }
    }
}
