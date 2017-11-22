using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using LitJson;
using System.Windows.Forms;

namespace OCAPIDemo
{
    public partial class FormDemo : Form
    {
        public FormDemo()
        {
            InitializeComponent();
        }

        private void buttonXml_Click(object sender, EventArgs e)
        {
            try
            {
                string html = api.HttpGet(this.textBoxXml.Text, Encoding.UTF8);
                this.textBoxXmlResult.Text = api.CaiPiaoDataHandleForXML(html);

                string LastNumber = api.GetCaiPiaoResultLastTime(html);

                int goodNumberCount = 0;
                string GoodNumberList = api.GetCaiPiaoResultLastTimeForCustomer(LastNumber, textBox1.Text, out goodNumberCount);

                this.textBoxXmlResult.Text += "\r\n" + "中" + goodNumberCount.ToString() + "号码。" + "\r\n";
                if (goodNumberCount > 0)
                {
                    this.textBoxXmlResult.Text += "中的号码是：" + GoodNumberList + "\r\n";
                }
                else
                {
                    this.textBoxXmlResult.Text += "一个都没有中！！！！！" + "\r\n";
                }


            }
            catch (Exception ex) { this.textBoxXmlResult.Text = "采集出现错误：" + ex.Message; }
        }

        private void buttonJson_Click(object sender, EventArgs e)
        {
            try
            {
                string html = api.HttpGet(this.textBoxJson.Text, Encoding.UTF8);
                textBoxJsonResult.Text = api.CaiPiaoDataHandleForJson(html);
            }
            catch (Exception ex) { this.textBoxJsonResult.Text = "采集出现错误：" + ex.Message; }
        }



        private void label3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.opencai.net/");
        }

        private void FormDemo_Load(object sender, EventArgs e)
        {

        }
    }
}