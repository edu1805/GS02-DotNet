namespace WellWork.Domain.Interfaces;

public interface ICheckInRepository
{
    Task<CheckIn?> GetByIdAsync(Guid id);

    Task AddAsync(CheckIn checkIn);
    Task UpdateAsync(CheckIn checkIn);
    Task DeleteAsync(CheckIn checkIn);

    // Listar check-ins por usuário (com paginação)
    Task<(IEnumerable<CheckIn> Items, long TotalCount)>
        ListByUserAsync(Guid userId, int pageIndex, int pageSize);
}