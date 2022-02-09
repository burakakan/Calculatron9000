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
        static readonly List<Operator> Opers = new List<Operator>() {
            new Operator("sqrt", 0, false, o => Sqrt(o[0]), 1),
            new Operator("log", 0, false, o => Log(o[0],10), 1),
            new Operator("ln", 0, false, o => Log(o[0]),  1),
            new Operator("^", 1, true, o => Pow(o[0],o[1]), -1, 1),
            new Operator("*", 2, false, o => o[0] * o[1], -1, 1),
            new Operator("/", 2, false,  o => o[0] / o[1], -1, 1),
            new Operator("+", 3, false,  o => o[0] + o[1], -1, 1),
            new Operator("-", 3, false,  o => o[0] - o[1], -1, 1),
        };

        public Expression(string expStr)
        {
            Elements = Arrange(expStr, Opers);

        }

        public List<object> Elements { get; set; }

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

        //internal double WorkOut()
        //{
        //    int i;

        //    //Get the last turn by dividing the underlying value of the least precedent operator by 20
        //    int lastTurn = Enum.GetValues(typeof(Ops)).Cast<int>().Max() / 20;

        //    for (int turn = 0; turn < lastTurn; turn++)
        //    {

        //        i = 0;
        //        while (i != -1)
        //        {
        //            if (Enum.IsDefined(typeof(Ops), turn * 20))
        //                //an op with the value (turn * 20) exists -> ops with the current precedence have even values -> search from first to last
        //                i = Elements.FindIndex(e => e.GetType() == typeof(Ops) && ((int)e) / 20 == turn);
        //            else
        //                i = Elements.FindLastIndex(e => e.GetType() == typeof(Ops) && ((int)e) / 20 == turn);

        //            //Eval(Elements, i);
        //        }


        //    }

        //    return 0;
        //}

        internal void Eval(int i)
        {
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
        struct Operator
        {
            public Operator(string notation, int precedence, bool backwards, Func<double[], double> func, params int[] relOpdInd)
            {
                Notation = notation;
                Precedence = precedence;
                Backwards = backwards;
                RelOpdInd = relOpdInd;
                Function = func;
            }
            public string Notation { get; }
            public int Precedence { get; }

            //When true, the operators of this kind will be searched and evaluated from right to left
            public bool Backwards { get; }
            //Operand indexes relative to the operator's.
            public int[] RelOpdInd { get; }
            public Func<double[], double> Function { get; }
            public override string ToString() => Notation;
        }
    }
}
