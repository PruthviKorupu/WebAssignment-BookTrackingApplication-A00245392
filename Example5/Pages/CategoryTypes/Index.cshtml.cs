using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Example5.Data;
using Example5.Models;
using Microsoft.Extensions.Caching.Memory;
using Example5.Consts;

namespace Example5.Pages.CategoryTypes
{
    public class IndexModel : PageModel
    {
        private readonly BookContext _context;
        private readonly IMemoryCache _memoryCache;

        public IndexModel(BookContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public IList<CategoryType> CategoryType { get;set; }

        public async Task OnGetAsync()
        {
            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(CacheKey.CategoryTypeList, out List<CategoryType> categoryTypeCaches))
            {
                //calling the server
                categoryTypeCaches = await _context.CategoryTypes.ToListAsync();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };
                //setting cache entries
                _memoryCache.Set(CacheKey.CategoryTypeList, categoryTypeCaches, cacheExpiryOptions);
            }

            CategoryType = categoryTypeCaches;
        }
    }
}
