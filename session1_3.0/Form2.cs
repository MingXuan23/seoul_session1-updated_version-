using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace session1_3._0
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
           
            
            chart1.Series["Series1"].Points.AddXY("my",0.2);
            chart1.Series["Series1"].Points.AddXY("bx",4);
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
