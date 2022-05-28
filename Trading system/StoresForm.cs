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
    public partial class Company_Stores : Form
    {
        Commercial_CompanyEntities Ent;
        int storenum;
        public Company_Stores()
        {
            InitializeComponent();
            Ent = new Commercial_CompanyEntities();
        }

        private void Company_Stores_Load(object sender, EventArgs e)
        {
            var MyStores = Ent.Display_Stores(null);
            storenum = 1;
            int YPosition = 60;
            foreach (var Store in MyStores)
            {
                if (storenum < 5)
                {
                    StoreGroupBox StoreContainer;
                    StoreContainer = new StoreGroupBox();
                    StoreContainer.Location = new Point(12, YPosition);
                    StoreContainer.Size = new Size(436, 134);
                    StoreContainer.Title = "Store " + storenum.ToString();
                    this.Controls.Add(StoreContainer);
                    storenum++;
                    YPosition += 140;
                    StoreContainer.IDTextBox = Store.Store_ID.ToString();
                    StoreContainer.NameTextBox = Store.Store_Name;
                    StoreContainer.AddressTextBox = Store.Store_Address;
                    StoreContainer.KeeperTextBox = Store.Storekeeper;
                    StoreContainer.ItemsButton.Click += new EventHandler(ItemsButton_Click);
                }
                else
                {
                    Button NextBtn;
                    NextBtn = new Button();
                    NextBtn.Location = new Point(415, YPosition);
                    NextBtn.AutoSize = true;
                    NextBtn.Text = "Next";
                    NextBtn.Click += new EventHandler(NextBtn_Click);
                    this.Controls.Add(NextBtn);
                }
            }
        }
        private void ItemsButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            int storeid = int.Parse(((StoreGroupBox)(btn.Parent).Parent).IDTextBox);
            var StoreItems = Ent.Display_Store_Items(storeid);
            GroupBox ItemsContainer;
            ItemsContainer = new GroupBox();
            ItemsContainer.Location = new Point(450, ((StoreGroupBox)(btn.Parent).Parent).Location.Y + 3);
            ItemsContainer.Size = new Size(460, 128);
            ItemsContainer.Text = btn.Parent.Text + " Items";
            int YPosition = 12;
            foreach (var Item in StoreItems)
            {
                ItemData MyItem;
                MyItem = new ItemData();
                MyItem.Location = new Point(12, YPosition);
                MyItem.IDTextBox = Item.Item_Code.ToString();
                string ItemName = Ent.Display_Items(Item.Item_Code).First().Item_Name;
                MyItem.NameTextBox = ItemName;
                MyItem.QtyTextBox = Item.Total_Quantity.ToString();
                ItemsContainer.Controls.Add(MyItem);
                ItemsContainer.Controls.Add(MyItem);
                YPosition += 30;
            }
            this.Controls.Add(ItemsContainer);
        }
        private void NextBtn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DetailsBtn);
            var MyStores = Ent.Display_Stores(null);
            int i = 1;
            int counter = 1;
            int YPosition = 60;
            foreach (var Store in MyStores)
            {
                i++;
                if (i > storenum)
                {
                    if (counter < 5)
                    {
                        StoreGroupBox StoreContainer;
                        StoreContainer = new StoreGroupBox();
                        StoreContainer.Location = new Point(12, YPosition);
                        StoreContainer.Size = new Size(436, 134);
                        StoreContainer.Title = "Store " + storenum.ToString();
                        this.Controls.Add(StoreContainer);
                        storenum++;
                        counter++;
                        YPosition += 140;
                        StoreContainer.IDTextBox = Store.Store_ID.ToString();
                        StoreContainer.NameTextBox = Store.Store_Name;
                        StoreContainer.AddressTextBox = Store.Store_Address;
                        StoreContainer.KeeperTextBox = Store.Storekeeper;
                        StoreContainer.ItemsButton.Click += new EventHandler(ItemsButton_Click);
                    }
                    else
                    {
                        Button NextBtn;
                        NextBtn = new Button();
                        NextBtn.Location = new Point(415, YPosition);
                        NextBtn.AutoSize = true;
                        NextBtn.Text = "Next";
                        NextBtn.Click += new EventHandler(NextBtn_Click);
                        this.Controls.Add(NextBtn);
                    }
                }
            }
        }
        private void DetailsBtn_Click(object sender, EventArgs e)
        {
            DetailsForm SecondForm = new DetailsForm();
            SecondForm.Show();
            this.Hide();
        }

        private void ReportsBtn_Click(object sender, EventArgs e)
        {
            Reports ThirdForm = new Reports();
            ThirdForm.Show();
            this.Hide();
        }
    }
}
