using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Shop_it_DIY
{
    public partial class Form4 : Form
    {
        APD65_63011212052Entities context = new APD65_63011212052Entities();
        Form2 form2;
        Sproduct product = new Sproduct();
        int qc = 0;
        string id;
        public Form4(Form2 form2)
        {
            InitializeComponent();
            this.form2 = form2;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            var typePd = context.View_1.ToList();
            foreach (var item in typePd)
            {
                comboBox1.Items.Add(item.Expr1);
                comboBox2.Items.Add(item.Expr1);
                comboBox3.Items.Add(item.Expr1);
            }
            spromotionBindingSource.DataSource = context.Spromotions.ToList();
            //bindingSource1.DataSource = context.Sproducts.ToList();
            //bindingSource2.DataSource = context.Sproducts.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
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
            sproductBindingSource.DataSource = context.Sproducts.Where(s => s.Pd_type == f).ToList();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Spromotion promotion = new Spromotion();

                promotion.Pm_PfirstID = textBox11.Text;
                Console.WriteLine("1 -- " + promotion.Pm_PfirstID);

                promotion.Pm_Pfirst = textBox12.Text;
                Console.WriteLine("1 -- " + promotion.Pm_Pfirst);

                promotion.Pm_PsecondID = textBox14.Text;
                Console.WriteLine("2 -- " + promotion.Pm_PsecondID);

                promotion.Pm_Psecond = textBox13.Text;
                Console.WriteLine("2 -- " + promotion.Pm_Psecond);

                promotion.Pm_rebate = 10;

                context.Spromotions.Add(promotion);
                qc += context.SaveChanges();
            }
            catch
            {

            }
            MessageBox.Show("Save success " + qc + " record");
            spromotionBindingSource.DataSource = context.Spromotions.ToList();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string f = comboBox1.SelectedItem.ToString();
            bindingSource1.DataSource = context.Sproducts.Where(s => s.Pd_type == f).ToList();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string f = comboBox3.SelectedItem.ToString();
            bindingSource2.DataSource = context.Sproducts.Where(s => s.Pd_type == f).ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string id = textBox5.Text;
            listView2.Items.Clear();
            Console.WriteLine(id);
            string url = "https://www.jib.co.th/web/product/readProduct/" + id;
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);
            product.Pd_ID = id;

            Console.WriteLine(product.Pd_name);
            try
            {
                HtmlNode NameNode = doc.DocumentNode
                    .SelectSingleNode("//meta[@property=\"og:title\"]");
                product.Pd_name = NameNode.Attributes["content"].Value;

                HtmlNode descriptionNode = doc.DocumentNode
                    .SelectSingleNode("//meta[@property=\"og:description\"]");
                product.Pd_detail = descriptionNode.Attributes["content"].Value;
                Console.WriteLine(product.Pd_detail);

                string[] data = new string[]
                {
                    product.Pd_ID,
                    product.Pd_name
                };
                listView2.Items.Add(new ListViewItem(data));

            }
            catch (Exception)
            {

            }


        }
        

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.Items)
            {

                string id = item.SubItems[0].Text;
                try
                {

                    string v = context.Sproducts.Where(s => s.Pd_ID == id).Select(s => s.Pd_ID).First();
                    //Console.WriteLine(v);
                    Console.WriteLine("Have data " + v);
                    MessageBox.Show("Have data " + v, " INPUT DATA ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    continue;
                }
                catch (Exception)
                {
                    Console.WriteLine(id);
                    string url = "https://www.jib.co.th/web/product/readProduct/" + id;
                    HtmlWeb web = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument doc = web.Load(url);
                    product.Pd_ID = id;
                    product.Pd_name = item.SubItems[1].Text;
                    Console.WriteLine(product.Pd_name);
                    try
                    {
                        HtmlNode descriptionNode = doc.DocumentNode
                            .SelectSingleNode("//meta[@property=\"og:description\"]");
                        product.Pd_detail = descriptionNode.Attributes["content"].Value;
                        Console.WriteLine(product.Pd_detail);
                    }
                    catch (Exception)
                    {
                        HtmlNode descriptionNode = doc.DocumentNode
                            .SelectSingleNode("//meta[@property=\"og:description\"]");
                        product.Pd_detail = descriptionNode.Attributes["content"].Value;
                        Console.WriteLine(product.Pd_detail);
                    }

                    HtmlNode priceNode = doc.DocumentNode
                        .SelectSingleNode("//div[@class=" +
                        "\"col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center price_block\"]");
                    String price = priceNode.InnerText;
                    price = new string(price.Where(c => char.IsDigit(c)).ToArray());
                    product.Pd_unitprice = decimal.Parse(price + ".00");
                    Console.WriteLine(product.Pd_unitprice);

                    HtmlNode imageNode = doc.DocumentNode
                        .SelectSingleNode("//meta[@property=\"og:image\"]");
                    product.Pd_img = imageNode.Attributes["content"].Value;
                    Console.WriteLine(product.Pd_img);

                    product.Pd_stock = 10;
                    Console.WriteLine(product.Pd_stock);

                    var typeNode = doc.DocumentNode
                        .SelectNodes("//div[@class=\"step_nav\"]//a");
                    product.Pd_type = typeNode[2].InnerText;
                    Console.WriteLine(product.Pd_type);

                    context.Sproducts.Add(product);
                    qc += context.SaveChanges();
                    Console.WriteLine(qc);
                }

            }
            MessageBox.Show("Save success " + qc + " record");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.form2.Visible = true;
            this.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int id = int.Parse(this.id.ToString());

            var result = context.Spromotions
                .Where(p => p.Pm_ID == id).First();

            context.Spromotions.Remove(result);
            int change = context.SaveChanges();
            if (change > 0)
            {
                MessageBox.Show("Delete success");
            }
            else
            {
                MessageBox.Show("Delete failed");
            }

            spromotionBindingSource.DataSource = context.Spromotions.ToList();
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.id = dataGridView4.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.id = dataGridView4.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.Items)
            {

                string id = item.SubItems[0].Text;
                try
                {

                    string v = context.Sproducts.Where(s => s.Pd_ID == id).Select(s => s.Pd_ID).First();
                    //Console.WriteLine(v);
                    Console.WriteLine("Have data " + v);
                    MessageBox.Show("Have data " + v, " INPUT DATA ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    continue;
                }
                catch (Exception)
                {
                    Console.WriteLine(id);
                    string url = "https://www.jib.co.th/web/product/readProduct/" + id;
                    HtmlWeb web = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument doc = web.Load(url);
                    product.Pd_ID = id;
                    product.Pd_name = item.SubItems[1].Text;
                    Console.WriteLine(product.Pd_name);
                    try
                    {
                        HtmlNode descriptionNode = doc.DocumentNode
                            .SelectSingleNode("//meta[@property=\"og:description\"]");
                        product.Pd_detail = descriptionNode.Attributes["content"].Value;
                        Console.WriteLine(product.Pd_detail);
                    }
                    catch (Exception)
                    {
                        HtmlNode descriptionNode = doc.DocumentNode
                            .SelectSingleNode("//meta[@property=\"og:description\"]");
                        product.Pd_detail = descriptionNode.Attributes["content"].Value;
                        Console.WriteLine(product.Pd_detail);
                    }

                    HtmlNode priceNode = doc.DocumentNode
                        .SelectSingleNode("//div[@class=" +
                        "\"col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center price_block\"]");
                    String price = priceNode.InnerText;
                    price = new string(price.Where(c => char.IsDigit(c)).ToArray());
                    product.Pd_unitprice = decimal.Parse(price + ".00");
                    Console.WriteLine(product.Pd_unitprice);

                    HtmlNode imageNode = doc.DocumentNode
                        .SelectSingleNode("//meta[@property=\"og:image\"]");
                    product.Pd_img = imageNode.Attributes["content"].Value;
                    Console.WriteLine(product.Pd_img);

                    if (product.Pd_stock != null)
                    {
                        product.Pd_stock = int.Parse(textBox19.Text);
                        Console.WriteLine(product.Pd_stock);
                    }
                    else
                    {
                        var n = 10;
                        textBox19.Text = n.ToString();
                        product.Pd_stock = int.Parse(textBox19.Text);
                        Console.WriteLine(product.Pd_stock);
                    }

                    var typeNode = doc.DocumentNode
                        .SelectNodes("//div[@class=\"step_nav\"]//a");
                    product.Pd_type = typeNode[2].InnerText;
                    Console.WriteLine(product.Pd_type);

                    context.Sproducts.Add(product);
                    qc += context.SaveChanges();
                    Console.WriteLine(qc);
                }

            }
            MessageBox.Show("Save success " + qc + " record");
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                string url = textBox20.Text;
                HtmlWeb web = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument document = web.Load(url);

                var numOfPageNode = document.DocumentNode
                    .SelectNodes("//div[@class=\"col-md-6 col-sm-6 pad-0\"]//span");
                textBox18.Text = numOfPageNode[1].InnerText;
                string countItem = numOfPageNode[0].InnerText;
                textBox9.Text = new string(countItem.Where(c => char.IsDigit(c)).ToArray());

                var typeOfProductNode = document.DocumentNode.SelectNodes("//span[@class=\"en\"]");
                textBox17.Text = typeOfProductNode[1].InnerText;
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter URL");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                int codepPage = 0;
                int countPage = 0;
                listView2.Items.Clear();
                while (true)
                {
                    string url = textBox20.Text + "/" + codepPage;
                    HtmlWeb web = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument doc = web.Load(url);
                    var titleNode = doc.DocumentNode.SelectNodes("//div[@class=\"cart_modal buy_promo\"]");
                    if (titleNode == null)
                    {
                        break;
                    }
                    foreach (HtmlNode node in titleNode)
                    {
                        string[] data = new string[]
                        {
                        node.Attributes["data-id"].Value,
                        node.Attributes["data-name"].Value
                        };
                        listView2.Items.Add(new ListViewItem(data));
                        countPage++;
                    }
                    Console.WriteLine(countPage);
                    codepPage += 100;
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

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                textBox1.Text = listView2.SelectedItems[0].Text;
                string url = "https://www.jib.co.th/web/product/readProduct/" + textBox1.Text;
                HtmlWeb web = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = web.Load(url);
                HtmlNode titleNode = doc.DocumentNode
                    .SelectSingleNode("//meta[@property=\"og:title\"]");
                textBox2.Text = titleNode.Attributes["content"].Value;

                HtmlNode descriptionNode = doc.DocumentNode
                    .SelectSingleNode("//meta[@property=\"og:description\"]");
                textBox3.Text = descriptionNode.Attributes["content"].Value;

                var typeNode = doc.DocumentNode
                    .SelectNodes("//div[@class=\"step_nav\"]//a");
                textBox7.Text = typeNode[2].InnerText;

                HtmlNode imageNode = doc.DocumentNode
                    .SelectSingleNode("//meta[@property=\"og:image\"]");
                pictureBox1.Image = LoadImage(imageNode.GetAttributeValue("content", ""));

                HtmlNode priceNode = doc.DocumentNode
                    .SelectSingleNode("//div[@class=" +
                    "\"col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center price_block\"]");
                String price = priceNode.InnerText;
                price = new string(price.Where(c => char.IsDigit(c)).ToArray());
                textBox4.Text = price;
            }
            catch (Exception)
            {
                textBox1.Text = listView2.SelectedItems[0].Text;
                string url = "https://www.jib.co.th/web/product/readProduct/" + textBox1.Text;
                HtmlWeb web = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = web.Load(url);
                HtmlNode titleNode = doc.DocumentNode
                    .SelectSingleNode("//meta[@property=\"og:title\"]");
                textBox2.Text = titleNode.Attributes["content"].Value;

                HtmlNode descriptionNode = doc.DocumentNode
                    .SelectSingleNode("//meta[@property=\"og:description\"]");
                textBox3.Text = descriptionNode.Attributes["content"].Value;

                var typeNode = doc.DocumentNode
                    .SelectNodes("//div[@class=\"step_nav\"]//a");
                textBox7.Text = typeNode[1].InnerText;

                HtmlNode imageNode = doc.DocumentNode
                    .SelectSingleNode("//meta[@property=\"og:image\"]");
                pictureBox1.Image = LoadImage(imageNode.GetAttributeValue("content", ""));

                HtmlNode priceNode = doc.DocumentNode
                    .SelectSingleNode("//div[@class=" +
                    "\"col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center price_block\"]");
                String price = priceNode.InnerText;
                price = new string(price.Where(c => char.IsDigit(c)).ToArray());
                textBox4.Text = price;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.Items)
            {

                string id = item.SubItems[0].Text;
                try
                {

                    string v = context.Sproducts.Where(s => s.Pd_ID == id).Select(s => s.Pd_ID).First();
                    //Console.WriteLine(v);
                    Console.WriteLine("Have data " + v);
                    continue;
                }
                catch (Exception)
                {
                    Console.WriteLine(id);
                    string url = "https://www.jib.co.th/web/product/readProduct/" + id;
                    HtmlWeb web = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument doc = web.Load(url);
                    product.Pd_ID = id;
                    product.Pd_name = item.SubItems[1].Text;
                    Console.WriteLine(product.Pd_name);
                    try
                    {
                        HtmlNode descriptionNode = doc.DocumentNode
                            .SelectSingleNode("//meta[@property=\"og:description\"]");
                        product.Pd_detail = descriptionNode.Attributes["content"].Value;
                        Console.WriteLine(product.Pd_detail);
                    }
                    catch (Exception)
                    {
                        HtmlNode descriptionNode = doc.DocumentNode
                            .SelectSingleNode("//meta[@property=\"og:description\"]");
                        product.Pd_detail = descriptionNode.Attributes["content"].Value;
                        Console.WriteLine(product.Pd_detail);
                    }

                    HtmlNode priceNode = doc.DocumentNode
                        .SelectSingleNode("//div[@class=" +
                        "\"col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center price_block\"]");
                    String price = priceNode.InnerText;
                    price = new string(price.Where(c => char.IsDigit(c)).ToArray());
                    product.Pd_unitprice = decimal.Parse(price + ".00");
                    Console.WriteLine(product.Pd_unitprice);

                    HtmlNode imageNode = doc.DocumentNode
                        .SelectSingleNode("//meta[@property=\"og:image\"]");
                    product.Pd_img = imageNode.Attributes["content"].Value;
                    Console.WriteLine(product.Pd_img);
                    try 
                    {
                        if (int.Parse(textBox19.Text) > 0)
                        {
                            product.Pd_stock = int.Parse(textBox19.Text);
                            Console.WriteLine(product.Pd_stock);
                        }
                        else
                        {
                            var n = 10;
                            textBox19.Text = n.ToString();
                            product.Pd_stock = int.Parse(textBox19.Text);
                            Console.WriteLine(product.Pd_stock);
                        }
                    }
                    catch { }



                    var typeNode = doc.DocumentNode
                        .SelectNodes("//div[@class=\"step_nav\"]//a");
                    product.Pd_type = typeNode[2].InnerText;
                    Console.WriteLine(product.Pd_type);

                    context.Sproducts.Add(product);
                    qc += context.SaveChanges();
                    Console.WriteLine(qc);
                }

            }
            MessageBox.Show("Save success " + qc + " record");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            listView2.Items.Clear();
            Console.WriteLine(id);
            string url = "https://www.jib.co.th/web/product/readProduct/" + id;
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);
            product.Pd_ID = id;

            Console.WriteLine(product.Pd_name);
            try
            {
                HtmlNode NameNode = doc.DocumentNode
                    .SelectSingleNode("//meta[@property=\"og:title\"]");
                product.Pd_name = NameNode.Attributes["content"].Value;

                HtmlNode descriptionNode = doc.DocumentNode
                    .SelectSingleNode("//meta[@property=\"og:description\"]");
                product.Pd_detail = descriptionNode.Attributes["content"].Value;
                Console.WriteLine(product.Pd_detail);

                string[] data = new string[]
                {
                    product.Pd_ID,
                    product.Pd_name
                };
                listView2.Items.Add(new ListViewItem(data));

            }
            catch (Exception)
            {

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int change = context.SaveChanges();
            MessageBox.Show("Change on " + change + " records");
            //sproductBindingSource.DataSource = context.Scustomers.ToList();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            int change = context.SaveChanges();
            MessageBox.Show("Change on " + change + " records");
            sproductBindingSource.DataSource = context.Sproducts.ToList();
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string f = comboBox2.SelectedItem.ToString();
            sproductBindingSource.DataSource = context.Sproducts.Where(s => s.Pd_type == f).ToList();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox10.Text))
                {
                    string text = textBox10.Text;
                    sproductBindingSource.DataSource = context.Sproducts.Where(s => s.Pd_ID == text || s.Pd_name == text || s.Pd_detail == text
                    || s.Pd_detail.Contains(text)).First();
                }
                else
                {
                    sproductBindingSource.DataSource = context.Sproducts.ToList();
                }
                //if (!string.IsNullOrEmpty(comboBox2.Text))
                //{
                //    string type = comboBox2.Text;
                //    sproductBindingSource.DataSource = context.Sproducts.Where(s => s.Pd_type == type).ToList();
                //}
                //else
                //{
                //    sproductBindingSource.DataSource = context.Sproducts.ToList();
                //}

            }
            catch
            {

            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var id = textBox11.Text;

            var result = context.Sproducts.Where(p => p.Pd_ID == id).First();

            context.Sproducts.Remove(result);
            int change = context.SaveChanges();
            if (change > 0)
            {
                MessageBox.Show("Delete success");
            }
            else
            {
                MessageBox.Show("Delete failed");
            }

            sproductBindingSource.DataSource = context.Sproducts.ToList();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                String textid = dataGridView1.SelectedRows[0]
                .Cells[0].Value.ToString();
                Console.WriteLine(textid);
                String url = context.Sproducts.Where(s => s.Pd_ID == textid).Select(s => s.Pd_img).First();
                Console.WriteLine(url);
                pictureBox2.Image = LoadImage(url);
            }
            catch { }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                String textid = dataGridView1.SelectedRows[0]
                .Cells[0].Value.ToString();
                Console.WriteLine(textid);
                String url = context.Sproducts.Where(s => s.Pd_ID == textid).Select(s => s.Pd_img).First();
                Console.WriteLine(url);
                pictureBox2.Image = LoadImage(url);
            }
            catch { }

        }
    }
}
