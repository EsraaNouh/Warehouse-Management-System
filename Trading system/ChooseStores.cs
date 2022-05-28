using System;
using System.Collections;
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
    public partial class ChooseStores : Form
    {
        Commercial_CompanyEntities Ent;
        ArrayList CheckedStores = new ArrayList();
        public ChooseStores()
        {
            InitializeComponent();
            Ent = new Commercial_CompanyEntities();
        }
        public string[] ChoosenStores
        {
            get
            {
                return (string[])CheckedStores.ToArray();
            }
        }

        private void ChooseStores_Load(object sender, EventArgs e)
        {
            var AllStores = Ent.Display_Stores(null);
            int Yposition = 58;
            foreach (var Store in AllStores)
            {
                CheckBox MyCheckBox = new CheckBox();
                MyCheckBox.Location = new Point(56, Yposition);
                MyCheckBox.Name = Store.Store_ID.ToString();
                MyCheckBox.Text = Store.Store_Name;
                Yposition += 20;
                this.Controls.Add(MyCheckBox);
                MyCheckBox.Click += new EventHandler(this.MyCheckBox_Click);
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

        private void MyCheckBox_Click(object sender, EventArgs e)
        {
            var chkbx = sender as CheckBox;
            if (chkbx.Checked)
            {
                CheckedStores.Add(chkbx.Name.ToString());
            }
            else
            {
                CheckedStores.Remove(chkbx.Name.ToString());
            }
        }
    }
}
