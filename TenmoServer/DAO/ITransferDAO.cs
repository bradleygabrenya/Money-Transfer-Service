using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoClient.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        public bool AddTransfer(Transfer transfer);
        public List<Transfer> GetTransfers(int account_id);

        public Transfer GetTransferDetail(int transfer_id);

        public bool RequestTransfer(Transfer transfer);

    }
}
