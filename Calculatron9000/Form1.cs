using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculatron9000
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            //MessageBox.Show(Thread.CurrentThread.CurrentUICulture.Name);

            //NumberFormatInfo nfi = new CultureInfo("", false).NumberFormat;

            //nfi.NumberDecimalSeparator = ".";

            //Thread.CurrentThread.CurrentUICulture.NumberFormat = nfi;
            //Thread.CurrentThread.CurrentCulture.NumberFormat = nfi;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //string expStr1 = "7 * 5+ 9,36/(sqrt 96,5 + 9 /(6  / 5)* ln5,32 +  4^  1,5) ^1,12 *ln ln96,32";
            //txtInput.Text = "7 * 5+ 9.36/(sqrt 96.5 + 9 /(6  / 5)* ln5.32 +  4^  1.5) ^1.12 *ln ln96.32";
        }


        private void btnWorkOut_Click(object sender, EventArgs e)
        {
            try
            {
                Expression exp1 = new Expression(txtInput.Text);

                //lblDisp.Visible = false;

                lblDisp.Text = exp1.WorkOut().ToString();

                //lblDisp.Location = new Point(12, this.Size.Width - lblDisp.Size.Width - 29);
                
                //lblDisp.Visible = true;
            }
            catch (InvalidExpressionException ie)
            {
                MessageBox.Show(ie.Message);
            }
        }
    }
}
