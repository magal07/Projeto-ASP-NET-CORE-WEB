using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // evita que conteúdos maliciosos sejam enviados pelo nosso Http
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller); // método de inserir, agregado ao nosso serviço 
            return RedirectToAction(nameof(Index)); // redireciona nossa inserção para o Index, nameof é para que caso eu mude o nome do Index eu não precise alterar mais nada
        }

    }
}
