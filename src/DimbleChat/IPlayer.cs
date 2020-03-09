namespace DimbleChat
{
    public interface IPlayer
    {
        public bool IsGm { get; }
        public string DisplayName { get; }
        public string Identifier { get; }
    }
}
