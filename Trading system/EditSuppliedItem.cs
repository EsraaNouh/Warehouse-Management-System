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
    public partial class EditSuppliedItem : Form
    {
        Commercial_CompanyEntities Ent;
        int PerNum;
        int ICode;
        public EditSuppliedItem()
        {
            InitializeComponent();
            Ent = new Commercial_CompanyEntities();
        }
        public int PerNumber
        {
            set
            {
                PerNum = value;
            }
        }
        public int ItemCode
        {
            set
            {
                ICode = value;
            }
        }

        private void EditSuppliedItem_Load(object sender, EventArgs e)
        {
            var SuppliedItems = Ent.Display_Supply_Permission_Items(PerNum);
            foreach(var Item in SuppliedItems)
            {
                if(Item.Item_Code == ICode)
                {
                    textBox4.Text = Ent.Display_Items(Item.Item_Code).First().Item_Name;
                    textBox1.Text = Item.Quantity.ToString();
                    dateTimePicker1.Value = (DateTime)Item.Prod_Date;
                    textBox3.Text = Item.Expiry.ToString();
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Ent.Edit_Supply_Permission_Item(PerNum, ICode, int.Parse(textBox1.Text), dateTimePicker1.Value, int.Parse(textBox3.Text));
            this.DialogResult = DialogResult.OK;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
