using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using BookStore.Models.Models;

namespace BookStore.Models.ViewModels;

public class ShoppingCartViewModel
{
    [ValidateNever]
    public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
    public OrderHeader OrderHeader { get; set; }
}
