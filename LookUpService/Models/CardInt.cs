namespace LookUpService.Models
{
    public struct CardInt
    {
        public CardInt(int rank, int suit)
        {
            Rank = rank;
            Suit = suit;
        }
        public int Rank { get; set; }
        public int Suit { get; set; }
    }
}