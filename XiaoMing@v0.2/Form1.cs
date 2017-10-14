using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace XiaoMing_v0._2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BrowserControl myCtrl = new BrowserControl();
            myCtrl.AddTab += new BrowserControl.OperatorEventHandler(myCtrl_AddTab);
            myCtrl.RemoveTab += new BrowserControl.OperatorEventHandler(myCtrl_RemoveTab);
            myCtrl.Dock = DockStyle.Fill;
            tabPage1.Controls.Add(myCtrl);
        }
        public void myCtrl_AddTab(object sender)
        {
            TabPage Page = new TabPage();
            Page.Name = "新标签页";
            Page.Text = "新标签页";
            Page.Font = new Font("宋体", Page.Font.Size, Page.Font.Style);
            BrowserControl myCtrl = new BrowserControl();
            myCtrl.AddTab += new BrowserControl.OperatorEventHandler(myCtrl_AddTab);
            myCtrl.RemoveTab += new BrowserControl.OperatorEventHandler(myCtrl_RemoveTab);
            myCtrl.Dock = DockStyle.Fill;
            Page.Controls.Add(myCtrl);
            tabControl1.Controls.Add(Page);
            tabControl1.SelectedTab = Page;
        }
        public void myCtrl_RemoveTab(object sender)
        {
            if(tabControl1.Controls.Count > 1)
            {
                BrowserControl myCtrl = sender as BrowserControl;
                TabPage Page = (TabPage)myCtrl.Parent;
                tabControl1.Controls.Remove(Page);
            }
        }
    }
}
