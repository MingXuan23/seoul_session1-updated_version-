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
    public partial class Create : Form
    {
        seoul_session1Entities ent=new seoul_session1Entities();
        int first = 0;
        User user = new User();
        public Create()
        {
            InitializeComponent();
            
        }

        public void input()
        {
            user.ID = ent.Users.Max(x => x.ID) + 1;
            user.GUID = Guid.NewGuid();
            user.UserTypeID = 2;
            user.BirthDate = dateTimePicker1.MinDate;
            user.FamilyCount = 1;
            textBox1.DataBindings.Add("Text", user, "Username");
            textBox2.DataBindings.Add("Text", user, "FullName");
            textBox3.DataBindings.Add("Text", user, "Password");
            dateTimePicker1.DataBindings.Add("Value", user, "BirthDate");
            radioButton2.DataBindings.Add("Checked", user, "Gender");
            numericUpDown1.DataBindings.Add("Value", user, "FamilyCount");

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            var form = Application.OpenForms["Form1"];
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(Control control in groupBox1.Controls)
            {
                if(control is TextBox)
                {
                    TextBox tb=control as TextBox;
                    if(tb.Text=="")
                    {
                        MessageBox.Show("Please fill all field");
                        return;
                    }
                }
                else if(control is CheckBox)
                {
                    CheckBox cb=control as CheckBox;
                    if(!cb.Checked)
                    {
                        MessageBox.Show("Please read the T&C first");
                        return;
                    }
                }
                else if (control is DateTimePicker)
                {
                    DateTimePicker dp = control as DateTimePicker;
                    if (dp.Value==dp.MinDate ||dp.Value>DateTime.Now)
                    {
                        MessageBox.Show("Please select valid birthday");
                        return;
                    }
                }
               
            }
            if(textBox3.Text!=textBox4.Text || textBox3.Text.Length<5)
            {
                MessageBox.Show("The password must be same and longer than 5 character");
                return;
            }
            if(!radioButton1.Checked&&!radioButton2.Checked)
            {
                MessageBox.Show("Please select your gender");
                return;
            }

           /* User user = new User();
            user.ID = ent.Users.Max(x => x.ID) +1;
            user.GUID=Guid.NewGuid();
            user.UserTypeID = 2;
            user.Username = textBox1.Text;
            user.FullName = textBox2.Text;
            user.Password= textBox3.Text;
            user.Gender = radioButton2.Checked;
            user.BirthDate = dateTimePicker1.Value;
            user.FamilyCount = (int)numericUpDown1.Value;*/
            ent.Users.Add(user);
            ent.SaveChanges();
            this.Hide();
            var form = Application.OpenForms["Management"];
            if (form == null)
            {
                new Management(user, 2).ShowDialog();
            }
            else
            {
                form.Show();
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StreamReader reader =new StreamReader("C:/Users/User/source/repos/session1_3.0/session1_3.0/Terms.txt");
            string term=reader.ReadToEnd();
            MessageBox.Show(term);
            first++;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked &&first==0)
            {
                checkBox1.Checked = false;
                MessageBox.Show("Please read T&C first");
            }
        }

        private void Create_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.MinDate;
            input();
        }
    }
}
