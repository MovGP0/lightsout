using NUnit.Framework;

namespace LightsOut.Tests
{
    [TestFixture]
    public sealed class LevelsLoaderTest
    {
        [Test]
        public void MustDownloadJson()
        {
            var json = LevelsLoader.GetLevels();
            Assert.That(json, Is.Not.Empty);
        }
    }
}
