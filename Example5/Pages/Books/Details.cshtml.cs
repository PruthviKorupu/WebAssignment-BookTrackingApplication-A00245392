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

namespace Example5.Pages.Books
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

        public Book Book { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_memoryCache.TryGetValue(CacheKey.BookList, out List<Book> bookCaches))
            {
                Book = bookCaches.FirstOrDefault(x => x.Id == id);

                if (Book == null)
                {
                    Book = await _context.Books.Include(x => x.Category).FirstOrDefaultAsync(m => m.Id == id);
                    bookCaches.Add(Book);

                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromSeconds(20)
                    };

                    _memoryCache.Set(CacheKey.BookList, bookCaches, cacheExpiryOptions);
                }
            }
            else
            {
                Book = await _context.Books.Include(x => x.Category).FirstOrDefaultAsync(m => m.Id == id);
            }

            if (Book == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
