using System;

namespace DimbleChat
{
    public class Player : IPlayer
    {
        public Player(string displayName, string identifier, bool isGm)
        {
            if (string.IsNullOrWhiteSpace(displayName)) throw new ArgumentNullException(nameof(displayName));
            if (string.IsNullOrWhiteSpace(identifier)) throw new ArgumentNullException(nameof(identifier));

            DisplayName = displayName;
            Identifier = identifier;
            IsGm = isGm;
        }

        public bool IsGm { get; }
        public string DisplayName { get; }
        public string Identifier { get; }
    }
}
