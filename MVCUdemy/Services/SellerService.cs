﻿using MVCUdemy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCUdemy.Services.Exceptions;

namespace MVCUdemy.Services
{
    public class SellerService
    {
        private readonly MVCUdemyContext _context;

        public SellerService(MVCUdemyContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();    
        }
        public async Task InsertAsync(Seller sellerObj)
        {
            _context.Add(sellerObj);
           await _context.SaveChangesAsync();
        }
        public async Task<Seller> FindByIdAsync(int id)
        {
            //Inlcude Carrega Relações
            return await _context.Seller.Include(obj=> obj.Department).FirstOrDefaultAsync(x=>x.Id == id);
        }
        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }catch(DbUpdateException e)
            {
                throw new IntegrityException("Can't delete seller because she/he has sales");
            }
            
            
        }
        public async Task UpdateAsync(Seller seller)
        {
            if(!await _context.Seller.AnyAsync(x=>x.Id == seller.Id))
            {
                throw new NotFoundException("Id Not Found");
            }
            try
            {
                _context.Update(seller);
               await _context.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
            
                
        }
    }
}
