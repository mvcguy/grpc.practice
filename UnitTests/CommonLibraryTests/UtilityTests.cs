using CommonLibrary;
using NUnit.Framework;

namespace UnitTests.CommonLibraryTests
{
    public class UtilityTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test(Description ="Verify that the method returns a string of given length", TestOf =typeof(Utilities))]
        
        public void RandomString01()
        {
            var result = Utilities.RandomString(8);
            Assert.AreEqual(7, result.Length);
        }
    }
}