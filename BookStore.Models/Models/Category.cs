using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace BookStore.Models.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [MaxLength(30)]
    public string Name { get; set; }
    [DisplayName("Display Order")]
    [Range(1, 30, ErrorMessage ="Greska!")]
    public int DisplayOrder { get; set; }
}

