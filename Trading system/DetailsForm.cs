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
    public partial class DetailsForm : Form
    {
        Commercial_CompanyEntities Ent;
        int NoOfCheckboxControls;
        public DetailsForm()
        {
            InitializeComponent();
            Ent = new Commercial_CompanyEntities();
            tabControl1.Size = this.Size;
            tabControl2.Size = this.Size;
        }

        private void DetailsForm_Load(object sender, EventArgs e)
        {
            var MyStores = Ent.Display_Stores(null);
            foreach (var Store in MyStores)
            {
                StoreComboBox.Items.Add(Store.Store_ID.ToString());
                SPerStComboBox. Items.Add(Store.Store_ID.ToString());
                SupPerStComboBox.Items.Add(Store.Store_ID.ToString());
                RPerStComboBox.Items.Add(Store.Store_ID.ToString());
            }
            var MyItems = Ent.Display_Items(null);
            foreach (var Item in MyItems)
            {
                ItemComboBox.Items.Add(Item.Item_Code);
            }
            var MySuppliers = Ent.Display_Suppliers(null);
            SuppliersGridView.DataSource = MySuppliers;
            MySuppliers = Ent.Display_Suppliers(null);
            foreach(var Supplier in MySuppliers)
            {
                SPerSComboBox.Items.Add(Supplier.Supplier_Id.ToString());
                SupPerSComboBox.Items.Add(Supplier.Supplier_Id.ToString());
            }
            var MyClients = Ent.Display_Clients(null);
            ClientGridView.DataSource = MyClients;
            MyClients = Ent.Display_Clients(null);
            foreach (var Client in MyClients)
            {
                RPerCComboBox.Items.Add(Client.Client_Id.ToString());
                RelPerCComboBox.Items.Add(Client.Client_Id.ToString());
            }
            var SupplyPermissions = Ent.Display_Supply_Permissions(null);
            SupplyPermGridView.DataSource = SupplyPermissions.Select(SP => new
            {
                Permission_Num = SP.Permission_Num,
                Permission_Date = SP.Permission_Date,
                Store_Name = SP.Store_Name,
                Supplier_Name = SP.Supplier_Name,
                Number_Of_Items = Ent.Display_Supply_Permission_Items(SP.Permission_Num).First().Number_Of_Items
            }).ToList();
            SupplyPermissions = Ent.Display_Supply_Permissions(null);
            foreach (var Permission in SupplyPermissions)
            {
                SupPerComboBox.Items.Add(Permission.Permission_Num.ToString());
            }
            var ReleasePermissions = Ent.Display_Release_Permissions(null);
            ReleasePermGridView.DataSource = ReleasePermissions.Select(RP => new
            {
                Permission_Num = RP.Permission_Num,
                Permission_Date = RP.Permission_Date,
                Store_Name = RP.Store_Name,
                Client_Name = RP.Client_Name,
                Number_Of_Items = Ent.Display_Release_Permission_Items(RP.Permission_Num).First().Number_Of_Items
            }).ToList();
            ReleasePermissions = Ent.Display_Release_Permissions(null);
            foreach (var Permission in ReleasePermissions)
            {
                RelPerComboBox.Items.Add(Permission.Permission_Num.ToString());
            }
        }

        private void StoreComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemsGridView.Visible = true;
            StIDTextBox.ReadOnly = true;
            ItemsGridView.Columns.Clear();
            var MyStore = Ent.Display_Stores(int.Parse(StoreComboBox.Text)).First();
            StIDTextBox.Text = MyStore.Store_ID.ToString();
            StNameTextBox.Text = MyStore.Store_Name;
            StAddTextBox.Text = MyStore.Store_Address;
            StKepperTextBox.Text = MyStore.Storekeeper;
            var StoreItems = Ent.Display_Store_Items(int.Parse(StoreComboBox.Text));
            ItemsGridView.DataSource = StoreItems.Select(SI => new
            {
                Item_Code = SI.Item_Code,
                Item_Name = Ent.Display_Items(SI.Item_Code).First().Item_Name,
                Quantity = SI.Total_Quantity
            }).ToList();
            DataGridViewButtonColumn MoveItem = new DataGridViewButtonColumn();
            ItemsGridView.Columns.Add(MoveItem);
            for (int i = 0; i < ItemsGridView.RowCount; i++)
            {
                ItemsGridView.Rows[i].Cells[3].Value = "Move";
            }
        }
        private void AddStoreButton_Click(object sender, EventArgs e)
        {
            if (StIDTextBox.Text == "")
            {
                MessageBox.Show("You Must Enter the store ID");
            }
            else if (StNameTextBox.Text == "")
            {
                MessageBox.Show("You Must Enter the store Name");
            }
            else
            {
                string Address = (StAddTextBox.Text == "") ? null : StAddTextBox.Text;
                string Keeper = (StKepperTextBox.Text == "") ? null : StKepperTextBox.Text;
                int Conf = (int)Ent.Add_Store(int.Parse(StIDTextBox.Text), StNameTextBox.Text, Address, Keeper).First();
                if (Conf == 1)
                {
                    MessageBox.Show("The Store was added successfully ");
                    StoreComboBox.Items.Add(StIDTextBox.Text);
                    SPerStComboBox.Items.Add(StIDTextBox.Text);
                    RPerStComboBox.Items.Add(StIDTextBox.Text);
                    SupPerStComboBox.Items.Add(StIDTextBox.Text);
                    StClrBtn_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Duplicated ID");
                }
            }
        }
        private void EditStoreButton_Click(object sender, EventArgs e)
        {
            if (StNameTextBox.Text == "")
            {
                MessageBox.Show("The store must have a name");
            }
            else
            {
                string Address = (StAddTextBox.Text == "") ? null : StAddTextBox.Text;
                string Keeper = (StKepperTextBox.Text == "") ? null : StKepperTextBox.Text;
                Ent.Edit_Store(int.Parse(StIDTextBox.Text), StNameTextBox.Text, Address, Keeper);
                MessageBox.Show("The Store was updated successfully ");
                StClrBtn_Click(sender, e);
            }
        }
        private void StClrBtn_Click(object sender, EventArgs e)
        {
            ItemsGridView.Visible = false;
            StoreComboBox.Text = StIDTextBox.Text = StNameTextBox.Text = StAddTextBox.Text = StKepperTextBox.Text = string.Empty;
            ItemsGridView.Columns.Clear();
            StIDTextBox.ReadOnly = false;
        }
        private void ItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ICodeTextBox.ReadOnly = true;
            var MyItem = Ent.Display_Items(int.Parse(ItemComboBox.Text)).First();
            ICodeTextBox.Text = MyItem.Item_Code.ToString();
            INameTextBox.Text = MyItem.Item_Name;
            MUnitsListBox.Visible = true;
            MUnitsLabel.Visible = true;
            AddMUBtn.Visible = true;
            IMUTextBox.Visible = false;
            var MyMUs = Ent.Display_I_M_Unit(MyItem.Item_Code);
            foreach (var MU in MyMUs)
            {
                MUnitsListBox.Items.Add(MU.Measuring_Unit);
            }
        }
        private void AddItemBtn_Click(object sender, EventArgs e)
        {
            if (ICodeTextBox.Text == "")
            {
                MessageBox.Show("You Must Enter the Item Code");
            }
            else if (INameTextBox.Text == "")
            {
                MessageBox.Show("You Must Enter the Item Name");
            }
            else
            {
                string mu = (IMUTextBox.Text == "") ? null : IMUTextBox.Text;
                int Conf = (int)Ent.Add_Item(int.Parse(ICodeTextBox.Text), INameTextBox.Text, mu).First();
                if (Conf == 1)
                {
                    MessageBox.Show("The Item was added successfully ");
                    ItemComboBox.Items.Add(ICodeTextBox.Text);
                    ClrItemBtn_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Duplicated Code");
                }
            }
        }
        private void EditItemBtn_Click(object sender, EventArgs e)
        {
            if (INameTextBox.Text == "")
            {
                MessageBox.Show("The item must have a name");
            }
            else
            {
                Ent.Edit_Item(int.Parse(ICodeTextBox.Text), INameTextBox.Text);
                MessageBox.Show("The Item was updated successfully ");
                ClrItemBtn_Click(sender, e);
            }
        }
        private void MUnitsListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.MUnitsListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                AddMeasuringUnit NewMU = new AddMeasuringUnit();
                DialogResult Conf;
                Conf = NewMU.ShowDialog();
                if (Conf == DialogResult.OK)
                {
                    Ent.Delete_I_M_Unit(int.Parse(ICodeTextBox.Text), MUnitsListBox.SelectedItem.ToString());
                    int Added = (int)Ent.Add_I_Measuring_Unit(int.Parse(ICodeTextBox.Text), NewMU.MesuringUnit).First();
                    if (Added == 1)
                    {
                        MessageBox.Show("The Measuring Unit was added successfully ");
                        ClrItemBtn_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Duplicated Measuring unit");
                    }
                }
            }
        }
        private void AddMUBtn_Click(object sender, EventArgs e)
        {
            AddMeasuringUnit NewMU = new AddMeasuringUnit();
            DialogResult Conf;
            Conf = NewMU.ShowDialog();
            if (Conf == DialogResult.OK)
            {
                int Added = (int)Ent.Add_I_Measuring_Unit(int.Parse(ICodeTextBox.Text), NewMU.MesuringUnit).First();
                if (Added == 1)
                {
                    MessageBox.Show("The Measuring Unit was added successfully ");
                    ClrItemBtn_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Duplicated Measuring unit");
                }
            }
        }

        private void ClrItemBtn_Click(object sender, EventArgs e)
        {
            ItemComboBox.Text = ICodeTextBox.Text = INameTextBox.Text = IMUTextBox.Text = string.Empty;
            MUnitsListBox.Visible = false;
            MUnitsLabel.Visible = false;
            AddMUBtn.Visible = false;
            IMUTextBox.Visible = true;
            MUnitsListBox.Items.Clear();
            ICodeTextBox.ReadOnly = false;
        }

        private void SuppliersGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SIDTextBox.ReadOnly = true;
                int id = int.Parse(SuppliersGridView.Rows[e.RowIndex].Cells["Supplier_Id"].Value.ToString());
                var MySupplier = Ent.Display_Suppliers(id).First();
                SIDTextBox.Text = MySupplier.Supplier_Id.ToString();
                SNameTextBox.Text = MySupplier.Supplier_Name;
                SMailTextBox.Text = MySupplier.Supplier_Email;
                SFaxTextBox.Text = MySupplier.Supplier_Fax;
                SMobTextBox.Text = MySupplier.Supplier_Mobile;
                SPhoneTextBox.Text = MySupplier.Supplier_Phone;
                SSiteTextBox.Text = MySupplier.Supplier_Website;
            }
        }
        private void AddSupplierBtn_Click(object sender, EventArgs e)
        {
            if (SIDTextBox.Text == "")
            {
                MessageBox.Show("You Must Enter the supplier ID");
            }
            else if (SNameTextBox.Text == "")
            {
                MessageBox.Show("You Must Enter the supplier Name");
            }
            else
            {
                string phone = (SPhoneTextBox.Text == "") ? null : SPhoneTextBox.Text;
                string fax = (SFaxTextBox.Text == "") ? null : SFaxTextBox.Text;
                string mobile = (SMobTextBox.Text == "") ? null : SMobTextBox.Text;
                string email = (SMailTextBox.Text == "") ? null : SMailTextBox.Text;
                string website = (SSiteTextBox.Text == "") ? null : SSiteTextBox.Text;
                int Conf = (int)Ent.Add_Supplier(int.Parse(SIDTextBox.Text), SNameTextBox.Text, phone, fax, mobile, email, website).First();
                if (Conf == 1)
                {
                    MessageBox.Show("The Supplier was added successfully ");
                    SuppliersGridView.DataSource = Ent.Display_Suppliers(null);
                    SPerSComboBox.Items.Add(SIDTextBox.Text);
                    SupPerSComboBox.Items.Add(SIDTextBox.Text);
                    SupplierClrBtn_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Duplicated ID");
                }
            }
        }
        private void EditSupplierBtn_Click(object sender, EventArgs e)
        {
            if (SNameTextBox.Text == "")
            {
                MessageBox.Show("The supplier must have a name");
            }
            else
            {
                string phone = (SPhoneTextBox.Text == "") ? null : SPhoneTextBox.Text;
                string fax = (SFaxTextBox.Text == "") ? null : SFaxTextBox.Text;
                string mobile = (SMobTextBox.Text == "") ? null : SMobTextBox.Text;
                string email = (SMailTextBox.Text == "") ? null : SMailTextBox.Text;
                string website = (SSiteTextBox.Text == "") ? null : SSiteTextBox.Text;
                Ent.Edit_Supplier(int.Parse(SIDTextBox.Text), SNameTextBox.Text, phone, fax, mobile, email, website);
                MessageBox.Show("The Supplier was updated successfully ");
                SuppliersGridView.DataSource = Ent.Display_Suppliers(null);
                SupplierClrBtn_Click(sender, e);
            }
        }
        private void SupplierClrBtn_Click(object sender, EventArgs e)
        {
            SIDTextBox.ReadOnly = false;
            SIDTextBox.Text = SNameTextBox.Text = SPhoneTextBox.Text = SFaxTextBox.Text = SMobTextBox.Text = SMailTextBox.Text = SSiteTextBox.Text = string.Empty;
        }
        private void ClientGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                CIDTextBox.ReadOnly = true;
                int id = int.Parse(ClientGridView.Rows[e.RowIndex].Cells["Client_Id"].Value.ToString());
                var MyClient = Ent.Display_Clients(id).First();
                CIDTextBox.Text = MyClient.Client_Id.ToString();
                CNameTextBox.Text = MyClient.Client_Name;
                CMailTextBox.Text = MyClient.Client_Email;
                CFaxTextBox.Text = MyClient.Client_Fax;
                CMobTextBox.Text = MyClient.Client_Mobile;
                CPhoneTextBox.Text = MyClient.Client_Phone;
                CSiteTextBox.Text = MyClient.Client_Website;
            }
        }
        private void AddClientBtn_Click(object sender, EventArgs e)
        {
            if (CIDTextBox.Text == "")
            {
                MessageBox.Show("You Must Enter the client ID");
            }
            else if (CNameTextBox.Text == "")
            {
                MessageBox.Show("You Must Enter the client Name");
            }
            else
            {
                string phone = (CPhoneTextBox.Text == "") ? null : CPhoneTextBox.Text;
                string fax = (CFaxTextBox.Text == "") ? null : CFaxTextBox.Text;
                string mobile = (CMobTextBox.Text == "") ? null : CMobTextBox.Text;
                string email = (CMailTextBox.Text == "") ? null : CMailTextBox.Text;
                string website = (CSiteTextBox.Text == "") ? null : CSiteTextBox.Text;
                int Conf = (int)Ent.Add_Client(int.Parse(CIDTextBox.Text), CNameTextBox.Text, phone, fax, mobile, email, website).First();
                if (Conf == 1)
                {
                    MessageBox.Show("The Client was added successfully ");
                    ClientGridView.DataSource = Ent.Display_Clients(null);
                    RPerCComboBox.Items.Add(CIDTextBox.Text);
                    RelPerCComboBox.Items.Add(CIDTextBox.Text);
                    ClientClrBtn_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Duplicated ID");
                }
            }
        }
        private void EditClientBtn_Click(object sender, EventArgs e)
        {
            if (CNameTextBox.Text == "")
            {
                MessageBox.Show("The client must have a name");
            }
            else
            {
                string phone = (CPhoneTextBox.Text == "") ? null : CPhoneTextBox.Text;
                string fax = (CFaxTextBox.Text == "") ? null : CFaxTextBox.Text;
                string mobile = (CMobTextBox.Text == "") ? null : CMobTextBox.Text;
                string email = (CMailTextBox.Text == "") ? null : CMailTextBox.Text;
                string website = (CSiteTextBox.Text == "") ? null : CSiteTextBox.Text;
                Ent.Edit_Client(int.Parse(CIDTextBox.Text), CNameTextBox.Text, phone, fax, mobile, email, website);
                MessageBox.Show("The Client was updated successfully ");
                ClientGridView.DataSource = Ent.Display_Clients(null);
                ClientClrBtn_Click(sender, e);
            }
        }
        private void ClientClrBtn_Click(object sender, EventArgs e)
        {
            CIDTextBox.ReadOnly = false;
            CIDTextBox.Text = CNameTextBox.Text = CPhoneTextBox.Text = CFaxTextBox.Text = CMobTextBox.Text = CMailTextBox.Text = CSiteTextBox.Text = string.Empty;
        }
        private void SupplyPermGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SuppPerItemsGridView.Visible = true;
            if (e.RowIndex >= 0)
            {
                int id = int.Parse(SupplyPermGridView.Rows[e.RowIndex].Cells["Permission_Num"].Value.ToString());
                var PermissionItems = Ent.Display_Supply_Permission_Items(id);
                SuppPerItemsGridView.DataSource = PermissionItems;
                SuppPerItemsGridView.Columns["Number_Of_Items"].Visible = false;
            }
        }
        private void AddSupPerBtn_Click(object sender, EventArgs e)
        {
            if (SPerNumTextBox.Text == "")
            {
                MessageBox.Show("You Must Enter the Permission number");
            }
            else if (SPerStComboBox.Text == "")
            {
                MessageBox.Show("You Must select a store");
            }
            else if (SPerSComboBox.Text == "")
            {
                MessageBox.Show("You Must select a supplier");
            }
            else if (NoOfItemsTextBox.Text == "")
            {
                MessageBox.Show("You Must enter 1 item at least");
            }
            else if(int.Parse(NoOfItemsTextBox.Text) < 1)
            {
                MessageBox.Show("You must enter 1 item at least");
            }
            else
            {
                int NoOfItems = int.Parse(NoOfItemsTextBox.Text);
                DialogResult res = MessageBox.Show("Are you sure you want to Add " + NoOfItems.ToString() + " Items", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (res == DialogResult.OK)
                {
                    SupplyPermissionItems NewItem = new SupplyPermissionItems();
                    NewItem.ItemNo = "Item 1 ";
                    DialogResult Conf;
                    do
                    {
                        Conf = NewItem.ShowDialog();
                        if (Conf == DialogResult.OK)
                        {
                            int Added = (int)Ent.Add_Supply_Permission(int.Parse(SPerNumTextBox.Text), SPerDateTimePicker.Value, int.Parse(SPerSComboBox.Text), int.Parse(SPerStComboBox.Text), NewItem.ItemCode, NewItem.ItemQuantity, NewItem.ItemProd, NewItem.ItemExp).First();
                            if(Added != 1)
                            {
                                MessageBox.Show("Duplicated Number");
                                return;
                            }
                        }
                    } while (Conf != DialogResult.OK);
                    for (int i = 2; i <= NoOfItems; i++)
                    {
                        NewItem = new SupplyPermissionItems();
                        NewItem.ItemNo = "Item " + i.ToString();
                        Conf = NewItem.ShowDialog();
                        if (Conf == DialogResult.OK)
                        {
                            Ent.Add_Supply_Permission_Item(int.Parse(SPerNumTextBox.Text), NewItem.ItemCode, NewItem.ItemQuantity, NewItem.ItemProd, NewItem.ItemExp);
                        }
                    }
                    MessageBox.Show("Supply Permission was added successfully");
                    SupplyPermGridView.DataSource = Ent.Display_Supply_Permissions(null).Select(SP => new
                    {
                        Permission_Num = SP.Permission_Num,
                        Permission_Date = SP.Permission_Date,
                        Store_Name = SP.Store_Name,
                        Supplier_Name = SP.Supplier_Name,
                        Number_Of_Items = Ent.Display_Supply_Permission_Items(SP.Permission_Num).First().Number_Of_Items
                    }).ToList();
                    SupPerComboBox.Items.Add(SPerNumTextBox.Text);
                }
                SPerClrBtn_Click(sender, e);
            }
        }
        private void SupPerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddSuppIBtn.Visible = true;
            SupPerItemsListBox.Items.Clear();
            var SupplyPermission = Ent.Display_Supply_Permissions(int.Parse(SupPerComboBox.Text)).First();
            SPerDate.Value = (DateTime)SupplyPermission.Permission_Date;
            SupPerStComboBox.Text = SupplyPermission.Store_ID.ToString();
            SupPerSComboBox.Text = SupplyPermission.Supplier_ID.ToString();
            var SupplyPerrmissionItems = Ent.Display_Supply_Permission_Items(int.Parse(SupPerComboBox.Text));
            foreach (var Item in SupplyPerrmissionItems)
            {
                SupPerItemsListBox.Items.Add(Item.Item_Code);
            }
        }
        private void SupPerItemsListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.SupPerItemsListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                EditSuppliedItem EditItem = new EditSuppliedItem();
                EditItem.PerNumber = int.Parse(SupPerComboBox.Text);
                EditItem.ItemCode = int.Parse(SupPerItemsListBox.SelectedItem.ToString());
                DialogResult Conf;
                Conf = EditItem.ShowDialog();
                if (Conf == DialogResult.OK)
                {
                    MessageBox.Show("Updated Successfully");
                    SuppPerItemsGridView.Visible = false;
                    ItemsGridView.Visible = false;
                }
            }
        }
        private void AddSuppIBtn_Click(object sender, EventArgs e)
        {
            SupplyPermissionItems NewItem = new SupplyPermissionItems();
            NewItem.ItemNo = "New Item ";
            DialogResult Conf;
            Conf = NewItem.ShowDialog();
            if (Conf == DialogResult.OK)
            {
                Ent.Add_Supply_Permission_Item(int.Parse(SupPerComboBox.Text), NewItem.ItemCode, NewItem.ItemQuantity, NewItem.ItemProd, NewItem.ItemExp);
                SupPerItemsListBox.Items.Add(NewItem.ItemCode.ToString());
                SuppPerItemsGridView.Visible = false;
                ItemsGridView.Visible = false;
            }
        }
        private void EditSupPerBtn_Click(object sender, EventArgs e)
        {
            Ent.Edit_Supply_Permission(int.Parse(SupPerComboBox.Text), SPerDate.Value, int.Parse(SupPerSComboBox.Text), int.Parse(SupPerStComboBox.Text));
            MessageBox.Show("Supply permission was updated successfully");
            SupplyPermGridView.DataSource = Ent.Display_Supply_Permissions(null).Select(SP => new
            {
                Permission_Num = SP.Permission_Num,
                Permission_Date = SP.Permission_Date,
                Store_Name = SP.Store_Name,
                Supplier_Name = SP.Supplier_Name,
                Number_Of_Items = Ent.Display_Supply_Permission_Items(SP.Permission_Num).First().Number_Of_Items
            }).ToList();
            ItemsGridView.Visible = false;
        }
        private void SPerClrBtn_Click(object sender, EventArgs e)
        {
            SPerNumTextBox.Text = SPerStComboBox.Text = SPerSComboBox.Text = NoOfItemsTextBox.Text = string.Empty;
            SPerDateTimePicker.ResetText();
        }
        private void ReleasePermGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ReleasePerItemsGridView.Visible = true;
            if (e.RowIndex >= 0)
            {
                int id = int.Parse(ReleasePermGridView.Rows[e.RowIndex].Cells["Permission_Num"].Value.ToString());
                var PermissionItems = Ent.Display_Release_Permission_Items(id);
                ReleasePerItemsGridView.DataSource = PermissionItems;
                ReleasePerItemsGridView.Columns["Number_Of_Items"].Visible = false;
            }
        }

        private void RPerStComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            for(int j = 1; j <= NoOfCheckboxControls; j++)
            {
                this.tabPage12.Controls.RemoveByKey(j.ToString());
            }
            label50.Visible = true;
            var AllDetails = Ent.Store_Details(int.Parse(RPerStComboBox.Text));
            int i = 1;
            int YPosition = 290;
            CheckBox ItemCheckBox;
            foreach (var Row in AllDetails)
            {
                ItemCheckBox = new CheckBox();
                ItemCheckBox.Location = new Point(180, YPosition);
                ItemCheckBox.Name = i.ToString();
                ItemCheckBox.AutoSize = true; 
                ItemCheckBox.Text = $"        Item: {Row.Item_Name}   Supplier: {Row.Supplier_Name}   Quantity: {Row.CurrentQuantity.ToString()}" ;
                this.tabPage12.Controls.Add(ItemCheckBox);
                i++;
                YPosition += 40;
                ItemCheckBox.Click += new EventHandler(this.ItemCheckBox_Click);
            }
            NoOfCheckboxControls = i--;
            AddRelPerBtn.Location = new Point(231, YPosition+20);
            RPerClrBtn.Location = new Point(356, YPosition+20);
        }
        private void ItemCheckBox_Click(object sender, EventArgs e)
        {
            var chkbx = sender as CheckBox;
            if(chkbx.Checked)
            {
                TextBox QtyTextBox = new TextBox();
                QtyTextBox.Name = "TXTBX"+chkbx.Name;
                QtyTextBox.Location = new Point(chkbx.Location.X + 300, chkbx.Location.Y);
                Label label = new Label();
                label.Name = "LBL"+ chkbx.Name;
                label.Text = "Enter Quantity";
                label.Location = new Point(QtyTextBox.Location.X, QtyTextBox.Location.Y - 15);
                this.tabPage12.Controls.Add(QtyTextBox);
                this.tabPage12.Controls.Add(label);
            }
            else
            {
                this.tabPage12.Controls.RemoveByKey("TXTBX" + chkbx.Name);
                this.tabPage12.Controls.RemoveByKey("LBL" + chkbx.Name);
            }
        }

        private void AddRelPerBtn_Click(object sender, EventArgs e)
        {
            int indicator = 0;
            if(RPerNumTextBox.Text == "")
            {
                MessageBox.Show("You must enter permission number");
            }
            else if (RPerCComboBox.Text == "")
            {
                MessageBox.Show("You must select client number");
            }
            else if (RPerStComboBox.Text == "")
            {
                MessageBox.Show("You must select store ID");
            }
            else 
            {
                foreach (Control c in this.tabPage12.Controls)
                {
                    if (c is CheckBox)
                    {
                        if (((CheckBox)c).Checked)
                        {
                            indicator = 1;
                        }
                    }
                }
                if (indicator == 0)
                {
                    MessageBox.Show("You must choose at least one item");
                }
                else
                {
                    int count = 0;
                    indicator = 0;
                    foreach (Control c in this.tabPage12.Controls)
                    {
                        if (c is CheckBox)
                        {
                            if (((CheckBox)c).Checked)
                            {
                                TextBox mytextbox = (TextBox)this.tabPage12.Controls.Find("TXTBX" + c.Name, true)[0];
                                if (mytextbox.Text == "")
                                {
                                    MessageBox.Show("You must enter quantity for selected items");
                                    indicator = 0;
                                }
                                else
                                {
                                    indicator = 1;
                                }
                                count++;
                            }
                        }
                    }
                    int[,] ItemValues = new int[count, 3]; //[ ,0]=>ICode [ ,1]=>qty [ ,2]=>SP_Num
                    if ( indicator == 1) 
                    {
                        int added = (int)Ent.Add_Release_Permission(int.Parse(RPerNumTextBox.Text), RPerDateTimePicker.Value, int.Parse(RPerCComboBox.Text), int.Parse(RPerStComboBox.Text)).First();
                        if(added == 1)
                        {
                            int j = 0;
                            foreach (Control c in this.tabPage12.Controls)
                            {
                                if (c is CheckBox)
                                {
                                    if (((CheckBox)c).Checked)
                                    {
                                        TextBox mytextbox = (TextBox)this.tabPage12.Controls.Find("TXTBX" + c.Name, true)[0];
                                        var Items = Ent.Store_Details(int.Parse(RPerStComboBox.Text));
                                        int i = 1;
                                        foreach (var Item in Items)
                                        {
                                            if (i == int.Parse(c.Name))
                                            {
                                                if(int.Parse(mytextbox.Text) > Item.CurrentQuantity)
                                                {
                                                    MessageBox.Show("This Quantity is not available in our store");
                                                    mytextbox.Text = Item.CurrentQuantity.ToString();
                                                }
                                                ItemValues[j, 0] = Item.Item_Code;
                                                ItemValues[j, 1] = int.Parse(mytextbox.Text);
                                                ItemValues[j, 2] = Item.SP_Num;
                                                j++;
                                            }
                                            i++;
                                        }
                                    }
                                }
                            }
                            for(int l = 0; l < count; l++)
                            {
                                Ent.Add_Release_Permission_Item(int.Parse(RPerNumTextBox.Text), ItemValues[l, 0], ItemValues[l, 1], ItemValues[l, 2]);
                            }
                            MessageBox.Show("Release Permission was added successfully");
                            ReleasePermGridView.DataSource = Ent.Display_Release_Permissions(null).Select(RP => new
                            {
                                Permission_Num = RP.Permission_Num,
                                Permission_Date = RP.Permission_Date,
                                Store_Name = RP.Store_Name,
                                Client_Name = RP.Client_Name,
                                Number_Of_Items = Ent.Display_Release_Permission_Items(RP.Permission_Num).First().Number_Of_Items
                            }).ToList();
                            RelPerComboBox.Items.Add(RPerNumTextBox.Text);
                            ItemsGridView.Visible = false;
                            RPerClrBtn_Click(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("Duplicated number" );
                        }
                    }
                }
            }
        }
        private void RelPerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddRelpIBtn.Visible = true;
            RelPerItemsListBox.Items.Clear();
            var ReleasePermission = Ent.Display_Release_Permissions(int.Parse(RelPerComboBox.Text)).First();
            RPerDate.Value = (DateTime)ReleasePermission.Permission_Date;
            RelPerStComboBox.Items.Add(ReleasePermission.Store_ID.ToString());
            RelPerStComboBox.Text = ReleasePermission.Store_ID.ToString();
            RelPerCComboBox.Text = ReleasePermission.Client_ID.ToString();
            var ReleasePermissionItems = Ent.Display_Release_Permission_Items(int.Parse(RelPerComboBox.Text));
            foreach (var Item in ReleasePermissionItems)
            {
                RelPerItemsListBox.Items.Add(Item.Item_Code);
            }
        }
        private void RelPerItemsListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.RelPerItemsListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                EditReleasedItem EditItem = new EditReleasedItem();
                EditItem.PerNumber = int.Parse(RelPerComboBox.Text);
                EditItem.ItemCode = int.Parse(RelPerItemsListBox.SelectedItem.ToString());
                EditItem.StoreID = int.Parse(RelPerStComboBox.Text);
                DialogResult Conf;
                Conf = EditItem.ShowDialog();
                if (Conf == DialogResult.OK)
                {
                    MessageBox.Show("Updated Successfully");
                    ReleasePerItemsGridView.Visible = false;
                    ItemsGridView.Visible = false;
                }
            }
        }
        private void EditRelPerBtn_Click(object sender, EventArgs e)
        {
            Ent.Edit_Supply_Permission(int.Parse(SupPerComboBox.Text), SPerDate.Value, int.Parse(SupPerSComboBox.Text), int.Parse(SupPerStComboBox.Text));
            MessageBox.Show("Supply permission was updated successfully");
            SupplyPermGridView.DataSource = Ent.Display_Supply_Permissions(null).Select(SP => new
            {
                Permission_Num = SP.Permission_Num,
                Permission_Date = SP.Permission_Date,
                Store_Name = SP.Store_Name,
                Supplier_Name = SP.Supplier_Name,
                Number_Of_Items = Ent.Display_Supply_Permission_Items(SP.Permission_Num).First().Number_Of_Items
            }).ToList();
            ItemsGridView.Visible = false;
        }
        private void AddRelpIBtn_Click(object sender, EventArgs e)
        {
            AddReleasedItem NewItem = new AddReleasedItem();
            NewItem.StoreID = int.Parse(RelPerStComboBox.Text);
            NewItem.PermissionNumber = int.Parse(RelPerComboBox.Text);
            DialogResult Conf;
            Conf = NewItem.ShowDialog();
            if (Conf == DialogResult.OK)
            {
                MessageBox.Show("Added Successfully");
                ItemsGridView.Visible = false;
                RelPerItemsListBox.Items.Add(NewItem.ItemCode.ToString());
                ReleasePerItemsGridView.Visible = false;
                ItemsGridView.Visible = false;
            }
        }
        private void RPerClrBtn_Click(object sender, EventArgs e)
        {
            RPerNumTextBox.Text = RPerStComboBox.Text = RPerCComboBox.Text = string.Empty;
            RPerDateTimePicker.ResetText();
            for (int j = 1; j <= NoOfCheckboxControls; j++)
            {
                this.tabPage12.Controls.RemoveByKey(j.ToString());
            }
            label50.Visible = false;
        }
        private void ItemsGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex == 3)
            {
                int ItemId = int.Parse(ItemsGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
                int StoreId = int.Parse(StoreComboBox.Text);
                MoveItems moveItems = new MoveItems();
                moveItems.StoreID = StoreId;
                moveItems.ItemCode = ItemId;
                DialogResult Conf;
                Conf = moveItems.ShowDialog();
                if (Conf == DialogResult.OK)
                {
                    MessageBox.Show("Moved Successfully");
                    ItemsGridView.Visible = false;
                    RPerClrBtn_Click(sender, e);
                }
            }
        }
        private void DetailsForm_Resize(object sender, EventArgs e)
        {
            tabControl1.Size = this.Size;
            tabControl2.Size = this.Size;
            tabControl3.Size = this.Size;
        }
        private void FirstPgBtn_Click(object sender, EventArgs e)
        {
            Company_Stores FirstForm = new Company_Stores();
            FirstForm.Show();
            this.Hide();
        }

    }
}
