using System;
using Xunit;

namespace DimbleChat.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void DisplayNameDisplays()
        {
            var player = new Player("bob", "123", false);

            Assert.Equal("bob", player.DisplayName);
            Assert.False(player.IsGm);
            Assert.Equal("123", player.Identifier);
        }

        [Fact]
        public void DisplayNameIsRequired()
        {
            Assert.Throws<ArgumentNullException>(() => new Player(null, null, false));
        }

        [Fact]
        public void IdentifierIsRequired()
        {
            Assert.Throws<ArgumentNullException>(() => new Player("bob-your-uncle", null, false));
        }
    }
}
