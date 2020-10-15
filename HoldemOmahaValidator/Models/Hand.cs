using System.Collections.Generic;
using System.Linq;
using LookUpService.Models;
using MoreLinq.Extensions;

namespace HoldemOmahaValidator.Models
{
    public class Hand
    {
        public Hand(IList<Card> cards)
        {
            Cards = cards;
        }

        public IList<Card> Cards { get; set; }
        public IList<CardInt> CardsPrimes { get; set; }
        public int Strength { get; set; }
        public override string ToString()
        {
            return Cards.Select(c => c.ToString()).ToDelimitedString("");
        }
    }
}