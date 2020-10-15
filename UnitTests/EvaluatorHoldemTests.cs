
using HoldemOmahaEval;
using HoldemOmahaEval.Models;
using HoldemOmahaEval.Validators;
using LookUpService;
using Xunit;

namespace ValidatorTests
{
    public class EvaluatorHoldemTests
    {
        private IEvaluator _eval;

        public EvaluatorHoldemTests()
        {

          _eval = new Evaluator(new LookService(), new InputModelValidator(HandType.Holdem));

        }

        [Theory]
        [InlineData("KsAsTsQsJs  5h6h  QcQd 2s3s", "2s3s=QcQd=5h6h")]
        [InlineData("KsAsTsQsQs  5h6h  QcQd 2s3s", "2s3s=QcQd=5h6h")]
        [InlineData("KsAsTsQsQs  5h6 h  QcQ d 2s3s", "2s3s=QcQd=5h6h")]
        [InlineData("Ks  5h6h QcQd  ", "2s3s=QcQd=5h6h")]
        [InlineData("Ks 54312%%$  5h6h QcQd  ", "2s3s=QcQd=5h6h #")]
        public void EvaluatorTests_Should_NotReturnResultOnInvalidString  (string input, string NotExpectedtOutput)
        {
            var result = _eval.Eval(input,HandType.Holdem);
            Assert.NotEqual(NotExpectedtOutput,result);
                
        }

        [Theory]
        [InlineData("AsAd3c AcAh 3h7h", "3h7h AcAh")]
        [InlineData("AsAd3c6s  AcAh  3h7h", "3h7h AcAh")]
        [InlineData("KsAsTsQsJs  5h6h  QcQd 2s3s", "2s3s=5h6h=QcQd")]
        [InlineData("2s3s4h Ac5h 5d6h AhKh", "AhKh Ac5h 5d6h")]
        [InlineData("2s3s4h5c Ac5h 5d6h AhKh", "Ac5h=AhKh 5d6h")]
        [InlineData("2s3s4h5c6d Ac5h 5d6h AhKh", "5d6h=Ac5h=AhKh")]
        [InlineData("2c2hQcQhAd Ac5h AhKh 5d6h ", "5d6h Ac5h AhKh")]
        [InlineData("KdQd4s4d4h Th2s AdAh QhKh  5d5c 2d3d KcAc Tc9c 7h8h 7s8s 9sTs ",
            "7h8h=7s8s=9sTs=Tc9c=Th2s 2d3d 5d5c KcAc=QhKh AdAh")]
        public void EvaluatorTests_Should_ReturnCorrectAnswerOnCorrectInput (string input, string expectedOut)
        {
            var result = _eval.Eval(input,HandType.Holdem);
            Assert.Equal(expectedOut,result);
        }
    }
}
