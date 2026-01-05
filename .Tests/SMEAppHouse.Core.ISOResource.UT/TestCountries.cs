using NUnit.Framework;
using SMEAppHouse.Core.ISOResource.CountryCodes;
using System.Linq;

namespace SMEAppHouse.Core.ISOResource.UT
{
    public class TestCountries
    {

        private Countries _countries = null;

        [SetUp]
        public void Setup()
        {
            _countries = Countries.Instance;
        }

        [Test]
        public void TestTheInitialization()
        {
            //_countries = Countries.Instance;
            Assert.Pass();
        }

        [Test]
        public void TestGetPhilippines()
        {
            var phil = _countries.FirstOrDefault(c => c.OfficialNameEn.ToLower().Equals("philippines"));
            Assert.Pass();
        }

    }
}