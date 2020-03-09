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
        public void EventingWorks()
        {
            IGame game = new Game(
                new List<IPlayer>(),
                "abc123",
                new List<ChatMessage>());

            var counter = 0;
            var message = "";


            var bob = game.AddPlayer(new Player("bob", "abc", false));
            var gru = game.AddPlayer(new Player("gru", "def", false));

            game.NoticeMessagesForUserAsync(bob, Handler);
            game.NoticeMessagesForUserAsync(gru, Handler);

            game.SendMessage("abc", "def", "hello world");

            Assert.Equal(2, counter);
            Assert.Equal("hello worldhello world", message);

            Task Handler(ChatMessage m)
            {
                message += m.Text;
                counter++;
                TestOutputHelper.WriteLine($"{m}");
                return Task.CompletedTask;
            }
        }

        [Fact]
        public void PublicChatMessagesGoToEveryone()
        {
            IGame game = new Game(
                new List<IPlayer>(),
                "gm-here",
                new List<ChatMessage>());

            var gm = game.AddPlayer(new Player("GameMaster!", "gm-here", true));
            var p1 = game.AddPlayer(new Player("bob", "1234", false));
            var p2 = game.AddPlayer(new Player("harry", "9876", false));
            var p3 = game.AddPlayer(new Player("tom", "7878", false));

            Assert.Equal(4, game.AllPlayers.Count);

            var gmCounter = 0;
            var p1Counter = 0;
            var p2Counter = 0;
            var p3Counter = 0;

            game.NoticeMessagesForUserAsync(gm, GmHandler);
            game.NoticeMessagesForUserAsync(p1, P1Handler);
            game.NoticeMessagesForUserAsync(p2, P2Handler);
            game.NoticeMessagesForUserAsync(p3, P3Handler);

            game.SendPublicMessage(p2, "hello everyone");
            game.SendMessage(p1.Identifier, p2.Identifier, "you scoundrel");

            Assert.Equal(2, gmCounter);
            Assert.Equal(2, p1Counter);
            Assert.Equal(2, p2Counter);
            Assert.Equal(1, p3Counter);

            Task GmHandler(ChatMessage m)
            {
                gmCounter++;
                TestOutputHelper.WriteLine($"gm {m}");
                return Task.CompletedTask;
            }

            Task P1Handler(ChatMessage m)
            {
                p1Counter++;
                TestOutputHelper.WriteLine($"p1 {m}");
                return Task.CompletedTask;
            }

            Task P2Handler(ChatMessage m)
            {
                p2Counter++;
                TestOutputHelper.WriteLine($"p2 {m}");
                return Task.CompletedTask;
            }

            Task P3Handler(ChatMessage m)
            {
                p3Counter++;
                TestOutputHelper.WriteLine($"p2 {m}");
                return Task.CompletedTask;
            }
        }
    }
}
