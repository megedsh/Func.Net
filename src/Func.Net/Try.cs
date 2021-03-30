using System;

namespace Func.Net
{
    public interface ITry
    {
        Exception Cause     { get; }
        bool      IsFailure { get; }
        bool      IsSuccess { get; }
        ITry OnFailure(Action<Exception> action);
        ITry OnSuccess(Action action);
        ITry Match(Action onSuccessAction, Action<Exception> onExceptionAction);
        void Finally(Action action);
    }

    public interface ITry<T> : ITry
    {
        T Get();
        new Try<T> OnFailure(Action<Exception> action);
        new Try<T> OnSuccess(Action<T> action);
        new Try<T> Match(Action<T> onSuccessAction, Action<Exception> onExceptionAction);
        Try<T> OrElseTry(Try<T> other);
        Try<T> OrElseTry(Func<Exception, Try<T>> otherTryFactory);
        T OrElse(T otherValue);
        T OrElse(Func<Exception, T> otherFactory);
        T OrElseThrow();
        Optional<T> ToOptional();
        Either<T, Exception> ToEither();
    }

    public static class Try
    {
        public static ITry Run(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            try
            {
                action();
                return Success<object>(null);
            }
            catch (Exception ex)
            {
                return Failure<object>(ex);
            }
        }

        

        public static ITry<T> Of<T>(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            try
            {
                return Success(func.Invoke());
            }
            catch (Exception ex)
            {
                return Failure<T>(ex);
            }
        }

        internal static ITry<T> Failure<T>(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return new Failure<T>(exception);
        }

        internal static ITry<T> Success<T>(T o) => new Success<T>(o);
    }

    public abstract class Try<T> : ITry<T>
    {
        public abstract bool      IsFailure { get; }
        public abstract bool      IsSuccess { get; }
        public abstract T         Get();
        public abstract Exception Cause { get; }

        ITry ITry.OnFailure(Action<Exception> action) => OnFailure(action);
        public ITry OnSuccess(Action action)
        {
            Validations.RequireNonNull(action);

            if (IsSuccess)
            {
                action();
            }
            return this;
        }
        
        public Try<T> OnFailure(Action<Exception> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (IsFailure)
            {
                action(Cause);
            }

            return this;
        }

        public Try<T> OnSuccess(Action<T> action)
        {
            Validations.RequireNonNull(action);

            if (IsSuccess)
            {
                action(Get());
            }

            return this;
        }

        public ITry Match(Action onSuccessAction, Action<Exception> onExceptionAction)
        {
            Validations.RequireNonNull(onSuccessAction,   nameof(onSuccessAction));
            Validations.RequireNonNull(onExceptionAction, nameof(onExceptionAction));
            if (IsSuccess)
            {
                onSuccessAction();
            }
            else
            {
                onExceptionAction(Cause);
            }

            return this;
        }

        public Try<T> Match(Action<T> onSuccessAction, Action<Exception> onExceptionAction)
        {
            Validations.RequireNonNull(onSuccessAction, nameof(onSuccessAction));
            Validations.RequireNonNull(onExceptionAction, nameof(onExceptionAction));
            if (IsSuccess)
            {
                onSuccessAction(Get());
            }
            else
            {
                onExceptionAction(Cause);
            }

            return this;
        }

        public Try<T> OrElseTry(Try<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return IsSuccess ? this : other;
        }

        public Try<T> OrElseTry(Func<Exception, Try<T>> otherTryFactory)
        {
            if (otherTryFactory == null)
            {
                throw new ArgumentNullException(nameof(otherTryFactory));
            }

            return IsSuccess ? this : otherTryFactory(Cause);
        }

        public T OrElse(T otherValue)
        {
            if (otherValue == null)
            {
                throw new ArgumentNullException(nameof(otherValue));
            }

            return IsSuccess ? Get() : otherValue;
        }

        public T OrElse(Func<Exception, T> otherFactory)
        {
            if (otherFactory == null)
            {
                throw new ArgumentNullException(nameof(otherFactory));
            }

            return IsSuccess ? Get() : otherFactory(Cause);
        }

        public T OrElseThrow() => IsSuccess ? Get() : throw Cause;

        public void Finally(Action action)
        {
            if (action == null) { throw new ArgumentNullException(nameof(action)); }
            action();
        }

        public Optional<T> ToOptional() => IsFailure ? Optional.Empty<T>() : Optional.OfNullable(Get());

        public Either<T, Exception> ToEither() => IsSuccess ? new Either<T, Exception>(Get()) : new Either<T, Exception>(Cause);
    }

    public class Success<T> : Try<T>
    {
        private readonly T m_value;

        public Success(T value) => m_value = value;

        public override T         Get()      => m_value;
        public override Exception Cause      => null;
        public override bool      IsFailure  => false;
        public override bool      IsSuccess  => true;
        public override string    ToString() => $"Success : + [{m_value}]";
    }

    public class Failure<T> : Try<T>
    {
        private readonly Exception m_cause;

        internal Failure(Exception cause) => m_cause = cause;

        public override Exception Cause     => m_cause;
        public override bool      IsFailure => true;
        public override bool      IsSuccess => false;
        public override T         Get()     => throw m_cause;

        public override string ToString() => $"Failure : + [{m_cause}]";
    }
}