using PhatTrienNhanSu.DbContexts;
using PhatTrienNhanSu.DbContexts.Entities;
using PhatTrienNhanSu.Service.Results;
using Microsoft.EntityFrameworkCore;

public interface IProviderCommand
{
    Task<ServiceResult> CreateProviderAsync(TrainingProvider provider, int creatorId);
    Task<ServiceResult> UpdateProviderAsync(TrainingProvider provider, int updaterId);
    Task<ServiceResult> DeleteProviderAsync(int id, int deleterId);
}

public class ProviderCommand : IProviderCommand
{
    private readonly PhatTrienNhanSuDbContext _context;
    public ProviderCommand(PhatTrienNhanSuDbContext context) { _context = context; }

    public async Task<ServiceResult> CreateProviderAsync(TrainingProvider provider, int creatorId)
    {
        if (await _context.TrainingProviders.AnyAsync(p => p.ProviderCode == provider.ProviderCode))
            return ServiceResult.Fail($"Mã nhà cung cấp '{provider.ProviderCode}' đã tồn tại.");

        provider.CreatedBy = creatorId;
        _context.TrainingProviders.Add(provider);
        await _context.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> UpdateProviderAsync(TrainingProvider provider, int updaterId)
    {
        var existing = await _context.TrainingProviders.FindAsync(provider.Id);
        if (existing == null) return ServiceResult.Fail("Không tìm thấy nhà cung cấp.");

        if (await _context.TrainingProviders.AnyAsync(p => p.ProviderCode == provider.ProviderCode && p.Id != provider.Id))
            return ServiceResult.Fail($"Mã nhà cung cấp '{provider.ProviderCode}' đã được sử dụng.");

        existing.ProviderCode = provider.ProviderCode;
        existing.ProviderName = provider.ProviderName;
        existing.IsInternal = provider.IsInternal;
        existing.Status = provider.Status;
        existing.UpdatedBy = updaterId;

        await _context.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteProviderAsync(int id, int deleterId)
    {
        var provider = await _context.TrainingProviders.FindAsync(id);
        if (provider == null) return ServiceResult.Fail("Không tìm thấy nhà cung cấp.");

        provider.Status = 0; // Xóa mềm
        provider.UpdatedBy = deleterId;

        await _context.SaveChangesAsync();
        return ServiceResult.Success();
    }
}