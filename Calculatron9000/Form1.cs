using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculatron9000
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        enum Ops { plus, times, ln };

        IDictionary<string, Ops> opDic = new Dictionary<string, Ops>() {
                {"+", Ops.plus},
                {"*", Ops.times},
                {"ln", Ops.ln},
            };
        private void Form1_Load(object sender, EventArgs e)
        {
            //string[] opchars = new string[] { "+", "*" , "ln"};

            string exp1 = "+ln3+88**0.7+ln5ln";

            List<object> exp1Parsed = Arrange(exp1, opDic);

            listBox1.DataSource = exp1Parsed;
        }

        List<object> Arrange(string exp, IDictionary<string, Ops> opDic)
        {
            List<object> elements = new List<object>();

            //clear whitespaces
            exp.Replace(" ", "");

            //isolate adjacent operators with whitespaces
            foreach (string o in opDic.Keys.ToArray())
                exp = exp.Replace(o, " " + o + " ");

            //split the expression by the operators to get the numbers
            //seperators are operators with attached whitespaces
            string[] nums = exp.Split(opDic.Keys.ToArray().Select(o => " "+o+" ").ToArray(), StringSplitOptions.RemoveEmptyEntries);

            string[] expParsed = exp.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string e in expParsed)
            {
                if (nums.Contains(e)) 
                    elements.Add(Convert.ToDouble(e));
                else
                    elements.Add(opDic[e]);
            }

            return elements;
        }
    }
}
