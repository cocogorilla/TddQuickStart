using System;

namespace TddQuickStart
{
    public class NonceGen
    {
        private readonly ITimeSource _timeSource;

        public NonceGen(INonceMethod method, INonceStore store, ITimeSource timeSource)
        {
            _timeSource = timeSource;
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (store == null) throw new ArgumentNullException(nameof(store));
            if (timeSource == null) throw new ArgumentNullException(nameof(timeSource));
            Method = method;
            Store = store;
        }

        public INonceMethod Method { get; }
        public INonceStore Store { get; }

        public Nonce CreateNonce()
        {
            var nonce = Method.GenerateNonce();
            Store.SaveNonce(nonce);
            return nonce;
        }

        public bool ValidateNonce(string nonceKey, int matchValue)
        {
            var foundNonce = Store.RetrieveNonce(nonceKey);
            return foundNonce.NonceValue == matchValue &&
                foundNonce.NonceExpiration > _timeSource.GetNowEpoch();
        }
    }

    public interface ITimeSource
    {
        long GetNowEpoch();
    }

    public interface INonceMethod
    {
        Nonce GenerateNonce();
    }

    public class DefaultNonceMethod : INonceMethod
    {
        public Nonce GenerateNonce()
        {
            return default(Nonce);
        }
    }

    public interface INonceStore
    {
        void SaveNonce(Nonce input);
        Nonce RetrieveNonce(string nonceKey);
    }

    public class Nonce
    {
        public string NonceKey { get; set; }
        public int NonceValue { get; set; }
        public long NonceExpiration { get; set; }
    }
}
