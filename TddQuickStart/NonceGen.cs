﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TddQuickStart
{
    public class NonceGen
    {
        public NonceGen(INonceMethod method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            Method = method;
        }

        public INonceMethod Method { get; }
    }

    public interface INonceMethod { }

    public class DefaultNonceMethod : INonceMethod { }
}
