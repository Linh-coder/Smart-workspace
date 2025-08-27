using Microsoft.EntityFrameworkCore;
using SmartWorkspace.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorkspace.Persistence.Extensions
{
    public static class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
        {
            if (spec.Criteria != null) query = query.Where(spec.Criteria);
            if (spec.OrderBy != null) query = query.OrderBy(spec.OrderBy);
            if (spec.OrderByDesc != null) query = query.OrderByDescending(spec.OrderByDesc);
            if (spec.IsPagingEnabled) query = query.Skip(spec.Skip!.Value).Take(spec.Take!.Value);

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
