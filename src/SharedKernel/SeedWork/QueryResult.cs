
using System.Collections.Generic;

namespace SharedKernel.SeedWork
{
    public class QueryResult<T>
    {
        public IEnumerable<T> Entities { get; set; }
        public int TotalCount { get; set; }

        public QueryResult()
        {

        }

        public QueryResult(IEnumerable<T> entities, int totalCount)
        {
            TotalCount = totalCount;

            Entities = entities;
        }
    }
}
