using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;

        public SellersController( SellerService sellerService) 
        {
            _sellerService = sellerService;
        }


        public IActionResult Index() 
        {
            var list = _sellerService.FinAll();
            return View(list);
        }

        
    }
}
