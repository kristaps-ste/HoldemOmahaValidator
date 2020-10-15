using System;
using System.Collections.Generic;
using System.Linq;
using LookUpService.Models;

namespace HoldemOmahaValidator.Models
{
    public class TestCaseModel
    {
        private LookUpService.LookService _look;
        public Hand Board { get; set; }
        public IList<Hand> HandsList = new List<Hand>();
       
        private Func<IList<CardInt>, IList<CardInt>, IList<int>> _combinationProvider;
        public TestCaseModel(LookUpService.LookService look,
            Func<IList<CardInt>, IList<CardInt>, IList<int>> combinationProvider )
        {
            _look = look;
            _combinationProvider = combinationProvider;
        }
        public void Evaluate()
        {
            TranslateToPrimes();
           HandsList= HandsList.Select(h =>
            {
                 h.Strength=GetHandStrength(Board, h, _combinationProvider);
                return h;
            }).ToList();

        }

        private int GetHandStrength(Hand board, Hand pocket, Func<IList<CardInt>, IList<CardInt>, IList<int>> combinationProvider)
        {
            return combinationProvider(board.CardsPrimes, pocket.CardsPrimes)
                .Select(it => _look.GetRank(it)).Min();
        }
        
        private void TranslateToPrimes()
        {
            Board.CardsPrimes = LookUpService.LookService.CharsToPrimes(Board.Cards);
            HandsList =
                HandsList.Select(h => { h.CardsPrimes = LookUpService.LookService.CharsToPrimes(h.Cards);
                    return h;
                }).ToList();
        }
    }
}