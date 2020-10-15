namespace LookUpService.Models

{
    public struct Card
    {
       public Card(char rank, char suit)
        {
            Rank = rank;
            Suit = suit;
            _strRepresentation=Rank + Suit.ToString();
        }
       private readonly string _strRepresentation;
        public char Rank { get; }
        public char Suit { get; }
       
        public override string ToString()
        {
            return _strRepresentation;
        }
    }
}