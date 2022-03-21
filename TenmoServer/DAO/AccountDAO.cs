using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountDAO : IAccountDAO
    {
        private readonly string connectionString;
        private string sqlGetAccounts = "SELECT * FROM account;";
        private string sqlGetBalance = $"SELECT balance FROM account WHERE user_id = @user_id";
        private string sqlDecreaseBalance = "UPDATE account SET balance = balance - @amount WHERE account_id = @account_from;";
        private string sqlIncreaseBalance = "UPDATE account SET balance = balance + @amount WHERE account_id = @account_to;";
        UserSqlDao userSqlDao;
        public AccountDAO(string dbconnectionString)
        {
            connectionString = dbconnectionString;
        }
        public decimal GetBalance(int user_id)
        {
            decimal balance = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sqlGetBalance, conn);
                cmd.Parameters.AddWithValue("@user_id", user_id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    balance = Convert.ToDecimal(reader["balance"]);
                }
               
            }

            return balance;
        }

        public List<Account> GetAccounts()
        {
            List<Account> accounts = new List<Account>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sqlGetAccounts, conn);
               
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Account account = new Account();
                    account.AccountId = Convert.ToInt32(reader["account_id"]);
                    account.UserId = Convert.ToInt32(reader["user_id"]);
                    account.Balance = Convert.ToDecimal(reader["balance"]);
                    accounts.Add(account);
                }
                
            }

            return accounts;
        }

        //public bool Transfer()
        //{
        //    List<User> users = userSqlDao.GetUsers();
        //}
    }
}
