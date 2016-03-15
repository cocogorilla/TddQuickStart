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
    }

    public interface INonceMethod { }

    public class DefaultNonceMethod : INonceMethod { }

    public interface INonceStore { }
}
