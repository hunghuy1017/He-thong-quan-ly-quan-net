using CRUDProductApp;
using Xunit;

namespace CRUDProductApp.Tests
{
    public class ValidationHelperTests
    {
        [Theory]
        [InlineData("valid name")]
        [InlineData("hello world")]
        public void IsValidName_ValidCases(string name)
        {
            Assert.True(ValidationHelper.IsValidName(name));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void IsValidName_InvalidCases(string name)
        {
            Assert.False(ValidationHelper.IsValidName(name));
        }

        [Theory]
        [InlineData("10")]
        [InlineData("0.99")]
        [InlineData("123456")]
        public void IsValidPrice_ValidCases(string price)
        {
            Assert.True(ValidationHelper.IsValidPrice(price));
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("abc")]
        [InlineData("")]
        public void IsValidPrice_InvalidCases(string price)
        {
            Assert.False(ValidationHelper.IsValidPrice(price));
        }
    }
}
