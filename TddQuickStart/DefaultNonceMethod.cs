namespace TddQuickStart
{
    public class DefaultNonceMethod : INonceMethod
    {
        public Nonce GenerateNonce()
        {
            return default(Nonce);
        }
    }
}