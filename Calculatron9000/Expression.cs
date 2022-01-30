using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculatron9000
{
    class Expression
    {
        char[] opchars = new char[] { '+', '*' };

        public List<object> Split(string exp, char[] opchars)
        {
            List<object> elements = new List<object>();

            string[] nums = exp.Split(opchars);

            char[] ops = exp.ToCharArray().TakeWhile(c => opchars.Contains(c)).ToArray();

            int i;
            for (i = 0; i < ops.Length; i++)
            {
                elements.Add(Convert.ToDouble(nums[i]));
                elements.Add(ops[i]);
            }
            elements.Add(nums[i]);

            

            return elements;
        }




    }
}
