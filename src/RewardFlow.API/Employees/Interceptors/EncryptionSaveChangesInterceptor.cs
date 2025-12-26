using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Reward_Flow_v2.Common.Encryption;
using Reward_Flow_v2.Employees.Data;

namespace Reward_Flow_v2.Employees.Interceptors;

public class EncryptionSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IAesEncryptionService _encryption;

    public EncryptionSaveChangesInterceptor(IAesEncryptionService encryptionService)
    {
        this._encryption = encryptionService;
    }
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            foreach (var entry in eventData.Context.ChangeTracker.Entries<Employee>())
            {
                if (entry.State is EntityState.Added or EntityState.Modified)
                {
                    entry.Entity.Name = _encryption.EncryptString(entry.Entity.Name);

                    if (entry.Entity.NationalNumber is not null)
                        entry.Entity.NationalNumber = _encryption.EncryptString(entry.Entity.NationalNumber);

                    if (entry.Entity.AccountNumber is not null)
                        entry.Entity.AccountNumber = _encryption.EncryptString(entry.Entity.AccountNumber);

                }
            }
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
