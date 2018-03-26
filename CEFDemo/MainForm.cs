using CefSharp;
using CefSharp.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

namespace CEFDemo
{
    public partial class MainForm : Form
    {
        private ChromiumWebBrowser _browser;
        private string _currentId = null;

        public MainForm() =>
            InitializeComponent();

        private void MainForm_Load(object sender, EventArgs e)
        {

            var url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HTML\\App.html");
            _browser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill,
            };
            _browser.JavascriptObjectRepository.ResolveObject += JavascriptObjectRepository_ResolveObject;
            CefPanel.Controls.Add(_browser);
        }

        private void MainForm_DoubleClick(object sender, EventArgs e) => _browser.ShowDevTools();

        private void JavascriptObjectRepository_ResolveObject(object sender, CefSharp.Event.JavascriptBindingEventArgs e)
        {
            if (e.ObjectName == "bridge")
            {
                var context = new ApplicationBridge
                {
                    OnEdit = (item) =>
                    {
                        _currentId = item.Id;
                        Invoke((Action)(() => InputTextBox.Text = item.Text));
                    }
                };

                e.ObjectRepository.Register("bridge", context, true, BindingOptions.DefaultBinder);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var item = $"{{ Id: '{_currentId ?? Guid.NewGuid().ToString()}', Text: '{InputTextBox.Text}' }}";
            _browser.GetMainFrame().ExecuteJavaScriptAsync($"(function(){{ updateItem({item}) }})();");
            _currentId = null;
            InputTextBox.Text = String.Empty;
        }
    }
}
