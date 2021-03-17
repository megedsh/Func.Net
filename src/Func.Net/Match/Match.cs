using System;
using System.Reflection.Metadata.Ecma335;

namespace Func.Net.Match
{


    public interface IMatch<T>
    {
        bool IsMatch(T var1);
    }


    public interface ICase<T, R> : IMatch<T>
    {
        R Invoke(T var1);
    }

    public interface IPattern<T> : IMatch<T>
    {
    }
    

    public class PatternInstanceOf<T> : ICase<T, object>
    {
        private readonly Type m_type;
        public PatternInstanceOf(Type type)
        {
            m_type = type;
        }

        public        object        Invoke(T var1) => var1;
        public bool IsMatch(T var1)
        {
            Validations.RequireNonNull(var1, "object matched is null");
            return var1.GetType().IsAssignableFrom(m_type);
        }
    }
    

    public class Match<T>
    {
        private readonly T m_value;

        public Match(T value)
        {
            m_value = value;
        }

        public TR Of<TR>(params ICase<T, TR>[] cases)
        {
            Validations.RequireNonNull(cases, "cases is null");

            foreach (ICase<T, TR> @case in cases)
            {
                if (@case.IsMatch(m_value))
                {
                    return @case.Invoke(m_value);
                }
            }

            throw new ArgumentOutOfRangeException(nameof(m_value), m_value, "No Match found");
        }

        public Optional<TR> OptionalOf<TR>(params ICase<T, TR>[] cases)
        {
            Validations.RequireNonNull(cases, "cases is null");

            foreach (ICase<T, TR> @case in cases)
            {
                if (@case.IsMatch(m_value))
                {
                    return Optional.OfNullable(@case.Invoke(m_value));
                }
            }

            return Optional.Empty<TR>();
        }

    }


}
