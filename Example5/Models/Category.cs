using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Example5.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long TypeId { get; set; }
        public string NameToken { get; set; }
        public string Description { get; set; }
        [ForeignKey(nameof(TypeId))]
        public virtual CategoryType CategoryType { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
