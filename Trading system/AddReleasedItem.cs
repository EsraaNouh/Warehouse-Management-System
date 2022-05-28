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
    public partial class AddReleasedItem : Form
    {
        Commercial_CompanyEntities Ent;
        int StID;
        int PerNum;
        int SPER;
        public AddReleasedItem()
        {
            InitializeComponent();
            Ent = new Commercial_CompanyEntities();
        }
        public int StoreID
        {
            set
            {
                StID = value;
            }
        }
        public int PermissionNumber
        {
            set
            {
                PerNum = value;
            }

        }
        public int ItemCode
        {
            get
            {
                return int.Parse(comboBox1.Text);
            }
        }

        private void AddReleasedItem_Load(object sender, EventArgs e)
        {
            var AllDetails = Ent.Store_Details(StID);
            foreach (var Row in AllDetails)
            {
                comboBox1.Items.Add(Row.Item_Code);
            }
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var AllDetails = Ent.Store_Details(StID);
            int i = 0;
            foreach (var Row in AllDetails)
            {
                if (i == comboBox1.SelectedIndex)
                {
                    textBox1.Text = Row.Item_Name;
                    textBox2.Text = Row.CurrentQuantity.ToString();
                    SPER = Row.SP_Num;
                }
                i++;
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if(int.Parse(textBox3.Text) > int.Parse(textBox2.Text))
            {
                MessageBox.Show("This Quantity is not available in our store");
                textBox3.Text = textBox2.Text;
            }
            Ent.Add_Release_Permission_Item(PerNum, int.Parse(comboBox1.Text), int.Parse(textBox3.Text), SPER); 
            this.DialogResult = DialogResult.OK;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
