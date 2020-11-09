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
using Microsoft.AspNetCore.Cors;
using SimpleCore.Api.Domians;

namespace SimpleCore.Api.Controllers
{
    /// <summary>
    /// 账号管理
    /// </summary>
    [Route("[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController]

    public class ArticleController : SimpleCore.ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public ArticleController(MyDbContext context, ILogger<AccountController> logger)
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
        public async Task<ActionResult<ArticleQueryViewModel>> GetArticle([FromQuery] ArticleQueryViewModel qryModel)
        {
            var pagination = new Pagination<Article>(qryModel);
            if (!string.IsNullOrEmpty(qryModel.Title)) {
                pagination.Where(p=>p.Title.Contains(qryModel.Title));
            }
            return await pagination.ExecuteAsync(this._context) as ArticleQueryViewModel;
        }

        /// <summary>
        /// GET: api/Account/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetAccount(Guid id)
        {
            var account = await _context.Article.FindAsync(id);
            


            var acconts = _context.Article.Where(art => art.Title.StartsWith("[dotnet]") && art.Content == "neirong").ToList();

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
        /// <param name="article"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(Guid id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

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
        /// <param name="article"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Article article)
        {
            if (article.Id == Guid.Empty) article.Id = Guid.NewGuid();
            _context.Article.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = article.Id }, article);
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
