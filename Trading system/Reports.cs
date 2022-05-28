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
    public partial class Reports : Form
    {
        Commercial_CompanyEntities Ent;
        public Reports()
        {
            InitializeComponent();
            Ent = new Commercial_CompanyEntities();
        }
        private void Reports_Load(object sender, EventArgs e)
        {
            var AllStores = Ent.Display_Stores(null);
            foreach(var Store in AllStores)
            {
                StComboBox.Items.Add(Store.Store_ID);
            }
            var AllItems = Ent.Display_Items(null);
            foreach (var Item in AllItems)
            {
                IComboBox.Items.Add(Item.Item_Code);
            }
            LongTimeGridView.DataSource = Ent.ItemsWeHaveForALongTime();
            ExpirationGridView.DataSource = Ent.ItemsNearingExpiration();
            AllItemsGridView.DataSource = Ent.ItemsInStores();
        }

        private void StComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            StNameLabel.Visible = StAddLabel.Visible = StKeepLabel.Visible = true;
            StNameLabel.Text = Ent.Display_Stores(int.Parse(StComboBox.Text)).First().Store_Name;
            StAddLabel.Text = Ent.Display_Stores(int.Parse(StComboBox.Text)).First().Store_Address;
            StKeepLabel.Text = Ent.Display_Stores(int.Parse(StComboBox.Text)).First().Storekeeper;
        }

        private void StBtn_Click(object sender, EventArgs e)
        {
            dataGridViewSt.Visible = true;
            dataGridViewSt.DataSource = Ent.StoreReport1(int.Parse(StComboBox.Text), StFromDate.Value, StToDate.Value);
        }

        private void IComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            INameLabel.Visible = IMUnitsLabel.Visible = true;
            INameLabel.Text = Ent.Display_Items(int.Parse(IComboBox.Text)).First().Item_Name;
            try
            {
                IMUnitsLabel.Text = Ent.Display_I_M_Unit(int.Parse(IComboBox.Text)).First().Measuring_Unit.ToString();

            }catch(Exception)
            {
                IMUnitsLabel.Text = "";
            }
        }

        private void IBtn_Click(object sender, EventArgs e)
        {
            dataGridViewI.Visible = true;
            dataGridViewI.DataSource = Ent.ItemReport2(int.Parse(IComboBox.Text), IFromDate.Value, IToDate.Value);
        }

        private void ChsStBtn_Click(object sender, EventArgs e)
        {
            ChooseStores Stores = new ChooseStores();
            DialogResult result;
            result = Stores.ShowDialog();
            if(result == DialogResult.OK)
            {
                MessageBox.Show(Stores.ChoosenStores.ToString());
                //SuppPerItemsGridView.Columns["Number_Of_Items"].Visible = false;
            }
            else
            {

            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Company_Stores FirstForm = new Company_Stores();
            FirstForm.Show();
            this.Hide();
        }

    }
}
