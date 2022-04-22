using Example5.Consts;
using Example5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example5.Pages.Categories
{
    public class DetailsModel : PageModel
    {
        private readonly Example5.Data.BookContext _context;
        private readonly IMemoryCache _memoryCache;

        public DetailsModel(Example5.Data.BookContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_memoryCache.TryGetValue(CacheKey.CategoryList, out List<Category> categoryCaches))
            {
                Category = categoryCaches.FirstOrDefault(x => x.Id == id);

                if (Category == null)
                {
                    Category = await _context.Categories.Include(x => x.CategoryType).FirstOrDefaultAsync(m => m.Id == id);
                    categoryCaches.Add(Category);

                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromSeconds(20)
                    };

                    _memoryCache.Set(CacheKey.CategoryList, categoryCaches, cacheExpiryOptions);
                }
            }
            else
                Category = await _context.Categories.Include(x => x.CategoryType).FirstOrDefaultAsync(m => m.Id == id);

            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
