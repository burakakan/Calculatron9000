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
        
        private void Form1_Load(object sender, EventArgs e)
        {
            string expStr1 = "4+3*ln8*7+5";

            Expression exp1 = new Expression(expStr1);

            //listBox1.DataSource = exp1.Elements;


            exp1.Eval(4);

            listBox1.DataSource = exp1.Elements;

        }

        
    }
}
