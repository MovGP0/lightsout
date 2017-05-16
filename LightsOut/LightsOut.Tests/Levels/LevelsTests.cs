using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LightsOut.Tests
{
    [TestFixture]
    public class LevelsTests
    {
        private static TestDataBuilder A { get; }

        [Test]
        public void ShouldConvertFromJson()
        {
            var expected = A.Level()
                .WithName("Level 0")
                .WithColumns(4)
                .WithRows(4)
                .WithOn(new[] { 0, 5, 10, 15 });

            var json = JsonObjectMother.Level0Json;

            var level = JsonConvert.DeserializeObject<Level>(json);
            Assert.That(level, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldConvertListFromJson()
        {
            var json = JsonObjectMother.AllLevelsJson;
            var levels = JsonConvert.DeserializeObject<List<Level>>(json);
            Assert.That(levels.Count, Is.EqualTo(3));
        }
    }
}