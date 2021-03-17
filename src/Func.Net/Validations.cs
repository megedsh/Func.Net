using System;
using System.Collections.Generic;
using System.Text;

namespace Func.Net
{
    public static class Validations
    {
        public static  T RequireNonNull<T>(T obj, String message) {
            if (obj == null)
                throw new NullReferenceException(message);
            return obj;
        }
    }
}
