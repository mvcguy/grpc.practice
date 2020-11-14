using CommonLibrary;
using NUnit.Framework;
using System.Linq;

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
    }
}