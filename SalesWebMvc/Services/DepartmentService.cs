using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMvcContext _context;
        // SEMPRE ATRIBUA O CONSTRUTOR ABAIXO PARA RECEBER NOSSO CONTEXTODB! 
        public DepartmentService(SalesWebMvcContext context)
        {
            _context = context;
        }

        
        public async Task<List<Department>> FindAllAsync() // O modificador 'async' indica que este método é assíncrono
        {
            // O uso de 'await' aqui faz com que o método espere a conclusão da operação assíncrona sem bloquear a thread.
            // '_context.Department.OrderBy(x => x.Name).ToListAsync()' retorna uma Task que contém a lista de departamentos ordenada.
            return await _context.Department.OrderBy(x => x.Name).ToListAsync(); // Lista ordenada A-Z 
        }

    }
}
