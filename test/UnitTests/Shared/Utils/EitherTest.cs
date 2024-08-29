using Shared.Utils;

namespace UnitTests.Shared.Utils
{
    public class EitherTest
    {
        class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        [Fact]
        public void ShouldValidateLeftImplementation()
        {
            var either = Either<string, int>.LeftValue("An error occurred");

            Assert.True(either.IsLeft);
            Assert.IsType<string>(either.Left);
            Assert.Equal("An error occurred", either.Left);

            Assert.False(either.IsRight);
            var exception = Assert.Throws<InvalidOperationException>(() => { _ = either.Right; });
            Assert.Equal("Cannot access Right value of a Left Either.", exception.Message);
        }

        [Fact]
        public void ShouldValidateRightImplementation()
        {
            var either = Either<string, Product>.RightValue(new Product
            {
                Id = 1,
                Name = "Product 01",
            });

            Assert.True(either.IsRight);
            Assert.IsType<Product>(either.Right);
            Assert.Equal(1, either.Right.Id);
            Assert.Equal("Product 01", either.Right.Name);

            Assert.False(either.IsLeft);
            var exception = Assert.Throws<InvalidOperationException>(() => { _ = either.Left; });
            Assert.Equal("Cannot access Left value of a Right Either.", exception.Message);
        }
    }
}
