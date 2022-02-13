using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Calculatron9000
{
    class Expression
    {
        public List<object> Elements { get; set; }
        public Expression(string expStr) => Elements = Arrange(expStr, Operators);

        //Break down the string representation of an expression into a List of operators and numbers in order
        List<object> Arrange(string exp, List<Operator> Operators)
        {
            List<object> elements = new List<object>();

            //clear whitespaces
            exp = exp.Replace(" ", "");

            //isolate adjacent operators with whitespaces
            foreach (string op in Operators.Select(o => o.Notation))
                exp = exp.Replace(op, " " + op + " ");

            //split the expression by the operators to get the numbers
            //seperators are operators with attached whitespaces
            //string[] nums = exp.Split(opDic.Keys.ToArray().Select(o => " " + o + " ").ToArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] nums = exp.Split(Operators.Select(o => o.Notation).Select(o => " " + o + " ").ToArray(), StringSplitOptions.RemoveEmptyEntries);

            string[] expParsed = exp.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string e in expParsed)
            {
                if (nums.Contains(e))
                    elements.Add(Convert.ToDouble(e));
                else
                    elements.Add(Operators.Find(o => o.Notation == e));
            }
            return elements;
        }
        internal double WorkOut()
        {
            //precedence index of the operator with lowest precedence
            int p_max = Operators.Select(o => o.Precedence).Max();
            bool b; //Whether or not the operators with the current precedence are to be searched backwards
            //Seek and do the operations based on their precedence from 0 to p_max
            for (int p = 0; p <= p_max; p++)
            {
                //Get the search direction of the first operator with current precedence because
                //operators with the same precedence have the same search direction
                b = Operators.Where(e => e.Precedence == p).First().Backwards;
                //Evaluate all the operations with the current precedence
                //When the expression runs out of such operations, FindOpIndex() returns -1,
                //which makes Eval() return false and terminate the while loop. 
                while (Eval(FindOpIndex(p, b))) { }
            }
            return (double)Elements[0];
        }
        private int FindOpIndex(int p, bool backwards)
        {
            //Get the index of the operator with the specified precedence
            if (backwards)
                return Elements.FindLastIndex(e => (e is Operator) && ((Operator)e).Precedence == p);
            return Elements.FindIndex(e => (e is Operator) && ((Operator)e).Precedence == p);
        }
        private bool Eval(int i)
        {
            if (i == -1) return false;

            //Get the operator at index i
            Operator op = (Operator)Elements[i];

            //Find the operands using RelOpdInd which specifies their location relative to the operator.
            //If the index difference is included in RelOpdInd, the element at that index is an operand.
            //Cast found objects into double and turn the resultant IEnumerable into an array.
            //Pass that array of doubles to the operator's function.
            //Replace the operator with the returned value in the expression.
            Elements[i] = op.Function(Elements.Where((num, numInd) => op.RelOpdInd.Contains(numInd - i)).Select(num => (double)num).ToArray());

            //Override the operands and remove them
            foreach (int relInd in op.RelOpdInd)
                Elements[i + relInd] = null;
            Elements.RemoveAll(e => e is null);

            return true;
        }
        class Operator
        {
            public Operator(string notation, int precedence, bool backwards, Func<double[], double> func, params int[] relOpdInd)
            {
                Notation = notation;
                Precedence = precedence;
                Backwards = backwards;
                RelOpdInd = relOpdInd;
                Function = func;
                Calc = null;
            }
            public string Notation { get; }
            public int Precedence { get; }

            //When true, the operators of this kind will be searched and evaluated from right to left
            public bool Backwards { get; }
            //Operand indexes relative to the operator's.
            public int[] RelOpdInd { get; }
            public Func<double[], double> Function { get; }
            public Func<Expression, double> Calc { get; }
            public override string ToString() => Notation;
        }
        static readonly List<Operator> Operators = new List<Operator>() {
            new Operator("sqrt", 0, true, o => Sqrt(o[0]), 1),
            new Operator("log", 0, true, o => Log(o[0],10), 1),
            new Operator("ln", 0, true, o => Log(o[0]),  1),
            new Operator("^", 1, true, o => Pow(o[0],o[1]), -1, 1),
            new Operator("*", 2, false, o => o[0] * o[1], -1, 1),
            new Operator("/", 2, false,  o => o[0] / o[1], -1, 1),
            new Operator("+", 3, false,  o => o[0] + o[1], -1, 1),
            new Operator("-", 3, false,  o => o[0] - o[1], -1, 1)
        };
    }
}
