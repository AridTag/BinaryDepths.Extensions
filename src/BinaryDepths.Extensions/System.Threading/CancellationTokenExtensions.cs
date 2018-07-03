using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace System.Threading
{
    public static class CancellationTokenExtensions
    {
        /// <summary>
        /// Creates a <see cref="CancellationTokenSource"/> linked with this token. Or a new CancellationTokenSource if the token is default
        /// </summary>
        /// <param name="sourceToken"></param>
        /// <returns></returns>
        public static CancellationTokenSource CreateLinkedTokenSource(this CancellationToken? sourceToken)
        {
            if (sourceToken == default)
                return new CancellationTokenSource();

            return CancellationTokenSource.CreateLinkedTokenSource(sourceToken.Value);
        }
    }
}
