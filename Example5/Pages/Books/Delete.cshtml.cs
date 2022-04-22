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
    public class DeleteModel : PageModel
    {
        private readonly Example5.Data.BookContext _context;
        private readonly IMemoryCache _memoryCache;

        public DeleteModel(Example5.Data.BookContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        [BindProperty]
        public Book Book { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book = await _context.Books
                .Include(b => b.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (Book == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book = await _context.Books.FindAsync(id);

            if (Book != null)
            {
                _context.Books.Remove(Book);
                await _context.SaveChangesAsync();

                if (_memoryCache.TryGetValue(CacheKey.BookList, out List<Book> bookCaches))
                {
                    bookCaches.RemoveAll(x => x.Id == Book.Id);

                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromSeconds(20)
                    };

                    _memoryCache.Set(CacheKey.CategoryTypeList, bookCaches, cacheExpiryOptions);
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
