using System.Linq;
using HoldemOmahaEval.Models;
using HoldemOmahaEval.Validators;
using HoldemOmahaValidator;
using HoldemOmahaValidator.Models;
using LookUpService;
using MoreLinq.Extensions;

namespace HoldemOmahaEval
{
    public class Evaluator : IEvaluator
    {
        private readonly LookService _look;
        private readonly InputModelValidator _inputValidator;

        public Evaluator(LookService look, InputModelValidator inputValidator)
        {
            _look = look;
            _inputValidator = inputValidator;
        }

        public string Eval(string input, HandType handType)
        {

            TestCaseModel template = handType == HandType.Holdem
                ? new TestCaseModel(_look, LookService.HoldemCombinator)
                : new TestCaseModel(_look, LookService.OmahaCombinator);

            var testCase = InputToModelConverter.Convert(input, template);
            var validationResult = _inputValidator.Validate(testCase);

            if (!validationResult.IsValid)
            {
                return validationResult.Errors.ToDelimitedString(";");
            }

            testCase.Evaluate();

            var grouped =
                testCase.HandsList
                    .Select(h => new { name = h.ToString(), rank = h.Strength })
                    .OrderBy(h => h.rank)
                    .Reverse()
                    .GroupBy(h => h.rank);

            var output =
                grouped.Select(it =>
                        it.OrderBy(h => h.name)
                            .Select(h => h.name)
                            .ToDelimitedString("="))
                    .ToDelimitedString(" ");

            return output;
        }
    }
}