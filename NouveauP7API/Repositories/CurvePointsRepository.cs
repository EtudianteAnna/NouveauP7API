﻿using Microsoft.EntityFrameworkCore;
using NouveauP7API.Data;
using NouveauP7API.Models;

namespace NouveauP7API.Repositories
{
    public class CurvePointsRepository : ICurvePointRepository
    {
        private readonly LocalDbContext _context;

        public CurvePointsRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CurvePoints>> GetAllAsync()
        // Ajouter la vérification pour éviter CS8604
        {
             
          return await _context.CurvePoints.ToListAsync();

        }

        public async Task<CurvePoints> GetByIdAsync(int id)
        {
#pragma warning disable CS8603 // Existence possible d'un retour de référence null.
            return await _context.CurvePoints.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id); ;
#pragma warning restore CS8603 // Existence possible d'un retour de référence null.
        }

        public async Task AddAsync(CurvePoints curvePoint)
        {
            _context.CurvePoints.Add(curvePoint);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CurvePoints curvePoint)
        {
            _context.CurvePoints.Update(curvePoint);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var curvePoint = await _context.CurvePoints.FindAsync(id);
            if (curvePoint != null)
            {
                _context.CurvePoints.Remove(curvePoint);
                await _context.SaveChangesAsync();
            }
        }
    }
}
    

    