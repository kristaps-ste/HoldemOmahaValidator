using System.Linq;
using FluentValidation;
using HoldemOmahaEval.Models;
using HoldemOmahaValidator.Models;
using HoldemOmahaValidator.Validators;
using MoreLinq.Extensions;

namespace HoldemOmahaEval.Validators
{
    public class InputModelValidator : AbstractValidator<TestCaseModel>
    {
        public InputModelValidator(HandType handType)
        {
            int cardsInhand =
                handType == HandType.Holdem ? 2 : handType == HandType.Omaha ? 4 : 0;

            CascadeMode = CascadeMode.Stop;
            // board check
            RuleFor(m => m.Board.Cards.Count).NotNull().InclusiveBetween(3, 5).WithMessage("Board card count 3 -5");

            RuleForEach(m => m.Board.Cards).SetValidator(new CardValidator());

            RuleFor(m => m.HandsList.Count).NotNull().GreaterThan(0).WithMessage("at least one  hand");
            RuleForEach(m => m.HandsList).NotNull().SetValidator(new HandValidator(cardsInhand));
            RuleFor(m => m).Must(NotContainDuplicates).WithMessage("duplicates in testcase");
        }
        private bool NotContainDuplicates(TestCaseModel m)
        {
            var board = m.Board.Cards.Flatten().ToList();
            var pockets = m.HandsList.Select(h => h.Cards.Flatten()).Flatten().ToList();

            board.AddRange(pockets);

            return board.ToHashSet().Count == board.Count;
        }
    }
}