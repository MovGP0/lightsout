using NUnit.Framework;

namespace LightsOut.Tests
{
    [TestFixture]
    public sealed class LevelsLoaderTest
    {
        private static TestDataBuilder An { get; }

        [Test]
        public void MustDownloadJson()
        {
            var loader = An.LevelsLoader();
            var json = loader.GetLevels();
            Assert.That(json, Is.Not.Empty);
        }
    }
}
