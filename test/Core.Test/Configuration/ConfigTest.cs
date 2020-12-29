using Core.Configuration;
using Moq;
using NUnit.Framework;

namespace Core.Test.Configuration
{
    [TestFixture]
    public class ConfigTest
    {
        [SetUp]
        public void SetUp()
        {
            _configDriver = new Mock<IConfigDriver>(MockBehavior.Strict);
            _config = new Config(_configDriver.Object);
        }

        private Config _config;
        private Mock<IConfigDriver> _configDriver;
        private const string VarName = "SomeVar";

        [Test]
        public void GetString_ReturnsValue()
        {
            _configDriver.Setup(c => c.GetValueOrNull(VarName)).Returns("some value");

            var value = _config.GetString(VarName);

            Assert.That(value, Is.EqualTo("some value"));
            _configDriver.Verify(c => c.GetValueOrNull(VarName), Times.Once);
        }

        [Test]
        public void GetString_IfDriversReturnsNull_ThrowsException()
        {
            _configDriver.Setup(c => c.GetValueOrNull(VarName)).Returns(() => null);

            Assert.Throws<ConfigurationKeyNotFoundException>(() => _config.GetString(VarName));
            _configDriver.Verify(c => c.GetValueOrNull(VarName), Times.Once);
        }

        [Test]
        public void GetBool_ReturnsTrue()
        {
            _configDriver.Setup(c => c.GetValueOrNull(VarName)).Returns("true");

            var value = _config.GetBool(VarName);

            Assert.That(value, Is.True);
            _configDriver.Verify(c => c.GetValueOrNull(VarName), Times.Once);
        }

        [Test]
        public void GetBool_ReturnsFalse()
        {
            _configDriver.Setup(c => c.GetValueOrNull(VarName)).Returns("false");

            var value = _config.GetBool(VarName);

            Assert.That(value, Is.False);
            _configDriver.Verify(c => c.GetValueOrNull(VarName), Times.Once);
        }

        [Test]
        public void GetBool_ThrowsException_IfValueIsInvalid()
        {
            _configDriver.Setup(c => c.GetValueOrNull(VarName)).Returns("another string");

            Assert.Throws<BoolConfigurationKeyException>(() => _config.GetBool(VarName));
            _configDriver.Verify(c => c.GetValueOrNull(VarName), Times.Once);
        }
    }
}
