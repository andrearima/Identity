using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Arima.Identity.Infra.Dados.MySql
{
    public class BancoDados : IDbCommand, IDbConnection
    {
        private static MySqlConnection _connection;
        private MySqlCommand _command;
        private IConfiguration _configuracao;
        public BancoDados()
        {
            _connection = new MySqlConnection(this.ConnectionString);
            _connection.ConnectionString = "Server=localhost;Database=identity;Uid=root;Pwd=urxi#2638;";
            ConnectionString = "Server=localhost;Database=identity;Uid=root;Pwd=urxi#2638;";
            Open();
        }
        public string ConnectionString
        {
            get => _configuracao.GetConnectionString("MySql");
            set => _connection.ConnectionString = value;
        }

        public Task<bool> ExecuteQueryASync(IDbCommand comando)
        {
            _command = (MySqlCommand)comando;
            return Task.FromResult(_command.ExecuteNonQueryAsync().Result > 0);
        }

        public int ConnectionTimeout
        {
            get
            {
                return _connection.ConnectionTimeout;
            }
        }
        public string Database
        {
            get
            {
                return _connection.Database;
            }
        }
        public ConnectionState State
        {
            get
            {
                return _connection.State;
            }
        }
        public string CommandText
        {
            get
            {
                if (_command == null)
                    _command = new MySqlCommand();

                return _command.CommandText;
            }
            set
            {
                if (_command == null)
                    _command = new MySqlCommand();

                _command.CommandText = value;
            }
        }
        public int CommandTimeout
        {
            get
            {
                if (_command != null)
                    return _command.CommandTimeout;

                return 0;
            }
            set
            {
                if (_command == null)
                    _command = new MySqlCommand();

                _command.CommandTimeout = value;
            }
        }
        public CommandType CommandType
        {
            get
            {
                if (_command != null)
                    return _command.CommandType;

                return CommandType.Text;
            }
            set
            {
                if (_command == null)
                    _command = new MySqlCommand();

                _command.CommandType = value;
            }
        }
        public IDbConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = (MySqlConnection)value;
            }
        }
        public IDataParameterCollection Parameters
        {
            get
            {
                return null;
            }
        }
        public IDbTransaction Transaction
        {
            get
            {
                return _command.Transaction;
            }
            set => throw new System.NotImplementedException();
        }
        public UpdateRowSource UpdatedRowSource { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        string IDbConnection.ConnectionString
        {
            get
            {
                return ConnectionString;
            }
            set
            {
                ConnectionString = value;
            }
        }
        public IDbTransaction BeginTransaction()
        {
            return _connection.BeginTransaction();
        }
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _connection.BeginTransaction(il);
        }
        public void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }
        public void Close()
        {
            _connection.Close();
        }
        public IDbCommand CreateCommand()
        {
            _command = new MySqlCommand();
            if (_command.Parameters != null)
                _command.Parameters.Clear();
            _command.Connection = _connection;
            return _command;
        }
        public void Open()
        {
            _connection.Open();
        }
        public void Cancel()
        {
            _command.Cancel();
        }
        public IDbDataParameter CreateParameter()
        {
            return new MySqlParameter();
        }
        public int ExecuteNonQuery()
        {
            if (_command.Connection.State != ConnectionState.Open)
                _connection.Open();
            return _command.ExecuteNonQuery();
        }
        public IDataReader ExecuteReader()
        {
            return _command.ExecuteReader();
        }
        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return _command.ExecuteReader(behavior);
        }
        public object ExecuteScalar(IDbCommand command)
        {
            return command.ExecuteScalar();
        }
        public object ExecuteScalar()
        {
            return _command.ExecuteScalar();
        }
        public void Prepare()
        {
            _command.Prepare();
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                    }

                    if (_command != null)
                    {
                        if (_command.Transaction != null)
                            _command.Transaction.Commit();

                        _command.Dispose();
                    }

                }
                _command = null;
                _connection = null;
                _configuracao = null;
                // TODO: liberar recursos não gerenciados (objetos não gerenciados) e substituir um finalizador abaixo.
                // TODO: definir campos grandes como nulos.

                disposedValue = true;
            }
        }

        // TODO: substituir um finalizador somente se Dispose(bool disposing) acima tiver o código para liberar recursos não gerenciados.
        // ~BancoDados()
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
