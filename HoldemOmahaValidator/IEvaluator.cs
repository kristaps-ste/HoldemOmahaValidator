using HoldemOmahaEval.Models;

namespace HoldemOmahaEval
{
    public interface IEvaluator
    {
        string Eval(string input, HandType handType);
    }
}