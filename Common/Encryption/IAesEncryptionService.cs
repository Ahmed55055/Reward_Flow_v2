namespace Reward_Flow_v2.Common.Encryption;

public interface IAesEncryptionService
{
    string EncryptString(string plainText);
    string DecryptString(string cipherText);
}