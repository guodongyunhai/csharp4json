using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using LitJson;
using System.Xml;

namespace OCAPIDemo
{
    public class api
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="enc">Encoding</param>
        /// <returns></returns>
        public static string HttpGet(string url, Encoding enc)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 10000;//设置10秒超时
            request.Proxy = null;
            request.Method = "GET";
            request.ContentType = "application/x-www-from-urlencoded";
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), enc);
            StringBuilder sb = new StringBuilder();
            sb.Append(reader.ReadToEnd());
            reader.Close();
            reader.Dispose();
            response.Close();
            return sb.ToString();
        }


        public static string CaiPiaoDataHandleForJson(string html)
        {
            if (!html.Substring(0, 5).Equals("{\"row", StringComparison.OrdinalIgnoreCase))
                throw new Exception(html);
            JsonData json = JsonMapper.ToObject(html);
            string JsonResult = "";
            JsonResult = "采集方式：UTF8编码标准下的Json格式\r\n";
            JsonResult += "采集时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "\r\n";
            JsonResult += "采集行数：" + json["rows"].ToString() + "行\r\n";
            foreach (JsonData row in json["data"])
            {
                JsonResult += "\r\n";
                JsonResult += "开奖期号：" + row["expect"].ToString() + "\r\n";
                JsonResult += "开奖号码：" + row["opencode"].ToString() + "\r\n";
                JsonResult += "开奖时间：" + row["opentime"].ToString() + "\r\n";
            }
            return JsonResult;
        }

        public static string CaiPiaoDataHandleForXML(string html)
        {
            if (!html.Substring(0, 5).Equals("<?xml", StringComparison.OrdinalIgnoreCase))
                throw new Exception(html);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(html);
            string JsonResult = "";
            JsonResult = "采集方式：UTF8编码标准下的Xml格式\r\n";
            JsonResult += "采集时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "\r\n";
            JsonResult += "采集行数：" + xml.SelectSingleNode("xml").Attributes["rows"].Value + "行\r\n";
            foreach (XmlNode node in xml.SelectNodes("xml/row"))
            {
                JsonResult += "\r\n";
                JsonResult += "开奖期号：" + node.Attributes["expect"].Value + "\r\n";
                JsonResult += "开奖号码：" + node.Attributes["opencode"].Value + "\r\n";
                JsonResult += "开奖时间：" + node.Attributes["opentime"].Value + "\r\n";
            }
            return JsonResult;
        }

        /// <summary>
        /// 获取最新一期的的号码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetCaiPiaoResultLastTime(string html)
        {
            if (!html.Substring(0, 5).Equals("<?xml", StringComparison.OrdinalIgnoreCase))
                throw new Exception(html);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(html);
            // string JsonResult = "";
            string OpenNumber = "";
            XmlNodeList numberList = xml.SelectNodes("xml/row");
            OpenNumber = numberList[0].Attributes["opencode"].Value;
            return OpenNumber;
        }


        public static string GetCaiPiaoResultLastTimeForCustomer(string OpenNumber, string CustomerNumber, out int goodNumberCount)
        {
            //开奖期号：2017137
            //开奖号码：05,10,20,23,26,31 + 03
            //开奖时间：2017 - 11 - 21 21:18:20


            string[] OpenNumberList = OpenNumber.Split(',');
            string OpenNumberBlue = OpenNumberList[5].Substring(OpenNumberList[5].Length - 2, 2);
            OpenNumberList[5] = OpenNumberList[5].Substring(0, 2);

            string[] CustomerNumberList = CustomerNumber.Split(',');
            string CustomerNumberBlue = CustomerNumberList[5].Substring(CustomerNumberList[5].Length - 2, 2);
            CustomerNumberList[5] = CustomerNumberList[5].Substring(0, 2);
            goodNumberCount = 0;
            string goodNumberString = "";
            for (int i = 0; i < CustomerNumberList.Length; i++)
            {
                for (int j = 0; j < OpenNumberList.Length; j++)
                {
                    if (int.Parse(CustomerNumberList[i]) == int.Parse(OpenNumberList[j]))
                    {
                        goodNumberCount++;
                        goodNumberString += CustomerNumberList[i] + ",";
                    }
                }
            }

            return goodNumberString;
        }


    }
}
