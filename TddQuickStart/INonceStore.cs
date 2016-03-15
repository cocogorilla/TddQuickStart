namespace TddQuickStart
{
    public interface INonceStore
    {
        void SaveNonce(Nonce input);
        Nonce RetrieveNonce(string nonceKey);
    }
}