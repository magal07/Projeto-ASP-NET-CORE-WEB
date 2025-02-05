using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using System.Collections.Generic;
using System.Linq;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
         private readonly SalesWebMvcContext _context;
        // SEMPRE ATRIBUA O CONSTRUTOR ABAIXO PARA RECEBER NOSSO CONTEXTODB! 
        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList(); //acessar a fonte de dados da tabela de vendedores e converter para uma lista.
        }

        public void Insert(Seller obj)
        {    /* só pra buscar o primeiro departamento e acrescenta-lo,
             caso nenhum seja informado, afim de não deixar acarretar em uma exceção */
            obj.Department = _context.Department.First(); 
            _context.Add(obj); // add pelo contexto
            _context.SaveChanges();
        }
    }
}
