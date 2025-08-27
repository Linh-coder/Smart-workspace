using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Domain.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>>? Criteria { get; protected set; }

        public List<Expression<Func<T, object>>> Includes { get; } = new();

        public Expression<Func<T, object>> OrderBy {  get; protected set; }

        public Expression<Func<T, object>> OrderByDesc { get; protected set; }

        public int? Take { get; protected set; }

        public int? Skip { get; protected set; }

        public bool IsPagingEnabled { get; protected set; }

        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected BaseSpecification() {  }

        protected void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression) => OrderBy = orderByExpression;
        protected void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression) => OrderByDesc = orderByDescExpression;

        protected void ApplyPaging(int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;
            Skip = (page - 1) * pageSize;
            Take = pageSize;
            IsPagingEnabled = true;
        }
    }
}
