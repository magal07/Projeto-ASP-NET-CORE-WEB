using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller // este controlador recebeu a chamada do caminho /sellers
    {
        private readonly SellerService _sellerService; // declarando dependência para este serviço
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService) // setando a controller para receber nosso serviço
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }
        public IActionResult Index()
        {
            var list = _sellerService.FindAll(); // esta operação vai retornar uma lista de seller

            return View(list); // passando a lista como argumento pra gerar um IActionResult contendo essa lista. (MVC)

            // chamamos o controlador, o controlador acessou nosso model, pegou o dado da lista e vai encaminhar para nossa VIEW!
        }
        public IActionResult Create()
        {
            var departaments = _departmentService.FindAll(); // buscar os deps
            var viewModel = new SellerFormViewModel { Departaments = departaments };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // evita que conteúdos maliciosos sejam enviados pelo nosso Http
        public IActionResult Create(Seller seller) // n precisa acrescentar o dpt id pois o framework já faz a intermediação
        {
            _sellerService.Insert(seller); // método de inserir, agregado ao nosso serviço 
            return RedirectToAction(nameof(Index)); // redireciona nossa inserção para o Index, nameof é para que caso eu mude o nome do Index eu não precise alterar mais nada
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value); // tem q passar como value pois ele é um obj opcional
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj); // caso passe pelos if, entra na nossa View(obj) 
        }
        // Método do delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
