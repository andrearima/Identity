using Arima.Identity.Domain;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arima.Identity.Infra.Dados.MySql
{
    public class UserStore : Domain.Dal.IUserStore<User>
    {
        public Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            user.Id = Guid.NewGuid();
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@LoginProvider", login.LoginProvider));
            comand.Parameters.Add(new MySqlParameter("@ProviderKey", login.ProviderKey));
            comand.Parameters.Add(new MySqlParameter("@ProviderDisplayName", login.ProviderDisplayName));
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.CommandText = @$"INSERT INTO aspnetusers (
                                    LoginProvider,
                                    ProviderKey,
                                    ProviderDisplayName,
                                    UserId)
                                    values (
                                    @LoginProvider,
                                    @ProviderKey,
                                    @ProviderDisplayName,
                                    @UserId
                                    )";
            try
            {
                banco.ExecuteQueryASync(comand);
                IdentityResult resul = new ArimasIdentityResult(true);
                return Task.FromResult(resul);
            }
            catch
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = $"Falha ao adicionar Usuario: {user.UserName}" }));
            }
        }
        private List<User> ObterPorReader(IDataReader reader)
        {
            List<User> lista = new List<User>();
            while (reader.Read())
            {
                User user = new User();
                user.Id = Guid.Parse(reader["Id"].ToString());
                user.UserName = reader["UserName"].ToString();
                user.NormalizedUserName = reader["NormalizedUserName"].ToString();
                user.Email = reader["Email"].ToString();
                user.NormalizedEmail = reader["NormalizedEmail"].ToString();
                user.EmailConfirmed = Convert.ToBoolean(reader["EmailConfirmed"]);
                user.PasswordHash = reader["PasswordHash"].ToString();
                user.SecurityStamp = reader["SecurityStamp"].ToString();
                user.ConcurrencyStamp = reader["ConcurrencyStamp"].ToString();
                user.PhoneNumber = reader["PhoneNumber"].ToString();
                user.PhoneNumberConfirmed = Convert.ToBoolean(reader["PhoneNumberConfirmed"]);
                user.TwoFactorEnabled = Convert.ToBoolean(reader["TwoFactorEnabled"]);
                user.LockoutEnabled = Convert.ToBoolean(reader["LockoutEnabled"]);
                user.AccessFailedCount = Convert.ToInt32(reader["AccessFailedCount"]);
                lista.Add(user);
            }

            return lista;
        }

        public List<Domain.User> ObterUsers()
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.CommandText = @$"SELECT 
                                    Id,
                                    UserName,
                                    NormalizedUserName,
                                    Email,
                                    NormalizedEmail,
                                    EmailConfirmed,
                                    PasswordHash,
                                    SecurityStamp,
                                    ConcurrencyStamp,
                                    PhoneNumber,
                                    PhoneNumberConfirmed,
                                    TwoFactorEnabled,
                                    LockoutEnd,
                                    LockoutEnabled,
                                    AccessFailedCount 
                                    FROM aspnetusers";

            return ObterPorReader(comand.ExecuteReader());
        }

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            RoleStore roleStore = new RoleStore();
            Role role = roleStore.FindByNameAsync(roleName, cancellationToken).Result;
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.Parameters.Add(new MySqlParameter("@RoleId", role.Id));
            comand.CommandText = @$"INSERT INTO aspnetuserroles
                        (UserId, RoleId) values
                        (@UserId, @RoleId )";
            try
            {
                banco.ExecuteQueryASync(comand);
                IdentityResult result = new ArimasIdentityResult(true);
                return Task.FromResult(result);
            }
            catch
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = $"Falha ao adicionar Role: {role.Name} ao usuario {user.UserName}" }));
            }
        }
        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            user.Id = Guid.NewGuid();
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Id", user.Id));
            comand.Parameters.Add(new MySqlParameter("@UserName", user.UserName));
            comand.Parameters.Add(new MySqlParameter("@NormalizedUserName", user.NormalizedUserName));
            comand.Parameters.Add(new MySqlParameter("@Email", user.Email));
            comand.Parameters.Add(new MySqlParameter("@NormalizedEmail", user.NormalizedEmail));
            comand.Parameters.Add(new MySqlParameter("@EmailConfirmed", user.EmailConfirmed));
            comand.Parameters.Add(new MySqlParameter("@PasswordHash", user.PasswordHash));
            comand.Parameters.Add(new MySqlParameter("@SecurityStamp", user.SecurityStamp));
            comand.Parameters.Add(new MySqlParameter("@ConcurrencyStamp", user.ConcurrencyStamp));
            comand.Parameters.Add(new MySqlParameter("@PhoneNumber", user.PhoneNumber));
            comand.Parameters.Add(new MySqlParameter("@PhoneNumberConfirmed", user.PhoneNumberConfirmed));
            comand.Parameters.Add(new MySqlParameter("@TwoFactorEnabled", user.TwoFactorEnabled));
            comand.Parameters.Add(new MySqlParameter("@LockoutEnd", user.LockoutEnd));
            comand.Parameters.Add(new MySqlParameter("@LockoutEnabled", user.LockoutEnabled));
            comand.Parameters.Add(new MySqlParameter("@AccessFailedCount", user.AccessFailedCount));
            comand.CommandText = @$"INSERT INTO aspnetusers (
                                    Id,
                                    UserName,
                                    NormalizedUserName,
                                    Email,
                                    NormalizedEmail,
                                    EmailConfirmed,
                                    PasswordHash,
                                    SecurityStamp,
                                    ConcurrencyStamp,
                                    PhoneNumber,
                                    PhoneNumberConfirmed,
                                    TwoFactorEnabled,
                                    LockoutEnd,
                                    LockoutEnabled,
                                    AccessFailedCount )
                                    values (
                                    @Id,
                                    @UserName,
                                    @NormalizedUserName,
                                    @Email,
                                    @NormalizedEmail,
                                    @EmailConfirmed,
                                    @PasswordHash,
                                    @SecurityStamp,
                                    @ConcurrencyStamp,
                                    @PhoneNumber,
                                    @PhoneNumberConfirmed,
                                    @TwoFactorEnabled,
                                    @LockoutEnd,
                                    @LockoutEnabled,
                                    @AccessFailedCount )";
            try
            {
                banco.ExecuteQueryASync(comand);
                IdentityResult resul = new ArimasIdentityResult(true);
                return Task.FromResult(resul);
            }
            catch (Exception ex)
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = $"Falha ao adicionar Usuario: {user.UserName}" }));
            }
        }
        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Id", user.Id));
            comand.CommandText = @$"DELETE FROM aspnetusers where Id = @Id";
            try
            {
                banco.ExecuteQueryASync(comand);
                IdentityResult resul = new ArimasIdentityResult(true);
                return Task.FromResult(resul);
            }
            catch
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = $"Falha ao excluir User: {user.UserName}" }));
            }
        }
        public Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@NormalizedEmail", normalizedEmail));
            comand.CommandText = @"SELECT 
                                    Id,
                                    UserName,
                                    NormalizedUserName,
                                    Email,
                                    NormalizedEmail,
                                    EmailConfirmed,
                                    PasswordHash,
                                    SecurityStamp,
                                    ConcurrencyStamp,
                                    PhoneNumber,
                                    PhoneNumberConfirmed,
                                    TwoFactorEnabled,
                                    LockoutEnd,
                                    LockoutEnabled,
                                    AccessFailedCount 
                                    FROM aspnetusers where NormalizedEmail = @NormalizedEmail";
            return Task.FromResult(ObterPorReader(comand.ExecuteReader()).FirstOrDefault());
        }
        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Id", userId));
            comand.CommandText = @"SELECT 
                                    Id,
                                    UserName,
                                    NormalizedUserName,
                                    Email,
                                    NormalizedEmail,
                                    EmailConfirmed,
                                    PasswordHash,
                                    SecurityStamp,
                                    ConcurrencyStamp,
                                    PhoneNumber,
                                    PhoneNumberConfirmed,
                                    TwoFactorEnabled,
                                    LockoutEnd,
                                    LockoutEnabled,
                                    AccessFailedCount 
                                    FROM aspnetusers where Id = @Id";
            return Task.FromResult(ObterPorReader(comand.ExecuteReader()).FirstOrDefault());
        }
        public Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@LoginProvider", loginProvider));
            comand.Parameters.Add(new MySqlParameter("@ProviderKey", providerKey));
            comand.CommandText = @"SELECT UserId FROM aspnetuserlogins WHERE 
                                        LoginProvider = @LoginProvider and ProviderKey = @ProviderKey";
            var id = comand.ExecuteScalar().ToString();

            return FindByIdAsync(id,cancellationToken);
        }
        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@NormalizedUserName", normalizedUserName));
            comand.CommandText = @"SELECT 
                                    Id,
                                    UserName,
                                    NormalizedUserName,
                                    Email,
                                    NormalizedEmail,
                                    EmailConfirmed,
                                    PasswordHash,
                                    SecurityStamp,
                                    ConcurrencyStamp,
                                    PhoneNumber,
                                    PhoneNumberConfirmed,
                                    TwoFactorEnabled,
                                    LockoutEnd,
                                    LockoutEnabled,
                                    AccessFailedCount 
                                    FROM aspnetusers where NormalizedUserName = @NormalizedUserName";
            return ObterPorReader(comand.ExecuteReader()).FirstOrDefault();
        }
        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@NormalizedUserName", user.Id));
            comand.CommandText = @"SELECT 
                                    NormalizedEmail
                                    FROM aspnetusers where NormalizedUserName = @NormalizedUserName";
            return Task.FromResult(banco.ExecuteScalar(comand)?.ToString());
        }
        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@NormalizedUserName", user.Id));
            comand.CommandText = @"SELECT 
                                    EmailConfirmed
                                    FROM aspnetusers where NormalizedUserName = @NormalizedUserName";
            return Task.FromResult(Convert.ToBoolean(banco.ExecuteScalar(comand)));
        }
        private IList<UserLoginInfo> ObterUserLoginInfo(IDataReader reader)
        {
            IList<UserLoginInfo> lista = new List<UserLoginInfo>();
            while (reader.Read())
            {
                UserLoginInfo userloginInfo = new UserLoginInfo(
                        reader["LoginProvider"].ToString(),
                        reader["ProviderDisplayName"].ToString(),
                        reader["ProviderKey"].ToString()
                    );
                lista.Add(userloginInfo);
            }
            return lista;
        }
        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.CommandText = @$"SELECT 
                                    LoginProvider
                                    ProviderDisplayName,
                                    ProviderKey
                                    FROM aspnetuserlogins where UserId = @UserId";
            return Task.FromResult(ObterUserLoginInfo(comand.ExecuteReader()));
        }
        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.CommandText = @$"SELECT 
                                    NormalizedEmail
                                    FROM aspnetusers where Id = @UserId";
            return Task.FromResult(banco.ExecuteScalar(comand).ToString());
        }
        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.CommandText = @$"SELECT 
                                    NormalizedUserName
                                    FROM aspnetusers where Id = @UserId";
            return Task.FromResult(banco.ExecuteScalar(comand).ToString());
        }
        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.CommandText = @$"SELECT 
                                    PasswordHash
                                    FROM aspnetusers where Id = @UserId";
            return Task.FromResult(banco.ExecuteScalar(comand).ToString());
        }
        public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.CommandText = @$"SELECT 
                                    PhoneNumber
                                    FROM aspnetusers where Id = @UserId";
            return Task.FromResult(banco.ExecuteScalar(comand).ToString());
        }
        public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.CommandText = @"SELECT 
                                    PhoneNumberConfirmed
                                    FROM aspnetusers where Id = @UserId";
            return Task.FromResult(Convert.ToBoolean(banco.ExecuteScalar(comand)));
        }
        public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.CommandText = @"SELECT 
                                    RoleId
                                    FROM aspnetuserroles where UserId = @UserId";

            IList<string> lista = new List<string>();
            using (var reader = comand.ExecuteReader())
                while (reader.Read())
                {
                    lista.Add(reader["RoleId"].ToString());
                }
            return Task.FromResult(lista);
        }
        public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.CommandText = @"SELECT 
                                    TwoFactorEnabled
                                    FROM aspnetusers where Id = @UserId";
            return Task.FromResult(Convert.ToBoolean(banco.ExecuteScalar(comand)));
        }
        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserName", user.UserName));
            comand.CommandText = @"SELECT 
                                    Id
                                    FROM aspnetusers where UserName = @UserName";
            return Task.FromResult(banco.ExecuteScalar(comand).ToString());
        }
        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.CommandText = @"SELECT 
                                    UserName
                                    FROM aspnetusers where Id = @UserId";
            return Task.FromResult(banco.ExecuteScalar(comand).ToString());
        }
        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Name", roleName));
            comand.CommandText = @"SELECT 
                                    Id
                                    FROM aspnetroles where Name = @Name";

            var id = comand.ExecuteScalar().ToString();
            comand.Parameters.Clear();
            comand.Parameters.Add(new MySqlParameter("@RoleId", id));
            comand.CommandText = @$"SELECT 
                                    UserId
                                    FROM aspnetuserroles where RoleId = @RoleId";
            IList<User> listaUsers = new List<User>();
            using var reader = comand.ExecuteReader();
            while (reader.Read())
            {
                var User = FindByIdAsync(reader["UserId"].ToString(), cancellationToken).Result;
                listaUsers.Add(User);
            }

            return Task.FromResult(listaUsers);
        }
        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Id", user.Id));
            comand.CommandText = @$"SELECT 
                                    PasswordHash
                                    FROM aspnetusers where Id = @Id";

            bool existe = comand.ExecuteNonQuery().ToString().Length > 0;

            return Task.FromResult(existe);
        }
        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            var listaUsers = GetUsersInRoleAsync(roleName, cancellationToken).Result;
            return Task.FromResult(listaUsers.Contains(user));
        }
        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            RoleStore roleStore = new RoleStore();
            var role = roleStore.FindByNameAsync(roleName, cancellationToken).Result;
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.Parameters.Add(new MySqlParameter("@RoleId", role.Id));
            comand.CommandText = @$"DELETE FROM aspnetuserroles WHERE
                                    UserId = @UserId and RoleId = @RoleId";

            return Task.FromResult(comand.ExecuteNonQuery());
        }
        public Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@LoginProvider", loginProvider));
            comand.Parameters.Add(new MySqlParameter("@ProviderKey", providerKey));
            comand.Parameters.Add(new MySqlParameter("@UserId", user.Id));
            comand.CommandText = @"DELETE FROM aspnetuserlogins WHERE
                                    LoginProvider = @LoginProvider 
                                    and ProviderKey = @providerKey
                                    and UserId = @UserId";

            return Task.FromResult(comand.ExecuteNonQuery());
        }
        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Email", email));
            comand.Parameters.Add(new MySqlParameter("@Id", user.Id));
            comand.CommandText = @$"UPDATE aspnetusers SET
                                    Email = @Email WHERE Id = @Id";

            return Task.FromResult(comand.ExecuteNonQuery());
        }
        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Confirmed", confirmed));
            comand.Parameters.Add(new MySqlParameter("@Id", user.Id));
            comand.CommandText = @"UPDATE aspnetusers SET
                                    EmailConfirmed = @Confirmed WHERE Id = @Id";

            return Task.FromResult(comand.ExecuteNonQuery());
        }
        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@NormalizedEmail", normalizedEmail));
            comand.Parameters.Add(new MySqlParameter("@Id", user.Id));
            comand.CommandText = @"UPDATE aspnetusers SET
                                    NormalizedEmail = @NormalizedEmail WHERE Id = @Id";

            return Task.FromResult(comand.ExecuteNonQuery());
        }
        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@NormalizedName", normalizedName));
            comand.Parameters.Add(new MySqlParameter("@Id", user.Id));
            comand.CommandText = @"UPDATE aspnetusers SET
                                    NormalizedUserName = @NormalizedName WHERE Id = @Id";

            return Task.FromResult(comand.ExecuteNonQuery());
        }
        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@PasswordHash", passwordHash));
            comand.Parameters.Add(new MySqlParameter("@Id", user.Id));
            comand.CommandText = @$"UPDATE aspnetusers SET
                                    PasswordHash = @PasswordHash WHERE Id = @Id";

            return Task.FromResult(comand.ExecuteNonQuery());
        }
        public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@PhoneNumber", phoneNumber));
            comand.Parameters.Add(new MySqlParameter("@Id", user.Id));
            comand.CommandText = @$"UPDATE aspnetusers SET
                                    PhoneNumber = @PhoneNumber WHERE Id = @Id";

            return Task.FromResult(comand.ExecuteNonQuery());
        }
        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.CommandText = @$"UPDATE aspnetusers SET
                                    PhoneNumberConfirmed = {confirmed} WHERE Id = {user.Id}";

            return Task.FromResult(comand.ExecuteNonQuery());
        }
        public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@TwoFactorEnable", enabled));
            comand.Parameters.Add(new MySqlParameter("@Id", user.Id));
            comand.CommandText = @"UPDATE aspnetusers SET
                                    TwoFactorEnable = TwoFactorEnable WHERE Id = @Id";

            return Task.FromResult(comand.ExecuteNonQuery());
        }
        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@UserName", userName));
            comand.Parameters.Add(new MySqlParameter("@Id", user.Id));
            comand.CommandText = @"UPDATE aspnetusers SET
                                    UserName = @UserName WHERE Id = @Id";

            return Task.FromResult(comand.ExecuteNonQuery());
        }
        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            using var banco = new BancoDados();
            using var comand = banco.CreateCommand();
            comand.Parameters.Add(new MySqlParameter("@Id", user.Id));
            comand.Parameters.Add(new MySqlParameter("@UserName", user.UserName));
            comand.Parameters.Add(new MySqlParameter("@NormalizedUserName", user.NormalizedUserName));
            comand.Parameters.Add(new MySqlParameter("@Email", user.Email));
            comand.Parameters.Add(new MySqlParameter("@NormalizedEmail", user.NormalizedEmail));
            comand.Parameters.Add(new MySqlParameter("@EmailConfirmed", user.EmailConfirmed));
            comand.Parameters.Add(new MySqlParameter("@PasswordHash", user.PasswordHash));
            comand.Parameters.Add(new MySqlParameter("@SecurityStamp", user.SecurityStamp));
            comand.Parameters.Add(new MySqlParameter("@ConcurrencyStamp", user.ConcurrencyStamp));
            comand.Parameters.Add(new MySqlParameter("@PhoneNumber", user.PhoneNumber));
            comand.Parameters.Add(new MySqlParameter("@PhoneNumberConfirmed", user.PhoneNumberConfirmed));
            comand.Parameters.Add(new MySqlParameter("@TwoFactorEnabled", user.TwoFactorEnabled));
            comand.Parameters.Add(new MySqlParameter("@LockoutEnd", user.LockoutEnd));
            comand.Parameters.Add(new MySqlParameter("@LockoutEnabled", user.LockoutEnabled));
            comand.Parameters.Add(new MySqlParameter("@AccessFailedCount", user.AccessFailedCount));
            comand.CommandText = @$"UPDATE aspnetusers set
                                    UserName = @UserName,
                                    NormalizedUserName = @NormalizedUserName,
                                    Email = Email,
                                    NormalizedEmail = @NormalizedEmail,
                                    EmailConfirmed = @EmailConfirmed,
                                    PasswordHash = @PasswordHash,
                                    SecurityStamp = @SecurityStamp,
                                    ConcurrencyStamp = @ConcurrencyStamp,
                                    PhoneNumber = @PhoneNumber,
                                    PhoneNumberConfirmed = @PhoneNumberConfirmed,
                                    TwoFactorEnabled = @TwoFactorEnabled,
                                    LockoutEnd = @LockoutEnd,
                                    LockoutEnabled = @LockoutEnabled,
                                    AccessFailedCount = @AccessFailedCount
                                    WHERE Id = @Id";
            try
            {
                await banco.ExecuteQueryASync(comand);
                IdentityResult resul = new ArimasIdentityResult(true);
                return resul;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Falha ao adicionar Usuario: {user.UserName} - {ex.Message}" });
            }
        }
        public IQueryable<User> Users => ObterUsers().AsQueryable<Domain.User>();

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes


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
        // ~UserStore()
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
