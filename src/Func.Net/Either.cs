using System;

namespace Func.Net
{
    public struct Either<TL, TR>
    {
        private readonly TL m_left;
        private readonly TR m_right;

        public Either(TL left)
        {
            m_left = left;
            m_right = default(TR);
            IsLeft = true;
        }

        public Either(TR right)
        {
            m_right = right;
            m_left = default(TL);
            IsLeft = false;
        }

        public bool IsRight => !IsLeft;
        public bool IsLeft { get; }

        public T Match<T>(Func<TL, T> leftFunc, Func<TR, T> rightFunc)
        {
            if (leftFunc == null)
            {
                throw new ArgumentNullException(nameof(leftFunc));
            }

            if (rightFunc == null)
            {
                throw new ArgumentNullException(nameof(rightFunc));
            }

            return IsLeft ? leftFunc(m_left) : rightFunc(m_right);
        }

        public void Match (Action<TL> leftConsumer, Action<TR> rightConsumer)
        {
            if (leftConsumer == null)
            {
                throw new ArgumentNullException(nameof(leftConsumer));
            }

            if (rightConsumer == null)
            {
                throw new ArgumentNullException(nameof(rightConsumer));
            }

            if (IsLeft)
            {
                leftConsumer(m_left);
            }
            else
            {
                rightConsumer(m_right);
            }

            
        }

        public T Match<T>(Action<TL> leftConsumer, Func<TR, T> rightFunc)
        {
            if (leftConsumer == null)
            {
                throw new ArgumentNullException(nameof(leftConsumer));
            }

            if (rightFunc == null)
            {
                throw new ArgumentNullException(nameof(rightFunc));
            }

            if (IsLeft)
            {
                leftConsumer(m_left);
                return default(T);
            }
            else
            {
                return rightFunc(m_right);
            }
        }

        public T Match<T>(Func<TL, T> leftFunc, Action<TR> rightConsumer)
        {
            if (leftFunc == null)
            {
                throw new ArgumentNullException(nameof(leftFunc));
            }

            if (rightConsumer == null)
            {
                throw new ArgumentNullException(nameof(rightConsumer));
            }

            if (IsLeft)
            {
                return leftFunc(m_left);
                
            }
            else
            {
                rightConsumer(m_right);
                return default(T);
            }
        }

        public void DoRight(Action<TR> rightAction)
        {
            if (rightAction == null)
            {
                throw new ArgumentNullException(nameof(rightAction));
            }

            if (!IsLeft)
            {
                rightAction(m_right);
            }
        }

        public void DoLeft(Action<TL> leftAction)
        {
            if (leftAction == null)
            {
                throw new ArgumentNullException(nameof(leftAction));
            }

            if (IsLeft)
            {
                leftAction(m_left);
            }
        }

        public TL LeftOrDefault() => Match(l => l, r => default(TL));

        public TR RightOrDefault() => Match(l => default(TR), r => r);

        public static implicit operator Either<TL, TR>(TL left) => new Either<TL, TR>(left);

        public static implicit operator Either<TL, TR>(TR right) => new Either<TL, TR>(right);
    }
}
