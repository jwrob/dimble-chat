using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DimbleChat.Tests
{
    public class GameEventTests
    {
        private readonly ITestOutputHelper TestOutputHelper;

        public GameEventTests(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        [Fact]
        public void NoobEvent()
        {
            IGame game = new Game(
                new List<IPlayer>(),
                "abc123",
                new List<ChatMessage>());

            var counter = 0;
            var message = "";
            game.NoticeMessagesForUserAsync("abc", Handler);

            game.NoticeMessagesForUserAsync("def", Handler);


            game.AddPlayer(new Player("bob", "abc", false));
            game.AddPlayer(new Player("gru", "def", false));
            game.SendMessage("abc", "def", "hello world");

            Assert.Equal(2, counter);
            Assert.Equal("hello worldhello world", message);

            async Task Handler(ChatMessage m)
            {
                message += m.Text;
                counter++;
                TestOutputHelper.WriteLine($"to:{m.To} from:{m.From} Text:{m.Text}");
            }
        }
    }
}
