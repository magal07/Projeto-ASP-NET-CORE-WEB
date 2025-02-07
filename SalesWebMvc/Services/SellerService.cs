using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using System.Collections.Generic;
using System.Linq;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;
using System.Threading.Tasks;

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

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync(); //acessar a fonte de dados da tabela de vendedores e converter para uma lista.
        }

        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj); // add pelo contexto
           await _context.SaveChangesAsync(); // só precisa no save pois ele quem irá acessar o DB
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller
                .Include(obj => obj.Department) // Fazendo um JOIN no nosso banco para buscar o Departamento
                .FirstOrDefaultAsync(obj => obj.Id == id); // buscando por ID &&somente ele acessa o banco e usa o async
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Seller.FindAsync(id);
            _context.Seller.Remove(obj); // aqui só remove do DBSET
           await _context.SaveChangesAsync(); // CONFIRMA A REMOÇÃO!
        }

        public async Task UpdateAsync(Seller obj)
        {  // hasAny = tem algum? ..." // verificando se existe algum registro no banco de dados conforme o que eu requisitar
            bool hasAny = await _context.Seller
                .AnyAsync(x => x.Id == obj.Id); // Lógica para testar se existe algum id igual ao objeto requisitado
            if (!hasAny) 
            {
                throw new NotFoundException("Id not found");
            }
            try { 
            _context.Update(obj);
            await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e) // interceptando exceção nível de acesso a dados e..
            {
                throw new DbConcurrencyException(e.Message); // relançando com a nossa exceção em nível de serviço, segregando as camadas!
            }
        }
    }
}
