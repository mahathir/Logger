using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossoverLogger.DTO
{
    public class PagedResult<TEntity>
    {
        IEnumerable<TEntity> _items;
        long _totalCount;

        public PagedResult(IEnumerable<TEntity> items, long totalCount)
        {
            _items = items;
            _totalCount = totalCount;
        }

        public IEnumerable<TEntity> Items { get { return _items; } }
        public long TotalCount { get { return _totalCount; } }
    }
}
