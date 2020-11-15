using CommonLibrary;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
            var key = FakeKeyStore.Key as RsaSecurityKey;
                        
            var value = key.Rsa.Encrypt(System.Text.Encoding.UTF8.GetBytes("Shahid-Ali"), RSAEncryptionPadding.Pkcs1);
            var decrypt = Encoding.UTF8.GetString(key.Rsa.Decrypt(value, RSAEncryptionPadding.Pkcs1));
            Assert.AreEqual("Shahid-Ali", decrypt);

        }
    }
}