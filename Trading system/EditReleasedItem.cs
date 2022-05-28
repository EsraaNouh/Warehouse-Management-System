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
    public partial class EditReleasedItem : Form
    {
        Commercial_CompanyEntities Ent;
        int PerNum;
        int ICode;
        int StID;
        int Remaining_Quantity;
        int OldQuantity;
        public EditReleasedItem()
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
        public int StoreID
        {
            set
            {
                StID = value;
            }
        }
        private void EditReleasedItem_Load(object sender, EventArgs e)
        {
            var ReleasedItems = Ent.Display_Release_Permission_Items(PerNum);
            foreach (var Item in ReleasedItems)
            {
                if (Item.Item_Code == ICode)
                {
                    textBox4.Text = Ent.Display_Items(Item.Item_Code).First().Item_Name;
                    textBox1.Text = Item.Quantity.ToString();
                    OldQuantity = (int)Item.Quantity;
                }
            }
            var Items = Ent.Store_Details(StID);
            Boolean flag = true;
            foreach (var Item in Items)
            {
                if( Item.Item_Code == ICode && flag)
                {
                    Remaining_Quantity = (int)Item.CurrentQuantity;
                    flag = false;
                }
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (int.Parse(textBox1.Text) <= OldQuantity)
            {
                Ent.Edit_Release_Permission_Item(PerNum, ICode, int.Parse(textBox1.Text));
                this.DialogResult = DialogResult.OK;
            }
            else if ((int.Parse(textBox1.Text) - OldQuantity) <= Remaining_Quantity)
            {
                Ent.Edit_Release_Permission_Item(PerNum, ICode, int.Parse(textBox1.Text));
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("This quantity is not available in our store");
                textBox1.Text = OldQuantity.ToString();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
