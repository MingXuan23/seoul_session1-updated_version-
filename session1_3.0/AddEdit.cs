using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace session1_3._0
{
    public partial class AddEdit : Form
    {
        Item item =new Item();
        User user { get; set; }
        bool add { get; set; }
        seoul_session1Entities ent=new seoul_session1Entities();

        int tab = 0;
        public AddEdit()
        {
            InitializeComponent();
        }
        public AddEdit(Item item,User user)
        {
            InitializeComponent();
            this.user = user;
            if(item!=null)
            {
                this.item = item;
                add = false;
            }
            else
            {
                add = true;
                
              
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        public void samedata()
        {
            item.Capacity = item.NumberOfBeds = item.NumberOfBedrooms = item.NumberOfBathrooms = item.MinimumNights = item.MaximumNights = (int)numericUpDown1.Minimum;
            comboBox1.DataBindings.Add("SelectedValue", item, "ItemTypeID");
            textBox1.DataBindings.Add("Text", item, "Title");
            textBox2.DataBindings.Add("Text", item, "ApproximateAddress");
            textBox3.DataBindings.Add("Text", item, "ExactAddress");
            textBox4.DataBindings.Add("Text", item, "Description");
            textBox5.DataBindings.Add("Text", item, "HostRules");

            numericUpDown1.DataBindings.Add("Value", item, "Capacity");
            numericUpDown2.DataBindings.Add("Value", item, "NumberOfBeds");
            numericUpDown3.DataBindings.Add("Value", item, "NumberOfBedrooms");
            numericUpDown4.DataBindings.Add("Value", item, "NumberOfBathrooms");
            numericUpDown5.DataBindings.Add("Value", item, "MinimumNights");
            numericUpDown6.DataBindings.Add("Value", item, "MaximumNights");

        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(add && tabControl1.SelectedIndex!=tab)
            {
                tabControl1.SelectedIndex =tab;
            }
        }

        private void AddEdit_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'seoul_session1DataSet1.Amenities' table. You can move, or remove it, as needed.
            this.amenitiesTableAdapter.Fill(this.seoul_session1DataSet1.Amenities);
            // TODO: This line of code loads data into the 'seoul_session1DataSet.ItemTypes' table. You can move, or remove it, as needed.
            this.itemTypesTableAdapter.Fill(this.seoul_session1DataSet.ItemTypes);


            var amenity = ent.Amenities.ToList();
            dataGridView1.Rows.Clear();
            foreach (var a in amenity)
            {
                var row = new object[3];
                row[0] = a.ID;
                row[1] = a.Name;
                row[2] = false;
                dataGridView1.Rows.Add(row);
            }

            var att=ent.Attractions.ToList();
            dataGridView2.Rows.Clear();
            foreach (var a in att)
            {
                var row = new object[3];
                row[0] = a.ID;
                row[1] = a.Name;
                row[2] = a.Area.Name;
                dataGridView2.Rows.Add(row);
            }
            samedata();
            if (add)
            {
                button1.Text = "Next";
                button2.Text = "Close";
                

            }
            else
            {
                button1.Visible = false;
                button2.Text = "Finish";

            /*    comboBox1.SelectedValue= item.ItemTypeID;
                textBox1.Text=item.Title ;
                numericUpDown1.Value=item.Capacity ;
                numericUpDown2.Value = item.NumberOfBeds;
                numericUpDown3.Value =item.NumberOfBedrooms;
                numericUpDown4.Value=item.NumberOfBathrooms ;
                textBox2.Text=item.ApproximateAddress  ;
                textBox3.Text=item.ExactAddress  ;
                textBox4.Text =item.Description ;
                textBox5.Text=item.HostRules  ;
                numericUpDown5.Value=item.MinimumNights;
                numericUpDown6.Value=item.MaximumNights ;*/

                var ava=ent.ItemAmenities.Where(x=>x.ItemID==item.ID).Select(x=>x.AmenityID).ToList();
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value == null)
                        continue;
                    var id = long.Parse(row.Cells[0].Value.ToString());
                    if (ava.Contains(id))
                    {
                        row.Cells[2].Value = true;
                        

                    }
                        
                }

                var distance = ent.ItemAttractions.Where(x => x.ItemID == item.ID).ToList(); ;
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[0].Value == null)
                        continue;
                    var id = long.Parse(row.Cells[0].Value.ToString());
                    if (distance.Select(x=>x.AttractionID).ToList().Contains(id))
                    {
                        var dis = distance.FirstOrDefault(x => x.AttractionID == id);
                        if(dis != null)
                        {
                            row.Cells[3].Value = dis.Distance;
                            row.Cells[4].Value = dis.DurationOnFoot; 
                            row.Cells[5].Value = dis.DurationByCar;
                        }
                    }
                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(tab==0)
            {
                foreach(Control c in tabPage1.Controls)
                {
                    if(c is TextBox)
                    {
                        if(c.Text=="")
                        {
                            MessageBox.Show("Please fill all field");
                        }
                    }
                }

              
                item.ID = ent.Items.Max(x => x.ID) + 1;
                item.GUID = Guid.NewGuid();
                item.UserID = user.ID;
                tab++;
                tabControl1.SelectedTab = tabPage2;
            }
            else if(tab ==1)
            {
                var maxId=ent.ItemAmenities.Max(x => x.ID); 
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value == null || row.Cells[2].Value == null)
                        continue;
                 
                    long aid = long.Parse(row.Cells[0].Value.ToString());
                    bool res;
                    if(bool.TryParse(row.Cells[2].Value.ToString(),out res) ==true &&res ==true)
                    {
                        var amm = new ItemAmenity();
                        amm.ID = ++maxId;
                        amm.GUID = Guid.NewGuid();
                        amm.ItemID=item.ID;
                        amm.AmenityID = aid;
                        ent.ItemAmenities.Add(amm);
                    }

                }
                tab++;
                tabControl1.SelectedTab = tabPage3;
                button1.Text = "Finish";
            }
            else if(tab==2)
            {
                var result=MessageBox.Show("Do you sure to save these changes?", "Alert", MessageBoxButtons.YesNo);
                if(result==DialogResult.Yes)
                {
                    int num = 0;
                    var maxId = ent.ItemAttractions.Max(x => x.ID);
                    List<ItemAttraction> itemAttractions = new List<ItemAttraction>();
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        if (row.Cells[0].Value == null)
                            continue;
                        long aid = long.Parse(row.Cells[0].Value.ToString());
                       
                        if (row.Cells[3].Value!=null)
                        {
                            var att = new ItemAttraction();
                            att.ID = ++maxId;
                            att.GUID = Guid.NewGuid();
                            att.ItemID = item.ID;
                            att.AttractionID = aid;
                            att.Distance=decimal.Parse(row.Cells[3].Value.ToString());
                            if(row.Cells[4].Value!=null)
                                att.DurationOnFoot = long.Parse(row.Cells[4].Value.ToString());
                            if (row.Cells[5].Value != null)
                                att.DurationByCar = long.Parse(row.Cells[5].Value.ToString());
                           itemAttractions.Add(att);
                            num++;
                        }
                        

                    }
                    if(num <2)
                    {
                        MessageBox.Show("Please key in at least 2 distance");
                        return;
                    }
                    else
                    {
                        var i = itemAttractions.OrderBy(x => x.Distance).FirstOrDefault();
                        item.AreaID = ent.Attractions.Where(x => x.ID == i.AttractionID).Select(x => x.AreaID).FirstOrDefault();
                        ent.Items.Add(item);
                        ent.ItemAttractions.AddRange(itemAttractions);
                        ent.SaveChanges();

                        this.Close();
                        var form = Application.OpenForms["Management"];
                        form.Show();
                    }
                    tab++;
                }
                else
                {
                    return;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(add)
            {
                var result = MessageBox.Show("Do you sure to cancel these changes?", "Alert", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    this.Close();
                    var form = Application.OpenForms["Management"];
                    form.Show();
                }
            }
            else
            {
               /* item = ent.Items.FirstOrDefault(x => x.ID == item.ID);
                item.ItemTypeID = (long)comboBox1.SelectedValue;
                item.Title = textBox1.Text;
                item.Capacity = int.Parse(numericUpDown1.Value.ToString());
                item.NumberOfBeds = int.Parse(numericUpDown2.Value.ToString());
                item.NumberOfBedrooms = int.Parse(numericUpDown3.Value.ToString());
                item.NumberOfBathrooms = int.Parse(numericUpDown4.Value.ToString());
                item.ApproximateAddress = textBox2.Text;
                item.ExactAddress = textBox3.Text;
                item.Description = textBox4.Text;
                item.HostRules = textBox5.Text;
                item.MinimumNights = int.Parse(numericUpDown5.Value.ToString());
                item.MaximumNights = int.Parse(numericUpDown6.Value.ToString());
*/
                var maxId = ent.ItemAmenities.Max(x => x.ID);
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value == null || row.Cells[2].Value==null)
                        continue;
                    long aid = long.Parse(row.Cells[0].Value.ToString());
                    bool res;
                    if (bool.TryParse(row.Cells[2].Value.ToString(), out res) == true )
                    {
                        if(res==true &&!ent.ItemAmenities.Where(x=>x.ItemID==item.ID &&x.AmenityID==aid).Any())
                        {
                            var amm = new ItemAmenity();
                            amm.ID = ++maxId;
                            amm.GUID = Guid.NewGuid();
                            amm.ItemID = item.ID;
                            amm.AmenityID = aid;
                            ent.ItemAmenities.Add(amm);
                        }
                        else if(res == false && ent.ItemAmenities.Where(x => x.ItemID == item.ID && x.AmenityID == aid).Any())
                        {
                            var amm = ent.ItemAmenities.FirstOrDefault(x => x.ItemID == item.ID && x.AmenityID == aid);
                            ent.ItemAmenities.Remove(amm);
                        }
                    }

                }


                maxId = ent.ItemAttractions.Max(x => x.ID);
                List<ItemAttraction> itemAttractions = new List<ItemAttraction>();
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[0].Value == null)
                        continue;
                    long aid = long.Parse(row.Cells[0].Value.ToString());

                    if (row.Cells[3].Value != null && !ent.ItemAttractions.Where(x=>x.ItemID==item.ID &&x.AttractionID==aid).Any())
                    {
                        var att = new ItemAttraction();
                        att.ID = ++maxId;
                        att.GUID = Guid.NewGuid();
                        att.ItemID = item.ID;
                        att.AttractionID = aid;
                        att.Distance = decimal.Parse(row.Cells[3].Value.ToString());
                        if (row.Cells[4].Value != null)
                            att.DurationOnFoot = long.Parse(row.Cells[4].Value.ToString());
                        if (row.Cells[5].Value != null)
                            att.DurationByCar = long.Parse(row.Cells[5].Value.ToString());
                        itemAttractions.Add(att);
                        
                    }
                    else if(row.Cells[3].Value != null && ent.ItemAttractions.Where(x => x.ItemID == item.ID && x.AttractionID == aid).Any())
                    {
                        var att = ent.ItemAttractions.FirstOrDefault(x=>x.ItemID==item.ID &&x.AttractionID==aid);

                        att.Distance = decimal.Parse(row.Cells[3].Value.ToString());
                        if (row.Cells[4].Value != null)
                            att.DurationOnFoot = long.Parse(row.Cells[4].Value.ToString());
                        if (row.Cells[5].Value != null)
                            att.DurationByCar = long.Parse(row.Cells[5].Value.ToString());
                        
                    }


                }
                if (ent.ItemAttractions.Where(x=>x.ItemID==item.ID).Count()<2)
                {
                    MessageBox.Show("Please key in at least 2 distance");
                    return;
                }
                else
                {
                   
                    ent.ItemAttractions.AddRange(itemAttractions);
                    ent.SaveChanges();

                    this.Close();
                    var form = Application.OpenForms["Management"];
                    form.Show();
                }

            }
        }
    }
}
