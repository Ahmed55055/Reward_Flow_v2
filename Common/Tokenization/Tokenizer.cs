using System.Security.Cryptography;
using System.Text;

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
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(hash);
    }
}