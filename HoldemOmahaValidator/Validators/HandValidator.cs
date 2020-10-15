using FluentValidation;
using HoldemOmahaValidator.Validators;
using HoldemOmahaValidator.Models;

namespace HoldemOmahaValidator.Validators
{
    class HandValidator :AbstractValidator<Hand>
    {
        public HandValidator(int cardsInHand)
        {
            RuleFor(m => m.Cards.Count).Equal(cardsInHand);
            RuleForEach(m => m.Cards).SetValidator(new CardValidator());
        }
    }
}
