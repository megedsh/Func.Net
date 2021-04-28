using System;
using System.Collections.Generic;

namespace Func.Net
{
    //Optional implementation for functional programming

    public static class Optional
    {
        public static Optional<T> Of<T>(T         value)
        {
            Validations.RequireNonNull(value);
            return new Optional<T>(value, true);
        }

        public static Optional<T> OfNullable<T>(T value) => value == null ? Optional<T>.Empty() : Of(value);
        public static Optional<T> Empty<T>()             => Optional<T>.Empty();
    }

    public struct Optional<T> : IEquatable<Optional<T>>, IComparable<Optional<T>>
    {
        public static Optional<T> Empty() => s_empty;
        private static readonly Optional<T> s_empty = new Optional<T>(default(T), false);
        private readonly        T           m_value;
        public bool IsPresent { get; }
        public bool IsEmpty => !IsPresent;

        internal Optional(T value, bool hasValue)
        {
            m_value = value;
            IsPresent = hasValue;
        }

        public T Get()
        {
            if (IsPresent)
            {
                return m_value;
            }

            throw new ArgumentNullException("No value present");
        }

        public void IfPresent(Action<T> consumer)
        {
            if (IsPresent)
            {
                consumer.Invoke(m_value);
            }
        }

        public void IfNotPresent(Action action)
        {
            if (IsPresent)
            {
                action.Invoke();
            }
        }

        public Optional<T> Filter(Func<T, bool> predicate)
        {
            Validations.RequireNonNull(predicate, nameof(predicate));
            if (IsEmpty)
            {
                return this;
            }

            return predicate(m_value) ? this : s_empty;
        }

        public Optional<TResult> Map<TResult>(Func<T, TResult> mapper)
        {
            Validations.RequireNonNull(mapper, nameof(mapper));
            return doMatch(v => Optional.OfNullable(mapper(v)), Optional<TResult>.Empty);
        }

        public Optional<TResult> FlatMap<TResult>(Func<T, Optional<TResult>> mapper)
        {
            Validations.RequireNonNull(mapper, nameof(mapper));
            if (IsPresent)
            {
                return mapper(m_value);
            }

            return Optional<TResult>.Empty();
        }

        public T OrElse(T other) => IsPresent ? m_value : other;

        public T OrElse(Func<T> factory)
        {
            Validations.RequireNonNull(factory, nameof(factory));
            return IsPresent ? m_value : factory();
        }

        public Optional<T> OrOptional(T other) => IsPresent ? this : Optional.OfNullable(other);

        public Optional<T> OrOptional(Func<T> factory)
        {
            Validations.RequireNonNull(factory, nameof(factory));
            return IsPresent ? this : Optional.OfNullable(factory());
        }

        public T OrElseThrow<TException>(Func<TException> exceptionFactory)
            where TException : Exception
        {
            Validations.RequireNonNull(exceptionFactory, nameof(exceptionFactory));
            if (IsPresent)
            {
                return m_value;
            }

            throw exceptionFactory.Invoke();
        }

        public TResult Match<TResult>(Func<T, TResult> onPresent, Func<TResult> onEmpty)
        {
            return doMatch(onPresent, onEmpty);
        }

        private TResult doMatch<TResult>(Func<T, TResult> onPresent, Func<TResult> onEmpty)
        {
            Validations.RequireNonNull(onPresent, nameof(onPresent));
            Validations.RequireNonNull(onEmpty,   nameof(onEmpty));
            return IsPresent ? onPresent(m_value) : onEmpty();
        }

        public void Match(Action<T> onPresent, Action onEmpty)
        {
            Validations.RequireNonNull(onPresent, nameof(onPresent));
            Validations.RequireNonNull(onEmpty,   nameof(onEmpty));
            if (IsPresent)
            {
                onPresent(m_value);
            }
            else
            {
                onEmpty();
            }
        }

        public Optional<T> PeekEmpty(Action onEmpty)
        {
            Validations.RequireNonNull(onEmpty,   nameof(onEmpty));
            if (IsEmpty)
                onEmpty();
            return this;
        }

        public Optional<T> PeekPresent(Action<T> onPresent)
        {
            Validations.RequireNonNull(onPresent, nameof(onPresent));
            if (IsPresent)
                onPresent(m_value);
            return this;
        }

        public bool Equals(Optional<T> other)
        {
            if (IsEmpty && other.IsEmpty)
            {
                return true;
            }

            if (IsPresent && other.IsPresent)
            {
                return EqualityComparer<T>.Default.Equals(m_value, other.m_value);
            }

            return false;
        }

        public override bool Equals(object obj) => obj is Optional<T> ? Equals((Optional<T>)obj) : false;

        public override int GetHashCode()
        {
            if (IsPresent)
            {
                if (m_value == null)
                {
                    return 1;
                }

                return m_value.GetHashCode();
            }

            return 0;
        }

        public int CompareTo(Optional<T> other)
        {
            if (IsPresent && other.IsEmpty)
            {
                return 1;
            }

            if (IsEmpty && other.IsPresent)
            {
                return -1;
            }

            return Comparer<T>.Default.Compare(m_value, other.m_value);
        }

        public override string ToString()
        {
            if (IsPresent)
            {
                if (m_value == null)
                {
                    return "Optional[null]";
                }

                return $"Optional[{m_value}]";
            }

            return "Optional[Empty]";
        }
    }
}