namespace Reward_Flow_v2.Common.Tokenization;

public interface ITokenizer
{
    IEnumerable<string> TokenizeToNGrams(string text, int n, bool includeSpaces = false);
    string HashToken(string token);
}