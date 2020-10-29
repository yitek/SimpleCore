
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SimpleCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCore
{
    public class Pagination<TEntity, TItem> where TEntity : class, new() where TItem:class,new()
    {
        public QueryViewModel<TItem> ViewModel { get; private set; }

        public bool ForcePage { get; private set; }
        public Pagination(bool forcePage=true) {
            this.ForcePage = forcePage;
        }
        
        public Pagination(QueryViewModel<TItem> model,bool forcePage=true) {
            this.ViewModel = model;
            this.PageIndex = model.PageIndex;
            this.PageSize = model.PageSize;
            this.ForcePage = forcePage;
        }



        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        public Expression<Func<TEntity, object>> Asc { get; set; }

        public Expression<Func<TEntity, object>> Desc { get; set; }

        public int? PageIndex { get; set; }

        public int? PageSize { get; set; }

        public QueryViewModel<TItem> Execute(DbContext dbContext,Expression<Func<TEntity, TItem>> selector){
            
            var query = this.MakeQuery(dbContext);
            
            this.ViewModel.Total = query.Count();
            if (this.ViewModel.Total > 0)
            {
                this.ViewModel.InternalItems = query.Select(selector).ToList();
            }
            else this.ViewModel.InternalItems = new List<TItem>();
            return this.ViewModel;
        }

        public async Task<QueryViewModel<TItem>> ExecuteAsync(DbContext dbContext, Expression<Func<TEntity, TItem>> selector)
        {
            var result = this.ViewModel;
            var query = this.MakeQuery(dbContext);
            
            result.Total = await query.CountAsync();
            if (result.Total > 0)
            {
                result.InternalItems = await query.Select(selector).ToListAsync();
            }
            else result.InternalItems = new List<TItem>();
            return result;
        }

        

        protected IQueryable<TEntity> MakeQuery(DbContext dbContext) {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();
            if (this.Criteria != null) query = query.Where(this.Criteria);
            if (this.Asc != null) query = query.OrderBy(this.Asc);
            else if (this.Desc != null) query = query.OrderByDescending(this.Desc);
            if (this.PageSize != null || this.PageIndex != null || this.ForcePage)
            {
                int pageSize = this.PageSize ?? 3;
                if (pageSize < 3) pageSize = 3;
                int pageIndex = this.PageIndex ?? 1;
                if (pageIndex < 1) pageIndex = 1;
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                this.ViewModel.PageSize = pageSize;
                this.ViewModel.PageIndex = pageIndex;
            }
            
            return query;
        }
        private Visitor _Visitor;


        public Pagination<TEntity,TItem> Where(Expression<Func<TEntity, bool>> expr) {
            return this.AndAlso(expr);
        }



        public Pagination<TEntity,TItem> AndAlso(Expression<Func<TEntity, bool>> expr) {
            if (expr == null) return this;
            if (this.Criteria == null)
            {
                this.Criteria = expr;
                this._Visitor = new Visitor(expr.Parameters[0]);
            }
            else {
                this.Criteria = Expression.Lambda<Func<TEntity, bool>>(Expression.AndAlso(this.Criteria.Body, this._Visitor.Replace(expr)), this._Visitor.MajorParameter);
            }
      
            return this;
        }

        Pagination<TEntity, TItem> OrElse(Expression<Func<TEntity, bool>> expr)
        {
            if (expr == null) return this;
            if (this.Criteria == null)
            {
                this.Criteria = expr;
                this._Visitor = new Visitor(expr.Parameters[0]);
            }
            else
            {
                this.Criteria = Expression.Lambda<Func<TEntity, bool>>(Expression.OrElse(this.Criteria.Body, this._Visitor.Replace(expr)), this._Visitor.MajorParameter);
            }

            return this;
        }

        class Visitor : System.Linq.Expressions.ExpressionVisitor {
            public ParameterExpression MajorParameter { get; private set; }

            public ParameterExpression ReplacedParameter { get; private set; }
            public Visitor(ParameterExpression param) {
                this.MajorParameter = param;
            }

            public Expression Replace(Expression<Func<TEntity, bool>> expr) {
                this.ReplacedParameter = expr.Parameters[0];
                return this.Visit(expr.Body);
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node == this.ReplacedParameter) return this.MajorParameter;
                return base.VisitParameter(node);
            }
        }

    }
}
