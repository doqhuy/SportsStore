using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository repository;
        public CartModel(IStoreRepository repo)
        {
            repository = repo;
        }
        public Cart? Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";
        public long ProductID {  get; set; }
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

        }
        public IActionResult OnPost(long productId, string returnUrl,string handler="Add")
        {

            if (handler == "RemoveAll")
            {
                RemoveAll(returnUrl);
            }

            Product? product = repository.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                

                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, 1);
                HttpContext.Session.SetJson("cart", Cart);
                ProductID = productId;

            }
            if (handler == "Remove")
            {
                Remove(productId, returnUrl);
            }

            return RedirectToPage(new { ReturnUrl = returnUrl });
        }

        public IActionResult Remove(long productId, string returnUrl)
        {
            Product? product = repository.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.RemoveLine(product);
                HttpContext.Session.SetJson("cart", Cart);

            }
            return RedirectToPage(new { ReturnUrl= returnUrl });
        }

        public IActionResult RemoveAll(string returnUrl)
        {
            
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            Cart.Clears();
            HttpContext.Session.SetJson("cart", Cart);
            return RedirectToPage(new { ReturnUrl = returnUrl });
        }
    }
}
