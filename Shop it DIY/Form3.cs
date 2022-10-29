using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using System.Net;
using AForge.Controls;
using ZXing;
using System.Security.Cryptography;
using Windows.Globalization.DateTimeFormatting;
using System.Security.Policy;

namespace Shop_it_DIY
{
    public partial class Form3 : Form
    {
        APD65_63011212052Entities context = new APD65_63011212052Entities();
        Form2 form2;
        int id;
        int nid;
        int Sindex;
        int pm;
        double prosum;
        FilterInfoCollection webcams;
        VideoCaptureDevice videoIn;
        public Form3(Form2 form2, int id)
        {
            this.id = id;
            this.form2 = form2;
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (this.form2 != null)
            {
                webcams = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach (FilterInfo webcam in webcams)
                {
                    comboBox1.Items.Add(webcam.Name);
                }
            }
            var typePd = context.View_1.ToList();
            foreach (var item in typePd)
            {
                comboBox2.Items.Add(item.Expr1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1 != null)
                {
                    videoIn = new VideoCaptureDevice(webcams[comboBox1.SelectedIndex].MonikerString);
                    videoSourcePlayer1.VideoSource = videoIn;
                    videoSourcePlayer1.Start();
                }

            }
            catch
            {
                MessageBox.Show("Select your camera.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private double calculateTotal(ListView.ListViewItemCollection items)
        {
            double sum = 0;
            foreach (ListViewItem item in items)
            {
                sum += double.Parse(item.SubItems[6].Text);
            }
            return sum;
        }
        private int chackPromotion(ListView.ListViewItemCollection items)
        {
            int i = 0;
            foreach (ListViewItem item in items)
            {
                string id = item.SubItems[0].Text;
                if (!string.IsNullOrEmpty(id))
                {
                    try
                    {
                        this.prosum = 0;
                        var c = context.Spromotions.ToList();
                        foreach (var it in c)
                        {
                            if (id == it.Pm_PfirstID || id == it.Pm_PsecondID)
                            {
                                i++;
                                this.Sindex = it.Pm_ID;
                            }
                        }
                        if (i == 2)
                        {
                            double f1 = double.Parse(context.Spromotions.Where(s => s.Pm_ID == this.Sindex).Select(s => s.Pm_Pfirst).First());
                            double f2 = double.Parse(context.Spromotions.Where(s => s.Pm_ID == this.Sindex).Select(s => s.Pm_Psecond).First());
                            this.prosum = f1 + f2;
                            Console.WriteLine(this.prosum);
                            //MessageBox.Show("Have item " + i + " in promotion", "Promotion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch { }
                }
            }
            return i;
        }
        private void AddOrder(string id)
        {
            try
            {
                Console.WriteLine(id);
                var data = context.Sproducts.Where(s => s.Pd_ID == id).First();

                string[] item = new string[]
                {
                        data.Pd_ID.ToString(),
                        data.Pd_name.ToString(),
                        data.Pd_detail.ToString(),
                        data.Pd_type.ToString(),
                        data.Pd_unitprice.ToString(),
                        numericUpDown1.Value.ToString(),
                        (data.Pd_unitprice * numericUpDown1.Value).ToString()
                };
                numericUpDown1.Value = 1;
                listView1.Items.Add(new ListViewItem(item));
                double sum = calculateTotal(listView1.Items);
                this.pm = chackPromotion(listView1.Items);
                double sumed = sum;
                label33.Text = decimal.Parse(sumed.ToString()).ToString();
            }
            catch
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var image = videoSourcePlayer1.GetCurrentVideoFrame();

            if (image != null)
            {
                BarcodeReader reader = new BarcodeReader();
                var result = reader.Decode(image);
                if (result != null)
                {
                    textBox1.Text = result.Text;
                    Console.Beep(3000, 500);
                    string id = textBox1.Text;
                    if (id != null)
                    {
                        AddOrder(id);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (videoIn != null && videoIn.IsRunning)
            {
                videoSourcePlayer1.Stop();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            AddOrder(id);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string customer = "";

            try
            {

                string ph = textBox16.Text;
                Console.WriteLine(ph);
                if (!string.IsNullOrEmpty(textBox17.Text))
                {
                    int idc = int.Parse(textBox17.Text);
                    customer = context.Scustomers.Where(s => s.Cm_id == idc).Select(p => p.Cm_firstname).First();
                    this.nid = idc;
                }
                else
                {
                    Console.WriteLine(ph);
                    customer = context.Scustomers.Where(s => s.Cm_phone == ph).Select(s => s.Cm_firstname).First();
                    this.nid = context.Scustomers.Where(s => s.Cm_phone == ph).Select(s => s.Cm_id).First();
                }
                label26.Text = customer;
            }
            catch
            {
                MessageBox.Show("Customer information not found.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label26.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem itemSelected in listView1.SelectedItems)

                {
                    string s = label33.Text;
                    listView1.Items.Remove(itemSelected);
                    double sum = double.Parse(s) - double.Parse(itemSelected.SubItems[6].Text);
                    label33.Text = sum + ".00";
                }
            }
            catch { }
        }
        private const string DateFormat = "MM-dd-yyyy";
        private void button16_Click(object sender, EventArgs e)
        {
            if (this.pm == 2)
            {
                MessageBox.Show("Have item " + 2 + " in promotion", "Promotion", MessageBoxButtons.OK, MessageBoxIcon.None);
                double sum = double.Parse(label33.Text);
                double sumed = sum;
                sum = 0.1 * this.prosum;
                sumed = sumed - sum;
                MessageBox.Show("Get a discount " + sum + " ฿", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                label32.Text = "*ลด " + sum;
                label33.Text = decimal.Parse(sumed.ToString()).ToString();
            }

            var ci = new CultureInfo("th-TH");
            if (nid == null)
            {
                MessageBox.Show("Please enter customer information.", "DATA ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox17.Focus();
            }
            try
            {
                var random = new Random(GetSeed());
                //Console.WriteLine(random.Next());
                SOrder order = new SOrder();

                order.Or_date = DateTime.Now;
                Console.WriteLine(order.Or_date);

                order.Or_ornum = "OR-" + random.Next();
                Console.WriteLine(order.Or_ornum);

                order.Or_EmID = id;
                Console.WriteLine(order.Or_EmID);

                order.Or_CmID = nid;
                Console.WriteLine(order.Or_CmID);

                if (Sindex != null && Sindex != 0)
                {
                    order.Or_Pro = Sindex;
                    Console.WriteLine(order.Or_Pro);
                }

                order.Or_totalamount = decimal.Parse(label33.Text);
                Console.WriteLine(order.Or_totalamount);

                context.SOrders.Add(order);
                int change = context.SaveChanges();
                MessageBox.Show("Change: " + change + " records");

                if (change == 1)
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        SOrderItem orderItem = new SOrderItem();
                        orderItem.It_OrderID = int.Parse(order.Or_ID.ToString());
                        Console.WriteLine(orderItem.It_OrderID);


                        orderItem.It_productID = item.SubItems[0].Text;
                        Console.WriteLine(orderItem.It_productID);

                        orderItem.It_productNID = context.Sproducts.Where(s => s.Pd_ID == orderItem.It_productID).Select(s => s.Pd_nID).First();
                        Console.WriteLine(orderItem.It_productNID);

                        orderItem.It_unitprice = decimal.Parse(item.SubItems[4].Text);
                        Console.WriteLine(orderItem.It_unitprice);

                        orderItem.It_Quantity = int.Parse(item.SubItems[5].Text);
                        Console.WriteLine(orderItem.It_Quantity);

                        int stock = context.Sproducts.Where(s => s.Pd_nID == orderItem.It_productNID)
                            .Select(s => s.Pd_stock).First();
                        int nowStock = stock - orderItem.It_Quantity;
                        Console.WriteLine("= " + nowStock);
                        context.Sproducts.Where(s => s.Pd_ID == orderItem.It_productID).First()
                            .Pd_stock = nowStock;

                        context.SOrderItems.Add(orderItem);
                        context.SaveChanges();


                    }
                    MessageBox.Show("Save success");
                }
                label32.Text = "";
                listView1.Items.Clear();
                label12.Text = "0.00";
            }
            catch
            {
                MessageBox.Show("Save ERROR", "DATA ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetSeed()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var intBytes = new byte[4];
                rng.GetBytes(intBytes);
                return BitConverter.ToInt32(intBytes, 0);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_Click(object sender, EventArgs e)
        {
            try
            {
                string id;
                foreach (ListViewItem itemSelected in listView1.SelectedItems)

                {
                    id = itemSelected.SubItems[0].Text;
                    sproductBindingSource.DataSource = context.Sproducts.Where(s => s.Pd_ID == id).First();
                    pictureBox2.Image = LoadImage(context.Sproducts.Where(s => s.Pd_ID == id).Select(p => p.Pd_img).First());
                    numericUpDown2.Value = int.Parse(itemSelected.SubItems[5].Text);
                }
            }
            catch { }
        }

        private Image LoadImage(string url)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            myHttpWebRequest.UserAgent = "Chrome/105.0.0.0";
            Bitmap bmp = null;
            HttpWebResponse myHttpWebResponse = null;
            try
            {
                myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream streamResponse = myHttpWebResponse.GetResponseStream();
                Console.WriteLine("Error code: {0}", myHttpWebResponse.StatusCode);

                bmp = new Bitmap(streamResponse);
                if (streamResponse == null)
                {
                    bmp = null;
                }

                return bmp;
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                return bmp = null;
            }
            return bmp;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            sOrderBindingSource.DataSource = context.SOrders.ToList();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView3.SelectedRows[0].Cells[0].Value.ToString());
            Console.WriteLine("-" + id);
            sOrderItemBindingSource.DataSource = context.SOrderItems.Where(s => s.It_OrderID == id).ToList();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView3.SelectedRows[0].Cells[0].Value.ToString());
            Console.WriteLine("-" + id);
            sOrderItemBindingSource.DataSource = context.SOrderItems.Where(s => s.It_OrderID == id).ToList();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.form2.Visible = true;
            videoSourcePlayer1.Stop();
            this.Visible = false;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem itemSelected in listView1.SelectedItems)

                {
                    if (numericUpDown2.Value > 0)
                    {
                        itemSelected.SubItems[5].Text = numericUpDown2.Value.ToString();
                        itemSelected.SubItems[6].Text = (double.Parse(itemSelected.SubItems[4].Text) * double.Parse(itemSelected.SubItems[5].Text)) + ".00";
                    }
                    else
                    {
                        MessageBox.Show("Quantity not less than 1.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        numericUpDown2.Value = 1;
                    }

                }
                double sum = calculateTotal(listView1.Items);
                label33.Text = sum.ToString();
            }
            catch { }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox10.Text))
                {
                    string name = textBox10.Text;
                    sproductBindingSource.DataSource = context.Sproducts.Where(s => s.Pd_name == name || s.Pd_name.Contains(name)).ToList();
                }
                else
                {
                    sproductBindingSource.DataSource = context.Sproducts.ToList();
                }
                if (!string.IsNullOrEmpty(textBox9.Text))
                {
                    string detail = textBox9.Text;
                    sproductBindingSource.DataSource = context.Sproducts.Where(s => s.Pd_detail == detail || s.Pd_detail.Contains(detail)).ToList();
                }
                else
                {
                    sproductBindingSource.DataSource = context.Sproducts.ToList();
                }
                if (!string.IsNullOrEmpty(comboBox2.Text))
                {
                    string type = comboBox2.Text;
                    sproductBindingSource.DataSource = context.Sproducts.Where(s => s.Pd_type == type || s.Pd_detail.Contains(type)).ToList();
                }
                else
                {
                    sproductBindingSource.DataSource = context.Sproducts.ToList();
                }

            }
            catch
            {

            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string f = comboBox2.SelectedItem.ToString();
            bindingSource2.DataSource = context.Sproducts.Where(s => s.Pd_type == f).ToList();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            scustomerBindingSource.DataSource = context.Scustomers.ToList();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Scustomer customer = new Scustomer();
            customer.Cm_firstname = textBox15.Text;
            customer.Cm_lastname = textBox14.Text;
            customer.Cm_phone = textBox13.Text;
            customer.Cm_email = textBox12.Text;

            context.Scustomers.Add(customer);
            int change = context.SaveChanges();
            MessageBox.Show("Change: " + change + " records");

            scustomerBindingSource.DataSource = context.Scustomers.ToList();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int change = context.SaveChanges();
            MessageBox.Show("Change on " + change + " records");
            scustomerBindingSource.DataSource = context.Scustomers.ToList();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            scustomerBindingSource.AddNew();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox11.Text);

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

        private void button15_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string name = textBox19.Text;
                int idc = context.Scustomers.Where(s => s.Cm_firstname.Contains(name)).Select(s => s.Cm_id).First();
                bindingSource1.DataSource = context.SOrders.Where(s => s.Or_CmID == idc).ToList();
            }
            catch
            {

            }
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string phone = textBox18.Text;
                int idc = context.Scustomers.Where(s => s.Cm_phone.Contains(phone)).Select(s => s.Cm_id).First();
                bindingSource1.DataSource = context.SOrders.Where(s => s.Or_CmID == idc).ToList();
            }
            catch
            {

            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(textBox8.Text);
                bindingSource1.DataSource = context.SOrders.Where(s => s.Or_CmID.Equals(id)).ToList();
            }
            catch
            {

            }
        }

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = int.Parse(dataGridView6.SelectedRows[0].Cells[0].Value.ToString());
                bindingSource3.DataSource = context.SOrderItems.Where(s => s.It_OrderID == id).ToList();
            }
            catch
            {

            }
        }

        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int id = int.Parse(dataGridView6.SelectedRows[0].Cells[0].Value.ToString());
                bindingSource3.DataSource = context.SOrderItems.Where(s => s.It_OrderID == id).ToList();
            }
            catch
            {

            }
        }
    }
    internal class ComboBoxItem
    {
        public string Value { get; set; } //Supplier ID
        public string Text { get; set; } //Company Name
        public ComboBoxItem(string text)
        {
            Text = text;
        }
        public override string ToString()
        {
            return Text;
        }
    }
}
