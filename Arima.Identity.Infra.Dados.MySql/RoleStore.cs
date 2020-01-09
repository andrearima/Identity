using Arima.Identity.Domain;
using Arima.Identity.Domain.Dal;
using Arima.Identity.Infra.Dados.MySql;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Arima.Identity.Infra.Dados.MySql
{
    public class RoleStore : IRoleStorage<Domain.Role>
    {
        private List<Role> ObterRolePorDbReader(IDataReader data)
        {
            List<Role> lista = new List<Role>();
            while (data.Read())
            {
                Role role = new Role();
                role.Id = Guid.Parse(data["Id"].ToString());
                role.Name = data["Name"].ToString();
                role.NormalizedName = data["NormalizedName"].ToString();
                lista.Add(role);
            }
            return lista;
        }

        public List<Role> ObterRoles()
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.CommandText = "SELECT Id, Name, NormalizedName, ConcurrencyStamp FROM aspnetroles";
            return ObterRolePorDbReader(comand.ExecuteReader());
        }

        public Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            role.Id = Guid.NewGuid();
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.CommandText = @$"INSERT INTO aspnetroles 
                        (Id, Name, NormalizedName, ConcurrencyStamp) values
                        (@Id, @Name, @NormalizedName, @ConcurrencyStamp)";

            comand.Parameters.Add(new MySqlParameter("@Id", role.Id));
            comand.Parameters.Add(new MySqlParameter("@Name", role.Name));
            comand.Parameters.Add(new MySqlParameter("@NormalizedName", role.NormalizedName));
            comand.Parameters.Add(new MySqlParameter("@ConcurrencyStamp", role.ConcurrencyStamp));
            try
            {
                banco.ExecuteQueryASync(comand);
                IdentityResult resul = new ArimasIdentityResult(true);
                return Task.FromResult(resul);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = $"Falha ao salvar Role: {role.Name}" }));
            }
        }

        public Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Id", role.Id));
            comand.CommandText = @"DELETE FROM aspnetroles where Id = @Id";
            try
            {
                banco.ExecuteQueryASync(comand);
                IdentityResult resul = new ArimasIdentityResult(true);
                return Task.FromResult(resul);
            }
            catch
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = $"Falha ao Deletar Role: {role.Name}" }));
            }
        }

        public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Id", roleId));
            comand.CommandText = @$"SELECT Id, Name, NormalizedName FROM aspnetroles where Id = @Id";
            return Task.FromResult(ObterRolePorDbReader(comand.ExecuteReader()).FirstOrDefault());
        }

        public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@NormalizedName", normalizedRoleName));
            comand.CommandText = @$"SELECT Id, Name, NormalizedName FROM aspnetroles where NormalizedName = @NormalizedName";

            return Task.FromResult(ObterRolePorDbReader(comand.ExecuteReader()).FirstOrDefault());
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Id", role.Id));
            comand.CommandText = @$"SELECT NormalizedName FROM aspnetroles where Id = @Id";
            string NormalizedRoleName = comand.ExecuteScalar()?.ToString();

            return Task.FromResult(NormalizedRoleName);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Name", role.Name));
            comand.CommandText = @$"SELECT Id FROM aspnetroles where Name = @Name";

            string Id = comand.ExecuteScalar()?.ToString();
            if (string.IsNullOrEmpty(Id))
                return Task.FromResult(new object().ToString());
            return Task.FromResult(Id);
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Id", role.Id));
            comand.CommandText = @$"SELECT Name FROM aspnetroles where Id = @Id";
            string Name = comand.ExecuteScalar()?.ToString();
            if (string.IsNullOrEmpty(Name))
                return Task.FromResult(new object().ToString());

            return Task.FromResult(Name);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@NormalizedName", role.Name.ToUpper().Trim()));
            comand.Parameters.Add(new MySqlParameter("@Id", role.Id));
            comand.CommandText = @$"UPDATE aspnetroles Set NormalizedName = @NormalizedName where Id = @Id";
            return Task.FromResult(comand.ExecuteNonQuery());
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Name", roleName));
            comand.Parameters.Add(new MySqlParameter("@Id", role.Id));
            comand.CommandText = @$"UPDATE aspnetroles Set Name = @Name where Id = @Id";
            return Task.FromResult(comand.ExecuteNonQuery());
        }

        public Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            role.NormalizedName = role.Name.ToUpper().Trim();
            comand.Parameters.Add(new MySqlParameter("@Name", role.Name));
            comand.Parameters.Add(new MySqlParameter("@NormalizedName", role.NormalizedName));
            comand.Parameters.Add(new MySqlParameter("@Id", role.Id));
            comand.CommandText = @$"UPDATE aspnetroles Set Name = @Name, NormalizedName = @NormalizedName where Id = @Id";
            try
            {
                banco.ExecuteQueryASync(comand);
                IdentityResult resul = new ArimasIdentityResult(true);
                return Task.FromResult(resul);
            }
            catch
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = $"Falha ao Atualizar Role: {role.Name}" }));
            }
        }

        public IQueryable<Role> Roles => ObterRoles().AsQueryable<Role>();

        public new Task<IdentityResult> ValidateAsync(RoleManager<Role> manager, Role role)
        {
            IdentityResult identityResult = new IdentityResult();
            IdentityError[] listaErros = new IdentityError[3];

            if (manager.FindByIdAsync(role.Id.ToString()).Result?.Id.ToString().Length > 5)
                listaErros[0] = new IdentityError
                {
                    Description = "Role Id Já existente"
                };

            if (manager.FindByNameAsync(role.Name).Result?.Id.ToString().Length > 5)
                listaErros[1] = new IdentityError
                {
                    Description = "Name já existente"
                };

            if (Regex.IsMatch(role.Name, @"^[ a-zA-Z]"))
                listaErros[2] = new IdentityError
                {
                    Description = "Name deverá contem apenas letras"
                };

            

            if (listaErros[0] == null || listaErros[1] == null || listaErros[2] == null)
            {
                IdentityResult resul = new ArimasIdentityResult(true);
                return Task.FromResult(resul);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(listaErros));
            }
        }


        #region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes

        public object Succeeded { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: descartar estado gerenciado (objetos gerenciados).
                }

                // TODO: liberar recursos não gerenciados (objetos não gerenciados) e substituir um finalizador abaixo.
                // TODO: definir campos grandes como nulos.

                disposedValue = true;
            }
        }

        // TODO: substituir um finalizador somente se Dispose(bool disposing) acima tiver o código para liberar recursos não gerenciados.
        // ~RoleStore()
        // {
        //   // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
        //   Dispose(false);
        // }

        // Código adicionado para implementar corretamente o padrão descartável.
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
            Dispose(true);
            // TODO: remover marca de comentário da linha a seguir se o finalizador for substituído acima.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
