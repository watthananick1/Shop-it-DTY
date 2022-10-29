using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

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
            listView1.Items.Clear();
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
                listView1.Items.Add(new ListViewItem(data));


                //HtmlNode NameNode = doc.DocumentNode
                //    .SelectSingleNode("//meta[@property=\"og:title\"]");
                //product.Pd_name = NameNode.Attributes["content"].Value;

                //HtmlNode descriptionNode = doc.DocumentNode
                //    .SelectSingleNode("//meta[@property=\"og:description\"]");
                //product.Pd_detail = descriptionNode.Attributes["content"].Value;
                //Console.WriteLine(product.Pd_detail);
            }
            catch (Exception)
            {
                //HtmlNode NameNode = doc.DocumentNode
                //    .SelectSingleNode("//meta[@property=\"og:title\"]");
                //product.Pd_name = NameNode.Attributes["content"].Value;
                //Console.WriteLine(product.Pd_name);

                //HtmlNode descriptionNode = doc.DocumentNode
                //    .SelectSingleNode("//meta[@property=\"og:description\"]");
                //product.Pd_detail = descriptionNode.Attributes["content"].Value;
                //Console.WriteLine(product.Pd_detail);
            }

            //HtmlNode priceNode = doc.DocumentNode
            //    .SelectSingleNode("//div[@class=" +
            //    "\"col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center price_block\"]");
            //String price = priceNode.InnerText;
            //price = new string(price.Where(c => char.IsDigit(c)).ToArray());
            //product.Pd_unitprice = decimal.Parse(price + ".00");
            //Console.WriteLine(product.Pd_unitprice);

            //HtmlNode imageNode = doc.DocumentNode
            //    .SelectSingleNode("//meta[@property=\"og:image\"]");
            //product.Pd_img = imageNode.Attributes["content"].Value;
            //Console.WriteLine(product.Pd_img);

            //product.Pd_stock = 10;
            //Console.WriteLine(product.Pd_stock);

            //var typeNode = doc.DocumentNode
            //    .SelectNodes("//div[@class=\"step_nav\"]//a");
            //product.Pd_type = typeNode[2].InnerText;
            //Console.WriteLine(product.Pd_type);

            //context.Sproducts.Add(product);
            //qc += context.SaveChanges();
                //Console.WriteLine(qc);


        }
        

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
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
    }
}
