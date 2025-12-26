using System.Security.Cryptography;
using System.Text;
using Extensions.Data;

namespace Reward_Flow_v2.Common.Hashing;

public static class XxHasher
{

    public static string Hash(string input)
    {
        var rawBytes = Encoding.UTF8.GetBytes(input);

        using HashAlgorithm xxhash = XXHash64.Create();
        var result = xxhash.ComputeHash(rawBytes);

        return Convert.ToBase64String(result.ToArray());
    }
}