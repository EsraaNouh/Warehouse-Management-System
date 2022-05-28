using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trading_system
{
    public partial class AddMeasuringUnit : Form
    {
        public AddMeasuringUnit()
        {
            InitializeComponent();
        }

        public string MesuringUnit
        {
            get
            {
                return textBox1.Text ;
            }

        }
        private void Button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
