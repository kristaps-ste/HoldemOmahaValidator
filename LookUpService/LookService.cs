using System;
using System.Collections.Generic;
using System.Linq;
using LookUpService.Models;
using MoreLinq.Extensions;
using Sequences;

namespace LookUpService
{
    public class LookService : ILookService
    {
        public static int SuitPrime = 43;
        public static int[] PrimeList = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41 };
        public static string Faces = "23456789TJQKA";
        public static string Suits = "chds";
        public static int[] SuitList = { 1, 2, 4, 8 };
        public static ISequence<int> Ranks = Sequence.With(PrimeList);
        private readonly Dictionary<int, int> _table;

        public LookService()
        {
            int key = 1;
            //lets generate lookup table
            List<int> tmp = new List<int>();

            tmp.AddRange(Straights(true));
            tmp.AddRange(FourOfKind());
            tmp.AddRange(FullHouse());
            tmp.AddRange(NoPair(true)); // flush
            tmp.AddRange(Straights(false));
            tmp.AddRange(ThreeOfKind());
            tmp.AddRange(TwoPair());
            tmp.AddRange(OnePair());
            tmp.AddRange(NoPair(false));
            _table = tmp.ToDictionary(v => v, _ => key++);
        }
        public int GetRank(int key)
        {
            bool sucess = _table.TryGetValue(key, out var rank);

            return sucess ? rank : _table.Count + 1;
        }
        static IEnumerable<int> OnePair()
        {
            var kickers = PrimeList.Subsets(3);
            return PrimeList.Cartesian(kickers, (pair, kick) => new { pair, kick })
                .Where(it => it.kick.Contains(it.pair) == false)
                .Select(it => Power(it.pair, 2) * it.kick.Aggregate(Product))
                .Reverse();
        }
        static IEnumerable<int> TwoPair()
        {
            var pairs = PrimeList.Subsets(2).ToList();
            return pairs.Cartesian(PrimeList, (a, b) => new { a, b })
                .Where(it => it.a.Contains(it.b) == false)
                .Select(it => it.b * Power(it.a[0], 2) * Power(it.a[1], 2))
                .Reverse();
        }
        static IEnumerable<int> ThreeOfKind()
        {
            var kickers = PrimeList.Subsets(2).ToList();
            return PrimeList.Cartesian(kickers, (a, b) => new { a, b })
                 .Where(it => it.b.Contains(it.a) == false)
                 .Select(it => Power(it.a, 3) * it.b.Aggregate(Product))
                 .Reverse();
        }
        static IEnumerable<int> NoPair(bool suited)
        {

            var result = PrimeList.Subsets(5)
                .Select(it => it.Aggregate(Product))
                .Except(Straights(false)).Reverse();

            if (suited)
            {
                result = result.Select(AsSuited);
            }

            return result;
        }
        static IEnumerable<int> FullHouse()
        {
            return PrimeList.Cartesian(PrimeList, (three, two) => new { three, two })
                .Where(it => it.three != it.two)
                .Select(it => FactorFullHouse(it.three, it.two))
                .Reverse();
        }
        static IEnumerable<int> FourOfKind()
        {

            return PrimeList.Cartesian(PrimeList, (four, kicker) => new { four, kicker })
                .Where(it => it.four != it.kicker)
                .Select(it => FactorFourKind(it.four, it.kicker))
                .Reverse().ToList();
        }
        static IEnumerable<int> Straights(bool suited)
        {
            var res = Ranks.Sliding(5, 1).Select(seq => seq.ToList()
                    .Aggregate(Product))
                .Reverse().ToList();

            //Add Low end Straight
            res.Add(PrimeList.Last() * PrimeList.Slice(0, 4).Aggregate(Product));

            // add suited factor if required
            if (suited)
            {
                res = res.Select(AsSuited).ToList();
            }

            return res;
        }
        public static IList<CardInt> CharsToPrimes(IList<Card> cards)
        {
            return cards.Select(c =>
                   new CardInt(PrimeList[Faces.IndexOf(c.Rank)], SuitList[Suits.IndexOf(c.Suit)])).
                     ToList();
        }

        public static IList<int> HoldemCombinator(IList<CardInt> board, IList<CardInt> pockets)
        {
            var ranksTotal = board.Concat(pockets)
                .Subsets(5)
                .Select(hand => hand.Aggregate(ProductCards));
            return ranksTotal.Select(MultiplyIfSuited).ToList();
        }

        public static IList<int> OmahaCombinator(IList<CardInt> board, IList<CardInt> pockets)
        {
            var pocketVariants = pockets.Subsets(2);
            var boardVariants = board.Subsets(3);
            var ranksTotal = boardVariants.Cartesian(pocketVariants, (p, b) => p.Concat(b))
                .Select(hand => hand.Aggregate(ProductCards));
            return ranksTotal.Select(MultiplyIfSuited).ToList();
        }

        private static int MultiplyIfSuited(CardInt card)
        {
            return card.Suit > 0 ? card.Rank * SuitPrime : card.Rank;
        }
        private static CardInt ProductCards(CardInt acc, CardInt next)
        {
            acc.Rank *= next.Rank;
            acc.Suit &= next.Suit;
            return acc;
        }
        private static int AsSuited(int unsuited)
        {
            return unsuited * SuitPrime;
        }
        private static int Product(int acc, int next)
        {
            return acc * next;
        }
        private static int Power(int basE, int pow, int total = 1)
        {
            return (int)Math.Pow(basE, pow);
        }
        private static Func<int, int, int> FactorFourKind = (a, b) => Power(a, 4) * b;
        private static Func<int, int, int> FactorFullHouse = (a, b) => Power(a, 3) * Power(b, 2);
    }
}
