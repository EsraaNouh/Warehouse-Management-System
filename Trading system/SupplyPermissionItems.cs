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
    public partial class SupplyPermissionItems : Form
    {
        Commercial_CompanyEntities Ent;
        int ICode;
        int IQuantity;
        int IExp;
        public SupplyPermissionItems()
        {
            InitializeComponent();
            Ent = new Commercial_CompanyEntities();
        }
        private void SupplyPermissionItems_Load(object sender, EventArgs e)
        {
            var MyItems = Ent.Display_Items(null);
            foreach(var Item in MyItems)
            {
                comboBox4.Items.Add(Item.Item_Code);
            }
        }
        public string ItemNo
        {
            set
            {
                groupBox1.Text = value;
            }
        }
        public int ItemCode
        {
            get
            {
                return ICode;
            }
        }
        public int ItemQuantity
        {
            get
            {
                return IQuantity;
            }
        }
        public int ItemExp
        {
            get
            {
                return IExp;
            }
        }
        public DateTime ItemProd
        {
            get
            {
                return dateTimePicker2.Value;
            }
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            if(textBox6.Text == "" && comboBox4.Text == "")
            {
                MessageBox.Show("You must add item");
            }
            else if (textBox10.Text =="")
            {
                MessageBox.Show("You must enter the quantity");
            }
            else if (textBox7.Text == "")
            {
                MessageBox.Show("You must enter the expiration period");
            }
            else if(textBox6.Text != "")
            {
                if(textBox8.Text == "")
                {
                    MessageBox.Show("You must enter the item name ");
                }
                else
                {
                    string mu = (textBox9.Text == "") ? null : textBox9.Text;
                    int Conf = (int)Ent.Add_Item(int.Parse(textBox6.Text), textBox8.Text, mu).First();
                    if (Conf == 1)
                    {
                        ICode = int.Parse(textBox6.Text);
                        IQuantity = int.Parse(textBox10.Text);
                        IExp = int.Parse(textBox7.Text);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Duplicated Code");
                    }
                }
            }
            else
            {
                ICode = int.Parse(comboBox4.Text);
                IQuantity = int.Parse(textBox10.Text);
                IExp = int.Parse(textBox7.Text);
                this.DialogResult = DialogResult.OK;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void TextBox6_TextChanged(object sender, EventArgs e)
        {
            comboBox4.Text = string.Empty;
        }

        private void ComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox6.Text = textBox8.Text = textBox9.Text = string.Empty;
        }
    }
}
