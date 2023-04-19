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
    public partial class Form1 : Form
    {
        seoul_session1Entities ent=new seoul_session1Entities();
        public Form1()
        {
            InitializeComponent();
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new Create().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.UseSystemPasswordChar = !checkBox2.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text =="" &&textBox2.Text =="")
            {
                MessageBox.Show("Please key in username or employee name");
                return;
            }
            if (textBox3.Text == "" )
            {
                MessageBox.Show("Please fill password");
                return;
            }
            string name = "";
            string password = textBox3.Text;
            if (textBox1.Text != "")
            {
                name = textBox1.Text;
                var user=ent.Users.Where(x=>x.Username==name&&x.Password==password&&x.UserTypeID==1).FirstOrDefault();
                if(user==null)
                {
                    MessageBox.Show("This employee did not exist");
                    return;
                }
                else
                {
                    StreamWriter writter = new StreamWriter("record.txt");
                    writter.Write(name);
                    writter.Close();
                }
                this.Hide();
                var form = Application.OpenForms["Management"];
                if(form == null)
                {
                    new Management(user, 1).ShowDialog();
                }
                else
                {
                    form.Show();
                }

            }
            else
            {
                name = textBox2.Text;
                var user = ent.Users.Where(x => x.Username == name && x.Password == password && x.UserTypeID == 2).FirstOrDefault();
                if (user == null)
                {
                    MessageBox.Show("This user did not exist");
                    return;
                }
                else
                {
                    StreamWriter writter = new StreamWriter("record.txt");
                    writter.Write(name);
                    writter.Close();
                }

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

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader("record.txt");
            string name=reader.ReadToEnd();
            reader.Close();
            var user=ent.Users.Where(x => x.Username == name).FirstOrDefault();
            if(user!=null)
            {
                this.Hide();
                var form = Application.OpenForms["Management"];
                if (form == null)
                {
                    new Management(user, (int)user.UserTypeID).ShowDialog();
                }
                else
                {
                    form.Show();
                }
            }
        }
           
    }
}
