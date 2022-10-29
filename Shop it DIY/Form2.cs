using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shop_it_DIY
{
    public partial class Form2 : Form
    {
        APD65_63011212052Entities context = new APD65_63011212052Entities();
        Form1 form1;
        Form3 form3;
        Form4 form4;
        int id;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            try
            {
                if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter your username and password ", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var data = context.Semployees.Where(s => s.Em_username == username && s.Em_pssword == password).Select(s => s.Em_status).First();
                    this.id = context.Semployees.Where(s => s.Em_username == username && s.Em_pssword == password).Select(s => s.Em_ID).First();
                    Console.WriteLine(data + " " + this.id);
                    if (data == 1)
                    {
                        try
                        {

                            this.Visible = false;
                            form1.Visible = true;
                        }
                        catch
                        {
                            this.form1 = new Form1(this);
                            form1.Visible = true;
                            this.Visible = false;
                        }

                    }
                    else if (data == 2)
                    {
                        try
                        {
                            this.form3 = new Form3(this, this.id);
                            form3.Visible = true;
                            this.Visible = false;
                        }
                        catch
                        {
                            this.form3 = new Form3(this, this.id);
                            form3.Visible = true;
                            this.Visible = false;
                        }

                    }
                    else if (data == 3)
                    {
                        try
                        {
                            form4.Visible = true;
                            this.Visible = false;
                        }
                        catch
                        {
                            this.form4 = new Form4(this);
                            form4.Visible = true;
                            this.Visible = false;
                        }

                    }
                }

            }
            catch
            {
                MessageBox.Show("Invalid password or invalid username.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.form1 = new Form1(this);
            this.form3 = new Form3(this, this.id);
            this.form4 = new Form4(this);
        }

        private void Form2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
