using System;
using System.Collections.Generic;
using Xunit;

namespace DimbleChat.Tests
{
    public class GameTests
    {
        private static Game TestGame => new Game(new List<IPlayer>(), "abc123", new List<ChatMessage>());

        [Fact]
        public void LetsMakeAGame()
        {
            var game = TestGame;

            Assert.NotNull(game);
        }

        [Fact]
        public void EnsurePlayerIsAdded()
        {
            var game = TestGame;

            game.AddPlayer(new Player("123", "abc", false));

            Assert.Single(game.AllPlayers);
        }

        [Fact]
        public void EnsurePlayerIsAddedOnlyOnce()
        {
            var game = TestGame;

            game.AddPlayer(new Player("123", "abc", false));
            game.AddPlayer(new Player("123", "abc", false));

            Assert.Single(game.AllPlayers);
        }

        [Fact]
        public void EnsureMessageIsNotAddedIfSenderIsNotPlayingGame()
        {
            IGame game = new Game(
                new List<IPlayer>(),
                "abc123",
                new List<ChatMessage>());

            game.AddPlayer(new Player("Sam", "12345", false));

            // this 
            game.SendMessage("123", "12345", "hello world");

            Assert.Empty(game.AllMessages);
        }

        [Fact]
        public void EnsureMessageIsNotAddedIfRecipientIsNotPlayingGame()
        {
            IGame game = new Game(
                new List<IPlayer>(),
                "abc123",
                new List<ChatMessage>());

            game.AddPlayer(new Player("Sam", "12345", false));

            // this 
            game.SendMessage("12345", "345", "hello world");

            Assert.Empty(game.AllMessages);
        }

        [Fact]
        public void EnsureMessageIsAddedIfBothPartiesArePresent()
        {
            IGame game = TestGame;

            game.AddPlayer(new Player("ABC", "abc", false));
            game.AddPlayer(new Player("DEF", "def", false));

            game.SendMessage("abc", "def", "hello world");

            Assert.Single(game.AllMessages);
        }

        [Fact]
        public void SendMessageNullFromThrowsException()
        {
            IGame game = TestGame;

            Exception ex = Assert.Throws<ArgumentNullException>(
                () => { game.SendMessage(null, "123", "123"); });
        }

        [Fact]
        public void SendMessageNullToThrowsException()
        {
            IGame game = TestGame;

            Exception ex = Assert.Throws<ArgumentNullException>(
                () => { game.SendMessage("asd", null, ""); });
        }

        [Fact]
        public void SendMessageNullTextThrowsException()
        {
            IGame game = TestGame;

            Exception ex = Assert.Throws<ArgumentNullException>(
                () => { game.SendMessage("asd", "bar", null); });
        }

        [Fact]
        public void SendPublicMessageNullFromThrowsException()
        {
            IGame game = TestGame;

            Exception ex = Assert.Throws<ArgumentNullException>(
                () => { game.SendPublicMessage(null, "hello world"); });
        }

        [Fact]
        public void FindGameMasterReturnsNullWhenGMHasNotEnteredGame()
        {
            IGame game = TestGame;

            var gm = TestGame.GameMaster;

            Assert.Null(gm);
        }

        [Fact]
        public void FindGameMasterReturnsGameMasterWhenEntered()
        {
            IGame game = TestGame;

            game.AddPlayer(new Player("ThaGameMaster", "abc123", true));

            var gm = game.GameMaster;

            Assert.NotNull(gm);
        }
    }
}
