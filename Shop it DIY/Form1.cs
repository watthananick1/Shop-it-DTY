using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shop_it_DIY
{
    public partial class Form1 : Form
    {
        APD65_63011212052Entities context = new APD65_63011212052Entities();
        Sproduct product = new Sproduct();

        int qc = 0;
        Form2 form2;
        decimal sumprice = 0;
        public Form1(Form2 form2)
        {
            this.form2 = form2;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var results = context.Sstatus.OrderBy(s => s.Status_id).Select(s => new { s.Status_id, s.Status_name });
            foreach (var result in results)
            {
                comboBox1.Items.Add(new ComboBoxItem(result.Status_id.ToString(),
                    result.Status_name));
            }
            var typePd = context.View_1.ToList();
            foreach (var item in typePd)
            {
                comboBox2.Items.Add(item.Expr1);
            }
        }
        internal class ComboBoxItem
        {
            public string Value { get; set; } //Supplier ID
            public string Text { get; set; } //Company Name
            public ComboBoxItem(string val, string text)
            {
                Value = val;
                Text = text;
            }
            public override string ToString()
            {
                return Text;
            }
        }

        private const string DateFormat = "dd-MM-yyyy";
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try 
            {
                bindingSource2.Clear();
                this.sumprice = 0;
                string d2 = " 23:59:59";
                var ci = new CultureInfo("th-TH");
                var today = Convert.ToDateTime(dateTimePicker1.Value.Date
                    .ToString(DateFormat, ci));
                var todayend = Convert.ToDateTime(dateTimePicker1.Value.Date
                    .ToString(DateFormat, ci) + d2);
                Console.WriteLine(today.ToString());
                Console.WriteLine(todayend.ToString());
                var items = context.SOrders.ToList();
                
                foreach (var item in items) 
                {
                    if (item.Or_date >= today) 
                    {
                        if (item.Or_date < todayend) 
                        {
                            Console.WriteLine(item.Or_date.ToString());
                            bindingSource2.Add(item);
                            this.sumprice += item.Or_totalamount;
                        }
                    }
                }
                label27.Text = this.sumprice.ToString();
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.form2.Visible = true;
            this.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            scustomerBindingSource.DataSource = context.Scustomers.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            scustomerBindingSource.AddNew();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Scustomer customer = new Scustomer();
            customer.Cm_firstname = textBox9.Text;
            customer.Cm_lastname = textBox10.Text;
            customer.Cm_phone = textBox11.Text;
            customer.Cm_email = textBox12.Text;

            context.Scustomers.Add(customer);
            int change = context.SaveChanges();
            MessageBox.Show("Change: " + change + " records");

            scustomerBindingSource.DataSource = context.Scustomers.ToList();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int change = context.SaveChanges();
            MessageBox.Show("Change on " + change + " records");
            scustomerBindingSource.DataSource = context.Scustomers.ToList();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox4.Text);

            var result = context.Scustomers
                .Where(p => p.Cm_id == id)
                .First();

            context.Scustomers.Remove(result);
            int change = context.SaveChanges();
            if (change > 0)
            {
                MessageBox.Show("Delete success");
            }
            else
            {
                MessageBox.Show("Delete failed");
            }

            scustomerBindingSource.DataSource = context.Scustomers.ToList();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            semployeeBindingSource.DataSource = context.Semployees.Where(s => s.Em_status > 1).ToList();
            int id = int.Parse(dataGridView2.SelectedRows[0].Cells[0].Value.ToString());
            int statusid = context.Semployees.Where(s => s.Em_ID == id).Select(p => p.Em_status).First();
            comboBox1.Text = context.Sstatus.Where(s => s.Status_id == statusid).Select(p => p.Status_name).First();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            semployeeBindingSource.AddNew();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Semployee semployee = new Semployee();
            semployee.Em_firstname = textBox20.Text;
            semployee.Em_lastname = textBox19.Text;
            semployee.Em_phone = textBox18.Text;
            semployee.Em_email = textBox17.Text;
            semployee.Em_status = int.Parse(((ComboBoxItem)(comboBox1.SelectedItem)).Value);
            semployee.Em_username = textBox1.Text;
            semployee.Em_pssword = textBox2.Text;

            context.Semployees.Add(semployee);
            int change = context.SaveChanges();
            MessageBox.Show("Change: " + change + " records");

            semployeeBindingSource.DataSource = context.Semployees.Where(s => s.Em_status > 1).ToList();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = Image.FromFile(openFileDialog1.FileName);
                semployeeBindingSource.EndEdit();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int change = context.SaveChanges();
            MessageBox.Show("Change on " + change + " records");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox21.Text);

            var result = context.Semployees.Where(p => p.Em_ID == id).First();

            context.Semployees.Remove(result);
            int change = context.SaveChanges();
            if (change > 0)
            {
                MessageBox.Show("Delete success");
            }
            else
            {
                MessageBox.Show("Delete failed");
            }

            scustomerBindingSource.DataSource = context.Scustomers.ToList();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string f = comboBox2.SelectedItem.ToString();
            bindingSource1.DataSource = context.Sproducts.Where(s => s.Pd_type == f).ToList();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox8.Text))
                {
                    string name = textBox8.Text;
                    bindingSource1.DataSource = context.Sproducts.Where(s => s.Pd_name == name || s.Pd_name.Contains(name)).ToList();
                }
                else
                {
                    bindingSource1.DataSource = context.Sproducts.ToList();
                }
                if (!string.IsNullOrEmpty(textBox3.Text))
                {
                    string detail = textBox3.Text;
                    bindingSource1.DataSource = context.Sproducts.Where(s => s.Pd_detail == detail || s.Pd_detail.Contains(detail)).ToList();
                }
                else
                {
                    bindingSource1.DataSource = context.Sproducts.ToList();
                }
                if (!string.IsNullOrEmpty(comboBox2.Text))
                {
                    string type = comboBox2.Text;
                    bindingSource1.DataSource = context.Sproducts.Where(s => s.Pd_type == type || s.Pd_detail.Contains(type)).ToList();
                }
                else
                {
                    bindingSource1.DataSource = context.Sproducts.ToList();
                }

            }
            catch
            {

            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string name = textBox5.Text;
                int idc = context.Scustomers.Where(s => s.Cm_firstname.Contains(name)).Select(s => s.Cm_id).First();
                sOrderBindingSource.DataSource = context.SOrders.Where(s => s.Or_CmID == idc).ToList();
            }
            catch
            {

            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string phone = textBox6.Text;
                int idc = context.Scustomers.Where(s => s.Cm_phone.Contains(phone)).Select(s => s.Cm_id).First();
                sOrderBindingSource.DataSource = context.SOrders.Where(s => s.Or_CmID == idc).ToList();
            }
            catch
            {

            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(textBox7.Text);
                sOrderBindingSource.DataSource = context.SOrders.Where(s => s.Or_CmID.Equals(id)).ToList();
            }
            catch
            {


            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = int.Parse(dataGridView4.SelectedRows[0].Cells[0].Value.ToString());
                sOrderItemBindingSource.DataSource = context.SOrderItems.Where(s => s.It_OrderID == id).ToList();
            }
            catch
            {

            }
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = int.Parse(dataGridView4.SelectedRows[0].Cells[0].Value.ToString());
                sOrderItemBindingSource.DataSource = context.SOrderItems.Where(s => s.It_OrderID == id).ToList();
            }
            catch
            {

            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                bindingSource2.Clear();
                this.sumprice = 0;
                int day = int.Parse(numericUpDown1.Value.ToString());
                string d2 = " 23:59:59";
                string d1 = " 00:00:00";
                var ci = new CultureInfo("th-TH");
                var today1 = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString(DateFormat, ci) + d1);
                var today = Convert.ToDateTime(today1.AddDays(-day)
                    .ToString(DateFormat, ci) + d1);

                var todayend = Convert.ToDateTime(DateTime.Now
                    .ToString(DateFormat, ci) + d2);
                Console.WriteLine(today.ToString());
                Console.WriteLine(todayend.ToString());
                var items = context.SOrders.ToList();
                foreach (var item in items)
                {
                    string eh;
                    var dt = Convert.ToDateTime(item.Or_date.ToString(DateFormat, ci));
                    Console.WriteLine("** {0}", dt);
                    var td1 = Convert.ToDateTime(today.ToString(DateFormat, ci));
                    var td1end = Convert.ToDateTime(todayend.ToString(DateFormat, ci));
                    Console.WriteLine("-*- {0}", dt);
                    //Console.WriteLine("-- {0}", today);
                    if (item.Or_date >= today)
                    {
                        //Console.WriteLine(eh);
                        if (item.Or_date < todayend)
                        {
                            //eh = item.Or_date.ToString();
                            //Console.WriteLine(-1);
                            //Console.WriteLine("== {0}", dt);
                            bindingSource2.Add(item);
                            this.sumprice += item.Or_totalamount;
                        }

                    }

                }
                label27.Text = this.sumprice.ToString();
            }
            catch
            {

            }
        }
    }
}
