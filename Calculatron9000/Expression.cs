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
        public Expression(string expStr) => Elements = Arrange(expStr, Opers);

        //Break down the string representation of an expression into a List of operators and numbers in order
        List<object> Arrange(string exp, List<Operator> Opers)
        {
            List<object> elements = new List<object>();

            //clear whitespaces
            exp.Replace(" ", "");

            //isolate adjacent operators with whitespaces
            foreach (string op in Opers.Select(o => o.Notation))
                exp = exp.Replace(op, " " + op + " ");

            //split the expression by the operators to get the numbers
            //seperators are operators with attached whitespaces
            //string[] nums = exp.Split(opDic.Keys.ToArray().Select(o => " " + o + " ").ToArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] nums = exp.Split(Opers.Select(o => o.Notation).Select(o => " " + o + " ").ToArray(), StringSplitOptions.RemoveEmptyEntries);

            string[] expParsed = exp.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string e in expParsed)
            {
                if (nums.Contains(e))
                    elements.Add(Convert.ToDouble(e));
                else
                    elements.Add(Opers.Find(o => o.Notation == e));
            }
            return elements;
        }
        internal double WorkOut()
        {
            //dir: operator search direction (1 for forwards or -1 for backwards)
            //i_op: current operator's index
            //i_s: start index to search for the next operator
            //int dir, i_op, i_s;
            int i_op;

            foreach (Operator op in Opers)
            {
                //    for (int start = op.Backwards ? Elements.Count - 1 : 0; (!op.Backwards && start < Elements.Count) || (op.Backwards && start >= 0); start += op.Backwards ? -1 : 1)
                
                //dir = op.Backwards ? -1 : 1;
                //i_s = (1 - dir) / 2 * (Elements.Count - 1);
                while (true)
                {
                    //Find the index of the next op in Elements
                    //i_op = FindOpIndex(i_s, op);
                    i_op = FindOpIndex(op);
                    //If none left of the current op, break to check for the next op in the operator list
                    if (i_op == -1)
                        break;
                    //Evaluate
                    Eval(i_op);
                    //Move the starting index for the search to the position of the last evaluation
                    //Number of removed operands that were behind the operator in terms of search direction are taken in the account
                    //i_s = i_op - dir * (op.RelOpdInd.Count(i => i * dir < 0) - 1);
                }
            }

            return (double)Elements[0];
        }
        //private int FindOpIndex(int i_s, Operator op)
        private int FindOpIndex(Operator op)
        {
            //return op.Backwards ? Elements.FindLastIndex(i_s, e => e as Operator == op) : Elements.FindIndex(i_s, e => e as Operator == op);
            return op.Backwards ? Elements.FindLastIndex(e => e as Operator == op) : Elements.FindIndex(e => e as Operator == op);
        }
        private void Eval(int i)
        {
            if (i == -1)
            {
                //Message
                return;
            }

            //Get the operator at index i
            Operator op = (Operator)Elements[i];

            //Find the operands using RelOpdInd which specifies their location relative to the operator.
            //If the index difference is included in RelOpdInd, the element at that index is an operand.
            //Cast found objects into double and turn the resultant IEnumerable into an array.
            //Pass that array of doubles to the operator's function.
            //Replace the operator with the returned value in the expression.
            Elements[i] = op.Function(Elements.Where((num, numInd) => op.RelOpdInd.Contains(numInd - i)).Select(num => (double)num).ToArray());

            //Remove operands from the expression
            foreach (int relInd in op.RelOpdInd)
                Elements[i + relInd] = "empty";
            Elements.RemoveAll(e => e == (object)"empty");
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
        static readonly List<Operator> Opers = new List<Operator>() {
            new Operator("sqrt", 0, false, o => Sqrt(o[0]), 1),
            new Operator("log", 0, false, o => Log(o[0],10), 1),
            new Operator("ln", 0, false, o => Log(o[0]),  1),
            new Operator("^", 1, true, o => Pow(o[0],o[1]), -1, 1),
            new Operator("*", 2, false, o => o[0] * o[1], -1, 1),
            new Operator("/", 2, false,  o => o[0] / o[1], -1, 1),
            new Operator("+", 3, false,  o => o[0] + o[1], -1, 1),
            new Operator("-", 3, false,  o => o[0] - o[1], -1, 1)
        };
    }
}
