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
    public class DetailsModel : PageModel
    {
        private readonly Example5.Data.BookContext _context;
        private readonly IMemoryCache _memoryCache;

        public DetailsModel(Example5.Data.BookContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public CategoryType CategoryType { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_memoryCache.TryGetValue(CacheKey.CategoryTypeList, out List<CategoryType> categoryTypeCaches))
            {
                CategoryType = categoryTypeCaches.FirstOrDefault(x => x.Id == id);

                if (CategoryType == null)
                {
                    CategoryType = await _context.CategoryTypes.FirstOrDefaultAsync(m => m.Id == id);
                    categoryTypeCaches.Add(CategoryType);
                    
                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromSeconds(20)
                    };

                    _memoryCache.Set(CacheKey.CategoryTypeList, categoryTypeCaches, cacheExpiryOptions);
                }
            }
            else
                CategoryType = await _context.CategoryTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (CategoryType == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
