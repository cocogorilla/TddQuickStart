using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TddQuickStart
{
    public class NonceGen
    {
        public NonceGen(INonceMethod method, INonceStore store)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (store == null) throw new ArgumentNullException(nameof(store));
            Method = method;
            Store = store;
        }

        public INonceMethod Method { get; }
        public INonceStore Store { get; }

        public Nonce CreateNonce()
        {
            return Method.GenerateNonce();
        }
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

    public interface INonceStore { }

    public class Nonce
    {
        public int NonceValue { get; set; }
        public long NonceExpiration { get; set; }
    }
}
