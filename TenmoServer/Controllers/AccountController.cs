using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountController : ControllerBase
    {
        IAccountDAO accountDAO;
        IUserDao userDao;
        public AccountController(IAccountDAO accountDAO, IUserDao userDao)
        {
            this.accountDAO = accountDAO;
            this.userDao = userDao;
        }

        [HttpGet()]
        public ActionResult<List<Account>> GetAccounts()
        {

            List<Account> accounts = accountDAO.GetAccounts();
            //int userId = userDao.GetUser(User.Identity.Name).UserId;
            //Account account = new Account();
            //account.Balance = accountDAO.GetBalance(userId);

            if (accounts.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(accounts);
            }
            
        }

        [HttpGet("{user_id}")]
        public ActionResult<decimal> GetBalance(int user_id)
        {
            decimal balance = -1;
            balance = accountDAO.GetBalance(user_id);

            if (balance != -1)
            {
                return Ok(balance);
            }
            else
            {
                return NotFound();
            }
            
        }
    }
}
