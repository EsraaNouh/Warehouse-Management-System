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
    public partial class MoveItems : Form
    {
        Commercial_CompanyEntities Ent;
        int StID;
        int ICode;
        int Quantity;
        string supplier;
        DateTime date;
        int sper;
        int exp;
        public MoveItems()
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
        public int ItemCode
        {
            set
            {
                ICode = value;
            }
        }
        private void MoveItems_Load(object sender, EventArgs e)
        {
            var ItemsData = Ent.Store_Details(StID);
            foreach (var D in ItemsData)
            {
                Boolean flag = true;
                if (D.Item_Code == ICode && flag)
                {
                    textBox4.Text = D.CurrentQuantity.ToString();
                    Quantity = (int)D.CurrentQuantity;
                    supplier = D.Supplier_Name;
                    sper = D.SP_Num;
                    flag = false;
                }
            }
            var AllStores = Ent.Display_Stores(null);
            foreach (var Store in AllStores)
            {
                comboBox1.Items.Add(Store.Store_ID.ToString());
            }
            var ProdDate = Ent.Display_Supply_Permission_Items(sper);
            foreach (var P in ProdDate)
            {
                if( P.Item_Code == ICode)
                {
                    date = (DateTime)P.Prod_Date;
                    exp = (int)P.Expiry;
                }
            }
            textBox1.Text = Ent.Display_Stores(StID).First().Store_Name;
            textBox6.Text = Ent.Display_Items(ICode).First().Item_Name;
            textBox3.Text = supplier;
            textBox7.Text = date.ToString();
            textBox5.Text = exp.ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text == "")
            {
                MessageBox.Show("Please Choose a destination store");
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Enter Quantity");
            }
            else if (int.Parse(textBox2.Text) <= Quantity)
            {
                Ent.MoveItems(StID, int.Parse(comboBox1.Text), ICode, int.Parse(textBox2.Text), sper);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Quantity is not available");
                textBox2.Text = Quantity.ToString();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
