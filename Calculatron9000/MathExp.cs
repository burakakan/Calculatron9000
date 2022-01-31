using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculatron9000
{
    static class MathExp
    {
        enum Ops { plus, times, ln };

        static IDictionary<string, Ops> OpDic => new Dictionary<string, Ops>() {
                {"+", Ops.plus},
                {"*", Ops.times},
                {"ln", Ops.ln},
            };

        public static List<object> Arrange(this String exp)
        {
            List<object> elements = new List<object>();

            //clear whitespaces
            exp.Replace(" ", "");

            //isolate adjacent operators with whitespaces
            foreach (string o in OpDic.Keys.ToArray())
                exp = exp.Replace(o, " " + o + " ");

            //split the expression by the operators to get the numbers
            //seperators are operators with attached whitespaces
            string[] nums = exp.Split(OpDic.Keys.ToArray().Select(o => " " + o + " ").ToArray(), StringSplitOptions.RemoveEmptyEntries);

            string[] expParsed = exp.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string e in expParsed)
            {
                if (nums.Contains(e))
                    elements.Add(Convert.ToDouble(e));
                else
                    elements.Add(OpDic[e]);
            }

            return elements;
        }
    }
}
