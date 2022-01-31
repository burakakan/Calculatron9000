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
            string exp1 = "+ln3+88**0.7+ln5ln";

            List<object> exp1Parsed = exp1.Arrange();

            listBox1.DataSource = exp1Parsed;
        }

    }
}
