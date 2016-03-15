using System;

namespace TddQuickStart
{
    public class NonceGen
    {
        private readonly ITimeSource _timeSource;

        public NonceGen(INonceMethod method, INonceStore store, ITimeSource timeSource)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (store == null) throw new ArgumentNullException(nameof(store));
            if (timeSource == null) throw new ArgumentNullException(nameof(timeSource));
            _timeSource = timeSource;
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
}
