using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller // este controlador recebeu a chamada do caminho /sellers
    {
        private readonly SellerService _sellerService; // declarando dependência para este serviço

        public SellersController(SellerService sellerService) // setando a controller para receber nosso serviço
        {
            _sellerService = sellerService;
        }
        public IActionResult Index()
        {
            var list = _sellerService.FindAll(); // esta operação vai retornar uma lista de seller

            return View(list); // passando a lista como argumento pra gerar um IActionResult contendo essa lista. (MVC)

            // chamamos o controlador, o controlador acessou nosso model, pegou o dado da lista e vai encaminhar para nossa VIEW!
        }
    }
}
