using Example5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Example5.Data
{
    public static class SeedData
    {
        public static void Initialize(this IServiceProvider serviceProvider)
        {
            using var context = new BookContext(serviceProvider.GetRequiredService<DbContextOptions<BookContext>>());

            if (!context.CategoryTypes.Any())
            {
                context.CategoryTypes.AddRange(
                    new CategoryType
                    {
                        Name = "Comic",
                        Type = "Type 1",
                        Categories = new List<Category>
                        {
                            new Category
                            {
                                Description = "Funny",
                                NameToken = "Demo",
                                Books = new List<Book>
                                {
                                    new Book
                                    {
                                        Title = "On Tyranny Graphic Edition: Twenty Lessons from the Twentieth Century",
                                        ISBN = "1984859153",
                                        Author = "Timothy Snyder"
                                    },
                                    new Book
                                    {
                                        Title = "Naruto, Vol. 1, 1",
                                        ISBN = "1569319006",
                                        Author = "Masashi Kishimoto"
                                    }
                                }
                            }
                        }
                    },
                    new CategoryType
                    {
                        Name = "Horror",
                        Type = "Type 2",
                        Categories = new List<Category>
                        {
                            new Category
                            {
                                Description = "Horror",
                                NameToken = "Demo",
                                Books = new List<Book>
                                {
                                    new Book
                                    {
                                        Title = "The Letters of Mina Harker",
                                        ISBN = "1635901596",
                                        Author = "Dodie Bellamy"
                                    },
                                    new Book
                                    {
                                        Title = "The Death of Jane Lawrence",
                                        ISBN = "1250272580",
                                        Author = "Caitlin Starling"
                                    }
                                }
                            }
                        }
                    }
                );
            }

            context.SaveChanges();
        }
    }
}
