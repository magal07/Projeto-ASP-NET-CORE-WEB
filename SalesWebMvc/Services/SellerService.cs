using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using System.Collections.Generic;
using System.Linq;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;

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
        {    
            _context.Add(obj); // add pelo contexto
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            return _context.Seller
                .Include(obj => obj.Department) // Fazendo um JOIN no nosso banco para buscar o Departamento
                .FirstOrDefault(obj => obj.Id == id); // buscando por ID
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj); // aqui só remove do DBSET
            _context.SaveChanges(); // CONFIRMA A REMOÇÃO!
        }
    }
}
