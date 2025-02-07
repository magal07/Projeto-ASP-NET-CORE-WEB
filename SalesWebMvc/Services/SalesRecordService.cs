using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;


namespace SalesWebMvc.Services
{
    public class SalesRecordService 
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }
                    // OPERAÇÃO ASSÍNCRONA QUE BUSCA MEUS REGISTRO DE VENDAS POR DATA!
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result
                    .Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result
                    .Where(x => x.Date <= maxDate.Value);
            }
            return await result
                .Include(x => x.Seller) // x q leva em x.Seller, (faz o join das tabelas)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }
    }
}
