using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HoldemOmahaValidator.Models;
using LookUpService.Models;

namespace HoldemOmahaValidator
{
    public static  class InputToModelConverter
    {

        public static TestCaseModel Convert(string input, TestCaseModel testCase)
        {
            var toCards=input.Split(new [] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(it=> new Hand(StrToCards(it))).ToList();
            
            testCase.Board = toCards.Count>0? toCards[0]: testCase.Board;
            testCase.HandsList = toCards.Skip(1).ToList();
            return testCase;
        }
        private static IList<Card> StrToCards(string str)
        {
            return str.Select((c, index) => new {c, index})
                .GroupBy(g => g.index / 2, i => i.c)
                .Select(it => it.ToList()).ToList()
                .Select(it=> it.Count ==2 ? new Card(it[0],it[1]) : new Card(' ',' '))
                .ToList();
        }
    }
}