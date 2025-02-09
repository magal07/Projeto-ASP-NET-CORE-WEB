﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync(); // esta operação vai retornar uma lista de seller

            return View(list); // passando a lista como argumento pra gerar um IActionResult contendo essa lista. (MVC)

            // chamamos o controlador, o controlador acessou nosso model, pegou o dado da lista e vai encaminhar para nossa VIEW!
        }
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync(); // buscar os deps
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // evita que conteúdos maliciosos sejam enviados pelo nosso Http
        public async Task<IActionResult> Create(Seller seller) // n precisa acrescentar o dpt id pois o framework já faz a intermediação
        {  /* o if abaixo, representa navegadores ou ocasiões onde o JS tá desabilitado na WEB, e a requisição não é instanciada
              desta forma, o if valida que se não chegar nenhuma requisição por conta do JS desabilitado ou afins, é pra BARRAR a edição! */
            if (!ModelState.IsValid)
            {           
                var departments = await _departmentService.FindAllAsync(); // Obtém todos os departamentos através do serviço _departmentService
                var viewModel = new SellerFormViewModel // Cria uma instância do ViewModel SellerFormViewModel com os dados do vendedor e a lista de departamentos
                {
                    Seller = seller, // Define o vendedor no ViewModel
                    Departments = departments // Define a lista de departamentos no ViewModel
                };
                // Retorna a visão (View) com o ViewModel criado
                return View(viewModel);
            }
               await _sellerService.InsertAsync(seller); // método de inserir, agregado ao nosso serviço 
            return RedirectToAction(nameof(Index)); // redireciona nossa inserção para o Index, nameof é para que caso eu mude o nome do Index eu não precise alterar mais nada
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided"});
            }

            var obj = await _sellerService.FindByIdAsync(id.Value); // tem q passar como value pois ele é um obj opcional
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj); // caso passe pelos if, entra na nossa View(obj) 
        }
        // Método do delete acrescentado!
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {           // tratando exceções
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new {message = e.Message});
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id  not provided" });
            }
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {

                return RedirectToAction(nameof(Error), new { message = "Id  not found" });
            }
            return View(obj);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id  not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id  not found" });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            /* o if abaixo, representa navegadores ou ocasiões onde o JS tá desabilitado na WEB, e a requisição não é instanciada
              desta forma, o if valida que se não chegar nenhuma requisição por conta do JS desabilitado ou afins, é pra BARRAR a edição! */
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync(); // Obtém todos os departamentos através do serviço _departmentService
                var viewModel = new SellerFormViewModel // Cria uma instância do ViewModel SellerFormViewModel com os dados do vendedor e a lista de departamentos
                {
                    Seller = seller, // Define o vendedor no ViewModel
                    Departments = departments // Define a lista de departamentos no ViewModel
                };
            }
                if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }

           /* Podemos utilizar o appexception genérico, pois ele busca o upcasting do dbconcrr...
             catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            } */ 

            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            // Cria uma instância de ErrorViewModel com a mensagem de erro fornecida e o ID da requisição atual
            var viewModel = new ErrorViewModel
            {
                Message = message, // Define a mensagem de erro recebida como argumento
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier // Obtém o ID da atividade atual ou do identificador de rastreamento do HTTP
            }; // Activity faz parte do using System.Diagnostics

            // Retorna a visão de erro junto com o ViewModel
            return View(viewModel);
        }
    }
}
