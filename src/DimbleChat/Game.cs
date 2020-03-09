using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DimbleChat
{
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

        public void NoticeMessagesForUserAsync(IPlayer user, Func<ChatMessage, Task> action)
        {
            var userIdentifier = user.Identifier;
            Messages.CollectionChanged += async (sender, args) =>
            {
                if (args.NewItems == null) return;
                foreach (var message in args.NewItems)
                {
                    if (!(message is ChatMessage chatMessage)) return;

                    // the message is not for this user
                    if ((chatMessage.From != userIdentifier && chatMessage.To != userIdentifier)
                        // and the message is not to the public channel
                        && (chatMessage.To != ChatMessage.PublicChannelName)
                        // and the user is not the gm (they could possibly see everything)
                        && (userIdentifier != GmIdentifier)) return;

                    await action(chatMessage);
                }
            };
        }

        public IPlayer AddPlayer(IPlayer player)
        {
            if (Players.Any(p => p.Identifier == player.Identifier)) return null;

            Players.Add(player);

            return player;
        }

        public void SendMessage(string from, string to, string text)
        {
            if (string.IsNullOrWhiteSpace(from)) throw new ArgumentNullException(nameof(from));
            if (string.IsNullOrWhiteSpace(to)) throw new ArgumentNullException(nameof(to));
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));

            // sender or recipient is not playing
            if (Players.All(p => p.Identifier != @from) || Players.All(p => p.Identifier != to))
                return;

            Messages.Add(new ChatMessage(DateTimeOffset.Now, from, to, text));
        }

        public void SendPublicMessage(IPlayer @from, string text)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));

            if (Players.All(p => p.Identifier != from.Identifier)) return;

            Messages.Add(new ChatMessage(DateTimeOffset.Now, from.Identifier, ChatMessage.PublicChannelName, text));
        }
    }
}
