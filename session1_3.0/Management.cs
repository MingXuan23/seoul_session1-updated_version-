using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace session1_3._0
{
    public partial class Management : Form
    {
        User user { get; set; }
        int role { get; set; }
        seoul_session1Entities ent=new seoul_session1Entities();
        public Management()
        {
            InitializeComponent();
        }

        public Management(User user ,int role)
        {
            InitializeComponent();
            this.user = user;
            this.role = role;
        }

        private void Management_Load(object sender, EventArgs e)
        {
            if(role==1)
            {
                tabControl1.SelectedTab = tabPage2;
            }

            showItem();
        }

        public void showItem()
        {
            ent = new seoul_session1Entities();
            dataGridView2.Rows.Clear();
            var item = ent.Items.Where(x => x.UserID == user.ID).ToList();
            foreach (var i in item)
            {
                var row = new object[6];
                row[0] = i.ID;
                row[1] = i.Title;
                row[2] = i.Capacity;
                row[3] = i.Area.Name;
                row[4] = i.ItemType.Name;
                row[5] = "Edit Details";
                dataGridView2.Rows.Add(row);
            }
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Index % 2 == 0)
                    row.DefaultCellStyle.BackColor = Color.LightGray;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter writter = new StreamWriter("record.txt");
            writter.Write("");
            writter.Close();
            this.Hide();
            var form = Application.OpenForms["Form1"];
            if (form == null)
            {
                new Form1().ShowDialog();
            }
            else
            {
                form.Show();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword=textBox1.Text.ToLower();

            var item=ent.Items.Where(x=>x.Title.ToLower().Contains(keyword)).ToList();
            var temp = ent.Items.Where(x => x.Area.Name.ToLower().Contains(keyword)).ToList();
            item = item.Union(temp).ToList();

            temp= ent.ItemAttractions.Where(x => x.Attraction.Name.ToLower().Contains(keyword) && x.Distance <= 1).Select(x=>x.Item).ToList();
            item = item.Union(temp).ToList();

            dataGridView1.Rows.Clear();
            foreach(var i in item)
            {
                var row = new object[5];
                row[0] = i.ID;
                row[1] = i.Title;
                row[2] = i.Capacity;
                row[3] = i.Area.Name;
                row[4] = i.ItemType.Name;
                dataGridView1.Rows.Add(row);
            }

            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Index % 2 == 0)
                    row.DefaultCellStyle.BackColor = Color.LightGray;
            }


        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(role==1 &&tabControl1.SelectedTab==tabPage1)
                tabControl1.SelectedTab=tabPage2;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex!=5)
            {
                return;
            }
            var id = int.Parse(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());
            var item = ent.Items.FirstOrDefault(x => x.ID == id);
            this.Hide();
            new AddEdit(item,user).ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AddEdit(null, user).ShowDialog();
        }

        private void Management_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                showItem();
        }
    }
}
