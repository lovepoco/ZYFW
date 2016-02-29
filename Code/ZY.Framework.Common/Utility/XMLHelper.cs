using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ZY.Framework.Common.Utility
{
    /// <summary>
    /// XML操作类
    /// </summary>
    public class XMLHelper
    {
        #region 属性
        private XDocument _doc=null;
        /// <summary>
        /// 文档对象
        /// </summary>
        public XDocument XMLDoc
        {
            get
            {
                return _doc;
            }
        }
        private string _filename = "";
        /// <summary>
        /// 对应xml文件名
        /// </summary>
        public string filename
        {
            get { return _filename; }
            set { _filename = value; }
        }
        #endregion

        #region 构造方法
        public XMLHelper()
        {

        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="fname">对应xml文件名</param>
        public XMLHelper(string fname)
        {
            if (!string.IsNullOrEmpty(fname)) _filename = fname;
            Load(_filename);
        }
        #endregion

        #region 文档级
        /// <summary>
        /// 建立初始文档
        /// </summary>
        public void CreateDocument()
        {
            MemoryStream _mst=new MemoryStream();
            string _xmlcontent = @"<?xml version=""1.0"" encoding=""utf-8""?><config></config>";
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(_xmlcontent);
            _mst = new MemoryStream(bs);
            _doc = XDocument.Load(_mst);
            XNode fxnode = _doc.FirstNode;
        }
        /// <summary>
        /// 装载文档对象
        /// </summary>
        /// <param name="filename">文件名，为空表示加载filename属性对应文件</param>
        /// <returns></returns>
        public bool Load(string filename)
        {
            bool result = false;
            if (string.IsNullOrWhiteSpace(filename)) filename = _filename;
            try
            {
                _doc = XDocument.Load(filename);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 测试用，需删除
        /// </summary>
        public void test()
        {
            XDocument xdoc = XDocument.Load("e:\\test\\test.xml");
            var qu = xdoc.XPathSelectElements("//test");
            string _test = "";
            foreach (var item in qu)
            {
                _test += item.ToString();
            }
            string s = xdoc.ToString();
            //改名
            //root.Name = "testroot";
            //root.Save("e:\\test\\test.xml");
        }
        /// <summary>
        /// 对应filename属性保存xml
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            return Save(_filename);
        }
        /// <summary>
        /// 保存xml
        /// </summary>
        /// <param name="filename">文件名，为空表示个保存filename属性对应文件</param>
        /// <returns></returns>
        public bool Save(string filename)
        {
            bool result = false;
            if (string.IsNullOrWhiteSpace(filename)) filename = _filename;
            try
            {
                _doc.Save(filename);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region 节点级
        /// <summary>
        /// 通过xpath获取节点
        /// </summary>
        /// <param name="xpath">xpath</param>
        /// <returns></returns>
        public XElement getElement(string xpath)
        {
            XElement result = null;
            try
            {
                result = _doc.XPathSelectElement(xpath);
            }
            catch
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 获取节点集
        /// </summary>
        /// <param name="xpath">xpath</param>
        /// <returns></returns>
        public IEnumerable<XElement> getElements(string xpath)
        {
            IEnumerable<XElement> result;
            try
            {
                result = _doc.XPathSelectElements(xpath);
            }
            catch
            {
                result = null;
            }
            return result;
        }
        #endregion


    }
}
