using Example5.Consts;
using Example5.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example5.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly Example5.Data.BookContext _context;
        private readonly IMemoryCache _memoryCache;

        public IndexModel(Example5.Data.BookContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public IList<Category> Category { get;set; }

        public async Task OnGetAsync()
        {
            if (!_memoryCache.TryGetValue(CacheKey.CategoryList, out List<Category> categoryCaches))
            {
                //calling the server
                categoryCaches = await _context.Categories.ToListAsync();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };
                //setting cache entries
                _memoryCache.Set(CacheKey.CategoryList, categoryCaches, cacheExpiryOptions);
            }

            Category = categoryCaches;
        }
    }
}
