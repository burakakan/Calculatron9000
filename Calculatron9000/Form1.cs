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
            string expStr1 = "7 * 5+ 9.36/sqrt 96.5 + 9 /6  / 5* ln5.32 +  4^  1.5 ^1.12 *ln ln96.32";

            Expression exp1 = new Expression(expStr1);

            MessageBox.Show(exp1.WorkOut().ToString());
        }

        
    }
}
