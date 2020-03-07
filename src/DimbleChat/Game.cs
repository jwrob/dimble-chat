using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DimbleChat
{
    public interface IGame
    {
        void SendMessage(string from, string to, string text);
        IPlayer GameMaster { get; }
        ReadOnlyObservableCollection<ChatMessage> AllMessages { get; }
        void NoticeMessagesForUserAsync(string userIdentifier, Func<ChatMessage, Task> action);
        void AddPlayer(IPlayer player);
    }

    public interface IPlayer
    {
        public bool IsGm { get; }
        public string DisplayName { get; }
        public string Identifier { get; }
    }

    public class Game : IGame
    {
        private readonly string GmIdentifier;
        private ObservableCollection<IPlayer> Players { get; }
        private ObservableCollection<ChatMessage> Messages { get; }

        public ReadOnlyObservableCollection<ChatMessage> AllMessages
            => new ReadOnlyObservableCollection<ChatMessage>(Messages);

        public ReadOnlyObservableCollection<IPlayer> AllPlayers
            => new ReadOnlyObservableCollection<IPlayer>(Players);

        public Game(List<IPlayer> players, string gmIdentifier, List<ChatMessage> messages)
        {
            GmIdentifier = gmIdentifier;
            Players = new ObservableCollection<IPlayer>(players);
            Messages = new ObservableCollection<ChatMessage>(messages);
        }

        public IPlayer GameMaster => Players.FirstOrDefault(player => player.IsGm);

        public void NoticeMessagesForUserAsync(string userIdentifier, Func<ChatMessage, Task> action)
        {
            Messages.CollectionChanged += async (sender, args) =>
            {
                if (args.NewItems == null) return;
                foreach (var message in args.NewItems)
                {
                    if (!(message is ChatMessage chatMessage)) return;

                    await action(chatMessage);
                }
            };
            throw new NotImplementedException();
        }

        public void AddPlayer(IPlayer player)
        {
            if (Players.Any(p => p.Identifier == player.Identifier)) return;

            Players.Add(player);
        }

        public void SendMessage(string from, string to, string text)
        {
            // sender or recipient is not playing
            if (Players.All(p => p.Identifier != @from) || Players.All(p => p.Identifier != to))
                return;
            
            Messages.Add(new ChatMessage(DateTimeOffset.Now, from, to, text));
        }
    }

    public class Chat { }

    public class ChatMessage
    {
        internal const string PublicChannelName = "public-chat";

        public ChatMessage(DateTimeOffset timestamp, string from, string to, string text)
        {
            Timestamp = timestamp;
            From = @from;
            To = to;
            Text = text;
        }

        public DateTimeOffset Timestamp { get; }
        public string From { get; }
        public string To { get; }
        public string Text { get; }
    }

    public class Player : IPlayer
    {
        public Player(string displayName, string identifier, bool isGm)
        {
            DisplayName = displayName;
            Identifier = identifier;
            IsGm = isGm;
        }

        public bool IsGm { get; }
        public string DisplayName { get; }
        public string Identifier { get; }
    }
}
