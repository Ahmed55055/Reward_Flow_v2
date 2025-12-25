using System.Security.Cryptography;
using System.Text;
using Extensions.Data;

namespace Reward_Flow_v2.Common.Tokenization;

public class Tokenizer : ITokenizer
{
    public IEnumerable<string> TokenizeToNGrams(string text, int n, bool includeSpaces = false)
    {
        if (string.IsNullOrEmpty(text) || n <= 0 || n > text.Length)
            return Enumerable.Empty<string>();

        var processedText = includeSpaces ? text : text.Replace(" ", "");
        
        return Enumerable.Range(0, processedText.Length - n + 1)
            .Select(i => processedText.Substring(i, n));
    }

    public string HashToken(string token)
    {
        var rawBytes = Encoding.UTF8.GetBytes(token);

        using HashAlgorithm xxhash = XXHash32.Create();
        var result = xxhash.ComputeHash(rawBytes);

        return Convert.ToBase64String(result.Take(16).ToArray());
    }
}