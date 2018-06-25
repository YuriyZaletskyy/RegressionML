using System;
using System.Collections.Generic;
using System.Text;

namespace Regression.Common.Base
{
    /// <summary>
    /// Base class for SingleThreadedNormalizer, MultipleThreadedNormalizer, some other normalizer
    /// </summary>
    public abstract class BaseNormalizer
    {
        public abstract Matrix Normalize(Matrix matrixForNormalization);
    }
}
