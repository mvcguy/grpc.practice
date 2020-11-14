using CommonLibrary;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace UnitTests.CommonLibraryTests
{
    public class UtilityTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test(Description = "Verify that the method returns a string of given length", TestOf = typeof(Utilities))]

        public void RandomString01()
        {
            var result = Utilities.RandomString(8);
            Assert.AreEqual(8, result.Length);
        }

        [Test(Description = "Verify that method returns only english alphabets", TestOf = typeof(Utilities))]
        public void RandomString02()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = Utilities.RandomString();

                Assert.IsTrue(!result.Any(x => x < 97 || x >= 122),result);
            }
        }

        [Test(Description ="Generate RSA key for samples")]
        public void GenerateRsaKey()
        {
            var key = FakeKeyStore.key as RsaSecurityKey;
                        
            var value = key.Rsa.Encrypt(System.Text.Encoding.UTF8.GetBytes("Shahid-Ali"), RSAEncryptionPadding.Pkcs1);
            Assert.AreEqual("exYUH1oG6+hrFQXWZoxBM+531CiZc6Tohe91znA7br9WIN6hyBpKO2G5o23oztVjuA7Z8q1N2+lFZVP3UhuruaHQkxUP2SB3S1G9Os9/gJLLcF9ut9Kr/Y+QwLDA91EM4MqhJ8zELRWyJW8tp9DeHJDFHe/Xbsc4Fr6/xT3icjw="
                , System.Convert.ToBase64String(value));
        }
    }
}