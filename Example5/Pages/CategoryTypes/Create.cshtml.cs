using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Example5.Data;
using Example5.Models;
using Microsoft.Extensions.Caching.Memory;
using Example5.Consts;

namespace Example5.Pages.CategoryTypes
{
    public class CreateModel : PageModel
    {
        private readonly Example5.Data.BookContext _context;
        private readonly IMemoryCache _memoryCache;

        public CreateModel(Example5.Data.BookContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CategoryType CategoryType { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CategoryTypes.Add(CategoryType);
            await _context.SaveChangesAsync();

            if (_memoryCache.TryGetValue(CacheKey.CategoryTypeList, out List<CategoryType> categoryTypeCaches))
            {
                categoryTypeCaches.Add(CategoryType);
                
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };

                _memoryCache.Set(CacheKey.CategoryTypeList, categoryTypeCaches, cacheExpiryOptions);
            }

            return RedirectToPage("./Index");
        }
    }
}
