using NouveauP7API.Domain;

namespace NouveauP7API.Repositories
{
    public interface ICurvePointRepository
    {
        Task<CurvePoints> GetByIdAsync(int id);
        Task AddAsync(CurvePoints curvePoint);
        Task UpdateAsync(CurvePoints curvePoint);
        Task DeleteAsync(int id);
    }
}