using Noteify.Web.Services;
using Xunit;

namespace Noteify.Test.Unit.Web.Services
{
    public class ErrorServiceTest
    {
        [Fact]
        public void GetErrorMessage_404ErrorCodeAndCustomErrorMessageGiven_ReturnCustomErrorMessage()
        {
            //Given
            var errorCode = 404;
            var customErrorMessage = "Super error message!";

            //When
            var actual = ErrorService.GetErrorMessage(errorCode, customErrorMessage);

            //Then
            Assert.Equal("Super error message!", actual);
        }

        [Fact]
        public void GetErrorMessage_NoErrorCodeAndCustomErrorMessageGiven_ReturnCustomErrorMessage()
        {
            //Given
            var errorCode = 0;
            var customErrorMessage = "Super error message!";

            //When
            var actual = ErrorService.GetErrorMessage(errorCode, customErrorMessage);

            //Then
            Assert.Equal("Super error message!", actual);
        }

        [Fact]
        public void GetErrorMessage_NoErrorCodeAndNoCustomErrorMessageGiven_ReturnEmptyString()
        {
            //Given
            var errorCode = 0;
            var customErrorMessage = "";

            //When
            var actual = ErrorService.GetErrorMessage(errorCode, customErrorMessage);

            //Then
            Assert.Equal("", actual);
        }

        [Fact]
        public void GetErrorMessage_500ErrorCodeAndNoCustomErrorMessageGiven_ReturnSomeErrorMessage()
        {
            //Given
            var errorCode = 500;
            var customErrorMessage = "";

            //When
            var actual = ErrorService.GetErrorMessage(errorCode, customErrorMessage);

            //Then
            Assert.True(actual.Length > 0);
        }
    }
}