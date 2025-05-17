// (c) 2025 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using SharpLogShield.Logging;

namespace SharpLogShield.Test
{
    public class LogMaskerTests
    {
        [Theory]
        [InlineData("My email is john.doe@example.com", "***@example.com")]
        [InlineData("Credit card: 4111 1111 1111 1234", "**** **** **** 1234")]
        [InlineData("Codice fiscale: RSSMRA85M01H501U", "RSS*********501U")]
        [InlineData("Phone: +39 333-123-4567", "333-***-**67")]
        [InlineData("Address: Via Roma, Milano", "Via ****, Milano")]
        public void MaskSensitiveData_ShouldMaskKnownPatterns(string input, string expectedMaskedPart)
        {
            // Act
            string result = LogMasker.MaskSensitiveData(input);

            // Assert
            Assert.Contains(expectedMaskedPart, result);
        }

        [Fact]
        public void MaskSensitiveData_ShouldReturnSameMessage_IfNoSensitiveData()
        {
            // Arrange
            string input = "This is a safe log message without any sensitive data.";

            // Act
            string result = LogMasker.MaskSensitiveData(input);

            // Assert
            Assert.Equal(input, result);
        }

        [Fact]
        public void MaskSensitiveData_ShouldReturnNullOrEmpty_WhenInputIsNullOrEmpty()
        {
            // Act & Assert
            Assert.Null(LogMasker.MaskSensitiveData(null));
            Assert.Equal(string.Empty, LogMasker.MaskSensitiveData(string.Empty));
        }
    }
}