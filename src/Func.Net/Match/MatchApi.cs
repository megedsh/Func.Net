using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace Func.Net.Match
{
    public static class MatchApi
    {
        public static Match<int> Match(this int value)
        {
            return new Match<int>(value);
        }

        public static IPattern<T> Any<T>()
        {
            return PatternAny<T>.Instance;
        }

        public static ICase<T, object> InstanceOf<T>(Type type)
        {
            return new PatternInstanceOf<T>(type);
        }

        public static ICase<T, R> Case<T, R>(T val, R res)
        {
            return new Case<T, R>(Is(val), (v) => res);
        }

        public static ICase<T, R> Case<T, R>(IPattern<T> pattern , R res)
        {
            return new Case<T, R>(pattern, (v) => res);
        }

        public static PatternProtoType<T> Is<T>(T value)
        {
            return new PatternProtoType<T>(value);
        }
    }

    public static class Case
    {
        public static ICase<T, R>[] ArrayOf<T,R>(params ICase<T, R>[] cases)
        {
            return cases;
        }
    }

    public class Case<T, T1> : ICase<T, T1>
    {
        private readonly IPattern<T> m_pattern;
        private readonly Func<T, T1> m_func;

        public Case(IPattern<T> pattern, Func<T, T1> func)
        {
            m_pattern = pattern;
            m_func = func;
        }

        public T1 Invoke(T var1) => m_func.Invoke(var1);

        public bool IsMatch(T var1) => m_pattern.IsMatch(var1);
    }

    public class PatternProtoType<T> : IPattern<T>
    {
        private readonly T m_prototype;

        public PatternProtoType(T prototype)
        {
            m_prototype = prototype;
        }

        public T Invoke(T var1) => var1;

        public bool IsMatch(T var1) => var1.Equals(m_prototype);
    }

    public class PatternAny<T> : IPattern<T>
    {
        public static PatternAny<T> Instance = new PatternAny<T>();
        public bool IsMatch(T var1) => true;
    }
}
