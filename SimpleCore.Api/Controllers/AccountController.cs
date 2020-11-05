using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleCore.Domains;
using SimpleCore.Api.Domains;
using SimpleCore.Api.Models;

namespace SimpleCore.Api.Controllers
{
    /// <summary>
    /// 账号管理
    /// </summary>
    [Route("[controller]")]
    [ApiVersion("1.0", Deprecated=true)]
    [ApiController]
    public class AccountController : SimpleCore.ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public AccountController(MyDbContext context,ILogger<AccountController> logger)
        {
            this._context = context;
            this._logger = logger;

        }

        /// <summary>
        /// GET: api/Account
        /// 获取账号列表
        /// </summary>
        /// <param name="qryModel"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<AccountQueryViewModel>> GetAccount([FromQuery]AccountQueryViewModel qryModel)
        {
            var pagination = new Pagination<Account>(qryModel);
            return await pagination.ExecuteAsync(this._context) as AccountQueryViewModel;
        }

        /// <summary>
        /// GET: api/Account/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(Guid id)
        {
            var account = await _context.Account.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        /// <summary>
        /// PUT: api/Account/5
        /// To protect from overposting attacks, enable the specific properties you want to bind to, for
        /// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(Guid id, Account account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        /// <summary>
        /// POST: api/Account
        /// To protect from overposting attacks, enable the specific properties you want to bind to, for
        /// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = account.Id }, account);
        }

        /// <summary>
        /// DELETE: api/Account/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Account>> DeleteAccount(Guid id)
        {
            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();

            return account;
        }

        private bool AccountExists(Guid id)
        {
            return _context.Account.Any(e => e.Id == id);
        }
    }
}
