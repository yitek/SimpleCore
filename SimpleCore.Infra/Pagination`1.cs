using Microsoft.EntityFrameworkCore;
using SimpleCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore
{
    public class Pagination<TEntity>:Pagination<TEntity,TEntity> where TEntity:class,new()
    {
        public Pagination(bool forcePage=true):base(forcePage) { }

        public Pagination(QueryViewModel<TEntity> model,bool forcePage=true):base(model,forcePage)
        {
           
        }


        public QueryViewModel<TEntity> Execute(DbContext dbContext)
        {
            var result = this.ViewModel;
            var query = this.MakeQuery(dbContext);

            result.Total = query.Count();
            if (result.Total > 0)
            {
                result.InternalItems = query.ToList();
            }
            else result.InternalItems = new List<TEntity>();
            return result;
        }

        public async Task<QueryViewModel<TEntity>> ExecuteAsync(DbContext dbContext)
        {
            var result = this.ViewModel;
            var query = this.MakeQuery(dbContext);

            result.Total = await query.CountAsync();
            if (result.Total > 0)
            {
                result.InternalItems = await query.ToListAsync();
            }
            else result.InternalItems = new List<TEntity>();
            return result;
        }

    }
}
