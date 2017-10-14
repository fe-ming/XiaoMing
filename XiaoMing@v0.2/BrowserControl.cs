using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;
using XiaoMing_v0._2.Properties;

namespace XiaoMing_v0._2
{
    public partial class BrowserControl : UserControl
    {
        public IWinFormsWebBrowser KitBrowser { get; private set; }
        ChromiumWebBrowser kitBrowser;
        WebBrowser browser = new WebBrowser();
        String core = "chrome";
        public BrowserControl()
        {
            InitializeComponent();
            kitBrowser = new ChromiumWebBrowser("");
            kitBrowser.Dock = DockStyle.Fill;
            panel2.Controls.Add(kitBrowser);
            kitBrowser.LifeSpanHandler = new OpenPageSelf();
            KitBrowser = kitBrowser;
            kitBrowser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;//添加事件
            //kitBrowser.MouseDown += OnBrowserMouseClick; //不支持？
            kitBrowser.LoadError += OnLoadError;
            kitBrowser.TitleChanged += OnBrowserTitleChanged;
            kitBrowser.AddressChanged += OnBrowserAddressChanged;
            //kitBrowser.RegisterJsObject("bound", new BoundObject());
            //kitBrowser.RegisterAsyncJsObject("boundAsync", new AsyncBoundObject());
            var eventObject = new ScriptedMethodsBoundObject();
            eventObject.EventArrived += OnJavascriptEventArrived;
            // Use the default of camelCaseJavascriptNames
            // .Net methods starting with a capitol will be translated to starting with a lower case letter when called from js
            kitBrowser.RegisterJsObject("boundEvent", eventObject, camelCaseJavascriptNames: true);

            browser.NewWindow += new CancelEventHandler(browser_NewWindow);
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String url = this.textBox1.Text;
                if (url != "")
                {
                    if (core == "chrome")
                    {
                        kitBrowser.Load(url);
                    }
                    else if (core == "ie")
                    {
                        browser.Navigate(url);
                    }
                }
            }
        }
        private void OnIsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs args)
        {
            if (args.IsBrowserInitialized)
            {
                //MessageBox.Show(kitBrowser.TooltipText);
                kitBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("document.getElementById('su').value='123'");
                //kitBrowser.GetBrowser().ShowDevTools();
                //kitBrowser.ExecuteScriptAsync("alert('Hello')");
                //kitBrowser.ExecuteScriptAsync("alert('World')");
                //kitBrowser.ExecuteScriptAsync("document.addEventListener('click', function(){alert('1')}); ");
                string script = string.Format("document.getElementById('su').value;");
                kitBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                {
                    var response = x.Result;

                    if (response.Success && response.Result != null)
                    {
                        var startDate = response.Result;
                        //startDate is the value of a HTML element.
                    }
                });

            }
        }
        /// <summary>
        /// 在自己窗口打开链接
        /// </summary>
        internal class OpenPageSelf : ILifeSpanHandler
        {
            public bool DoClose(IWebBrowser browserControl, IBrowser browser)
            {
                return false;
            }

            public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
            {

            }

            public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
            {

            }

            public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl,
    string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures,
    IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
            {
                newBrowser = null;
                var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
                chromiumWebBrowser.Load(targetUrl);
                return true; //Return true to cancel the popup creation copyright by codebye.com.
            }
        }
        private void browser_NewWindow(object sender, CancelEventArgs e)
        {
            string url = ((WebBrowser)sender).StatusText;
            browser.Navigate(url);
            e.Cancel = true;
        }
        private void browser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {

            if ((e.CurrentProgress > 0) && (e.MaximumProgress > 0))
            {


            }

            else if (browser.ReadyState == WebBrowserReadyState.Complete)//加载完成后隐藏进度条
            {



            }

        }
        private void OnBrowserMouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Mouse Clicked" + e.X + ";" + e.Y + ";" + e.Button);
        }
        private void OnLoadError(object sender, LoadErrorEventArgs args)
        {
            //DisplayOutput("Load Error:" + args.ErrorCode + ";" + args.ErrorText);
        }
        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => Parent.Text = args.Title);
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => textBox1.Text = args.Address);
        }
        private static void OnJavascriptEventArrived(string eventName, object eventData)
        {
            switch (eventName)
            {
                case "click":
                    {
                        var message = eventData.ToString();
                        var dataDictionary = eventData as Dictionary<string, object>;
                        if (dataDictionary != null)
                        {
                            var result = string.Join(", ", dataDictionary.Select(pair => pair.Key + "=" + pair.Value));
                            message = "event data: " + result;
                        }
                        MessageBox.Show(message, "Javascript event arrived", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
            }
        }
        private void DisplayOutput(string output)
        {
            this.InvokeOnUiThreadIfRequired(() => textBox1.Text = output);
        }
        /// <summary>
        /// 内核切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            var url = textBox1.Text;
            if (core == "chrome")
            {
                browser.ProgressChanged += new WebBrowserProgressChangedEventHandler(browser_ProgressChanged);
                browser.DocumentTitleChanged += browser_DocumentTitleChanged;
                browser.Navigated += OnNavigated;
                browser.Dock = DockStyle.Fill;
                panel2.Controls.Clear();
                panel2.Controls.Add(browser);
                pictureBox4.Image = Resources.ie;
                core = "ie";
                if (url != "")
                {
                    browser.Navigate(url);
                }
            }
            else if (core == "ie")
            {
                kitBrowser.Dock = DockStyle.Fill;
                panel2.Controls.Clear();
                panel2.Controls.Add(kitBrowser);
                pictureBox4.Image = Resources.chrome;
                core = "chrome";
                if (url != "")
                {
                    LoadUrl(url);
                }
            }
        }
        //声明事件委托  
        public delegate void OperatorEventHandler(object sender);
        //定义事件  
        public event OperatorEventHandler AddTab;
        /// <summary>
        /// 添加新页签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            AddTab(this);
        }

        //定义事件  
        public event OperatorEventHandler RemoveTab;
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            RemoveTab(this);
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            var url = textBox1.Text;
            if (core == "chrome")
            {
                LoadUrl(url);
            }else if(core == "ie")
            {
                browser.Navigate(url);
            }
           
        }
        /// <summary>
        /// chrome 刷新
        /// </summary>
        /// <param name="url"></param>
        private void LoadUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                KitBrowser.Load(url);
            }
        }
        /// <summary>
        /// 后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (core == "chrome")
            {
                KitBrowser.Back();
            }else if (core == "ie")
            {
                browser.GoBack(); //后退
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (core == "chrome")
            {
                KitBrowser.Forward();
            }else if (core == "ie")
            {
                browser.GoForward(); //前进
            }
            
        }
        private void SetCanGoBack(bool canGoBack)
        {
            this.InvokeOnUiThreadIfRequired(() => pictureBox1.Enabled = canGoBack);
        }

        private void SetCanGoForward(bool canGoForward)
        {
            this.InvokeOnUiThreadIfRequired(() => pictureBox2.Enabled = canGoForward);
        }
        /// <summary>
        /// ie模式下监听导航改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnNavigated(object sender, WebBrowserNavigatedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => textBox1.Text = args.Url.ToString());
        }
        /// <summary>
        /// ie模式下网页标题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browser_DocumentTitleChanged(object sender, EventArgs e)
        {
            this.InvokeOnUiThreadIfRequired(() => Parent.Text = browser.DocumentTitle);
        }
    }
}
