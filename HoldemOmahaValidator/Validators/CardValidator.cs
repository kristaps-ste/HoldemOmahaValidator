using FluentValidation;
using LookUpService.Models;

namespace HoldemOmahaValidator.Validators
{
    public class CardValidator : AbstractValidator<Card>
    {
        private static  string _suits = "chds";
        private static  string _ranks = "23456789TJQKA";
        public CardValidator()
        {
            RuleFor(c => c.Rank).Must(c => _ranks.Contains(c.ToString())).WithMessage("Invalid rank");
            RuleFor(c => c.Suit).Must(c => _suits.Contains(c.ToString())).WithMessage("Invalid suit");
        }
    }
}