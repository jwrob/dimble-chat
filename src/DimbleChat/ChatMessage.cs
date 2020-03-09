using System;

namespace DimbleChat
{
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

        public override string ToString()
        {
            return $"from:{From} | to:{To} | text:{Text} | timestamp:{Timestamp}";
        }
    }
}
