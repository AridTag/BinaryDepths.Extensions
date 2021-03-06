﻿using System.Threading;

namespace BinaryDepths.Extensions
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
