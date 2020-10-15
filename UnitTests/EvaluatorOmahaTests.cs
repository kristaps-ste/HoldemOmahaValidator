using HoldemOmahaEval;
using HoldemOmahaEval.Models;
using HoldemOmahaEval.Validators;
using LookUpService;
using Xunit;

namespace ValidatorTests
{
    public class EvaluatorOmahaTests
    {
        private readonly IEvaluator _eval;
        public EvaluatorOmahaTests()
        {

            _eval = new Evaluator(new LookService(), new InputModelValidator(HandType.Omaha));
        }
        [Theory]
        [InlineData("2hQcQh6d9d Ac5h3c9h 5dAd2c7c", "5dAd2c7c Ac5h3c9h")]
        [InlineData("2hQcQh Ac5h3c9h 5dAd2c7c", "Ac5h3c9h 5dAd2c7c")]
        [InlineData("2hQcKhQh Ac5h3c9h 5dAd2c7c", "5dAd2c7c Ac5h3c9h")]
        public void EvaluatorTests_ShouldReturnExpectedValue  (string input, string expectedOut)
        {
            var result = _eval.Eval(input,HandType.Omaha);
            Assert.Equal(expectedOut,result);
        }

        [Theory]
        [InlineData("2hQcQcch6d9d d Ac5h3c 5 dAdd2c",  "5dAd2c7c Ac5h3c9h")]
        [InlineData("2hQcQh6d9d Ac5h3c 5dAd2c", "5dAd2c7c Ac5h3c9h")]
        [InlineData("2hQcQh6d9d Ac5h3c Ac5h3c Ac5h3c 5dAd2c", "5dAd2c7c Ac5h3c9h")]
        [InlineData("2hQcQh 6d 9d Ac5h3 #$^& 76 c 45dAd2c 65", "5dAd2c7c Ac5h3c9h")]

        public void EvaluatorTests_ShouldNotReturnExpectedValue (string input, string expectedOut)
        {
            var result = _eval.Eval(input,HandType.Omaha);
            Assert.NotEqual(expectedOut,result);
        }
    }
}