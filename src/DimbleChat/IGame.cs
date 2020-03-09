using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DimbleChat
{
    public interface IGame
    {
        void SendMessage(string from, string to, string text);
        void SendPublicMessage(IPlayer from, string text);
        IPlayer GameMaster { get; }
        ReadOnlyObservableCollection<IPlayer> AllPlayers { get; }
        ReadOnlyObservableCollection<ChatMessage> AllMessages { get; }
        void NoticeMessagesForUserAsync(IPlayer user, Func<ChatMessage, Task> action);
        IPlayer AddPlayer(IPlayer player);
    }
}
