namespace Core.Infrastructure
{
    public interface ISymmetricalProvider
    {
        byte[] GenerateKey(int len);
        byte[] AssymEncrypt(byte[] data, byte[] midulus, byte[] exponent);
        byte[] SymmEncrypt(byte[] data, byte[] key, byte[] iv);
        byte[] GetExponent(string key);
        byte[] GetModulus(string key);
    }
}
