namespace Shared.Utils
{
    public abstract class Either<L, R>
    {
        public abstract bool IsLeft { get; }
        public abstract bool IsRight { get; }

        public abstract L Left { get; }
        public abstract R Right { get; }

        public static Either<L, R> LeftValue(L value) => new LeftImplementation(value);
        public static Either<L, R> RightValue(R value) => new RightImplementation(value);

        private class LeftImplementation : Either<L, R>
        {
            private readonly L _value;

            public LeftImplementation(L value)
            {
                _value = value;
            }

            public override bool IsLeft => true;
            public override bool IsRight => false;
            public override L Left => _value;
            public override R Right => throw new InvalidOperationException("Cannot access Right value of a Left Either.");
        }

        private class RightImplementation : Either<L, R>
        {
            private readonly R _value;

            public RightImplementation(R value)
            {
                _value = value;
            }

            public override bool IsLeft => false;
            public override bool IsRight => true;
            public override L Left => throw new InvalidOperationException("Cannot access Left value of a Right Either.");
            public override R Right => _value;
        }
    }

}
