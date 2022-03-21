using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoClient.Models;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        ITransferDAO transferDAO;

        public TransferController(ITransferDAO transferDAO)
        {
            this.transferDAO = transferDAO;
        }

        [HttpPost]
        public ActionResult<bool> AddTransfer(Transfer transfer)
        {
            bool result = transferDAO.AddTransfer(transfer);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPost("request")]
        public ActionResult<bool> RequestTransfer(Transfer transfer)
        {
            bool result = transferDAO.RequestTransfer(transfer);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpGet("{account_id}")]
        public ActionResult<List<Transfer>> GetTransfers(int account_id)
        {
            List<Transfer> transfers = transferDAO.GetTransfers(account_id);

            if (transfers.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(transfers);
            }
        }

        [HttpGet("details/{transfer_id}")]
        public ActionResult<Transfer> GetTransferDetails(int transfer_id)
        {
            Transfer transfer = transferDAO.GetTransferDetail(transfer_id);
            if (transfer != null)
            {
                return Ok(transfer);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
