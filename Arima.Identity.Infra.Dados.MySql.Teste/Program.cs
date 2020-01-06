using System;
using System.Data;

namespace Arima.Identity.Infra.Dados.MySql.Teste
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Conectar();

        }

        static void Conectar()
        {
            using BancoDados dados = new BancoDados();
            using IDbCommand comand = dados.CreateCommand();
            comand.CommandText = "select * from colaborador";
            var obj = comand.ExecuteNonQuery();
        }
    }
}
