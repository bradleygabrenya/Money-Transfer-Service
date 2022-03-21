using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoClient.Models;

namespace TenmoServer.DAO
{
    public class TransferDAO : ITransferDAO
    {
        private string SQLAddTransfer = "INSERT INTO transfer (transfer_type_id, transfer_status_id, account_from, account_to, amount)"
            + " VALUES (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount);";
        private string sqlDecreaseBalance = "UPDATE account SET balance = balance - @amount WHERE account_id = @account_from;";
        private string sqlIncreaseBalance = "UPDATE account SET balance = balance + @amount WHERE account_id = @account_to;";
        private string sqlGetTransfers = "SELECT t.* FROM account a " +
                                        "JOIN transfer t ON a.account_id = t.account_to OR a.account_id = t.account_from " +
                                        "WHERE a.account_id = @account_id;";
        private string sqlGetTransfer = "SELECT * FROM transfer WHERE transfer_id = @transfer_id;";
        private string connectionString;

        public TransferDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public bool AddTransfer(Transfer transfer)
        {
            bool result = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(SQLAddTransfer, conn);
                cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                int count = cmd.ExecuteNonQuery();

                if (count > 0)
                {
                    SqlCommand cmd2 = new SqlCommand(sqlIncreaseBalance, conn);
                    cmd2.Parameters.AddWithValue("@amount", transfer.Amount);
                    cmd2.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd2.ExecuteNonQuery();

                    SqlCommand cmd3 = new SqlCommand(sqlDecreaseBalance, conn);
                    cmd3.Parameters.AddWithValue("@amount", transfer.Amount);
                    cmd3.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd3.ExecuteNonQuery();

                    result = true;
                }

            }
            return result;
        }

        public bool RequestTransfer(Transfer transfer)
        {
            bool result = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(SQLAddTransfer, conn);
                cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    result = true;
                }
            }
            return result;
        }

        public List<Transfer> GetTransfers(int account_id)
        {
            List<Transfer> returnTransfers = new List<Transfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sqlGetTransfers, conn);
                    cmd.Parameters.AddWithValue("@account_id", account_id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer t = GetTransferFromReader(reader);
                        returnTransfers.Add(t);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return returnTransfers;

        }

        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer t = new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]),
                TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]),
                AccountFrom = Convert.ToInt32(reader["account_from"]),
                AccountTo = Convert.ToInt32(reader["account_to"]),
                Amount = Convert.ToDecimal(reader["amount"])
            };

            return t;
        }

        public Transfer GetTransferDetail(int transfer_id)
        {
            Transfer transfer = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sqlGetTransfer, conn);
                cmd.Parameters.AddWithValue("@transfer_id", transfer_id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    transfer = GetTransferFromReader(reader);
                    
                }
            }

            return transfer;
        }
    }
}
