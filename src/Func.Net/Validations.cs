using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

using Func.Net.Extensions;

namespace Func.Net
{
    public static class Validations
    {
        public static  T RequireNonNull<T>(T obj, String message) {
            if (obj == null)
                throw new NullReferenceException(message);
            return obj;
        }

        public static void RequireNonNull(params object[] obj) {

            obj.ForEach((o,c)=> RequireNonNull(o,$"parameter {c} is null"));
        }
    }
}
