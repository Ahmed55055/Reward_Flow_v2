using Microsoft.EntityFrameworkCore.Diagnostics;
using Reward_Flow_v2.Common.Encryption;
using Reward_Flow_v2.Employees.Data;

namespace Reward_Flow_v2.Employees.Interceptors;

public class EncryptionMaterializationInterceptor(IAesEncryptionService encryption): IMaterializationInterceptor
{
    public object InitializedInstance(MaterializationInterceptionData materializationData, object entity)
    {
        if (entity is Employee employee)
        {
            employee.Name = encryption.DecryptString(employee.Name);
            if (employee.NationalNumber is not null)
                employee.NationalNumber = encryption.DecryptString(employee.NationalNumber);
            if (employee.AccountNumber is not null)
                employee.AccountNumber = encryption.DecryptString(employee.AccountNumber);
        }
        return entity;
    }
}
