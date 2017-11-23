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
                string result = "";
                string LastNumber = api.GetCaiPiaoResultLastTime(html);
                bool IsBlueNumber = false;
                string[] CostomerNumberList = new string[4] { "05,08,15,16,23,31 + 03", "02,06,11,16,17,22 + 09", "03,10,19,22,25,31 + 08", "06,11,13,25,26,30 + 07" };
                for (int i = 0; i < CostomerNumberList.Length; i++)
                {
                    result += "\r\n"+"客户号码 " + i.ToString() + ": " + CostomerNumberList[i] ;
                    int goodNumberCount = 0;
                    string GoodNumberList = api.GetCaiPiaoResultLastTimeForCustomer(LastNumber, CostomerNumberList[i], out goodNumberCount, out IsBlueNumber);
                    result += "\r\n" + "中" + goodNumberCount.ToString() + "个普通号码。" + "\r\n";
                    if (goodNumberCount > 0)
                    {
                        result += "中的号码是：" + GoodNumberList + "\r\n";
                    }
                    else
                    {
                        result += "普通号码一个都没有中！！！！！" + "\r\n";
                    }

                    if (IsBlueNumber)
                    {
                        result += "恭喜发财，中了一个特别号码！！！！！" + "\r\n";
                    }
                }

                this.textBoxXmlResult.Text = result + "\r\n";
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