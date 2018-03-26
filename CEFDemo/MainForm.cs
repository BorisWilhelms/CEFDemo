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
        private ApplicationBridge _bridge;
        private string _currentId;

        public MainForm() =>
            InitializeComponent();

        private void MainForm_Load(object sender, EventArgs e)
        {
            var url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HTML\\App.html");
            _browser = new ChromiumWebBrowser(url)
            {
                Dock = DockStyle.Fill,
            };
            // Register a function that provides the bridge object
            _browser.JavascriptObjectRepository.ResolveObject += JavascriptObjectRepository_ResolveObject;
            CefPanel.Controls.Add(_browser);
        }

        private void MainForm_DoubleClick(object sender, EventArgs e) =>
            _browser.ShowDevTools();

        private void JavascriptObjectRepository_ResolveObject(object sender, CefSharp.Event.JavascriptBindingEventArgs e)
        {
            // Check if asked for an object with name "brdige"
            if (e.ObjectName == "bridge")
            {
                _bridge = new ApplicationBridge(_browser.GetMainFrame(), EditItem);
                e.ObjectRepository.Register("bridge", _bridge, true, BindingOptions.DefaultBinder);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            _bridge?.UpdateItem(new ToDoItem() { Id = _currentId ?? Guid.NewGuid().ToString(), Text = InputTextBox.Text });
            _currentId = null;
            InputTextBox.Text = String.Empty;
        }

        private void EditItem(ToDoItem item)
        {
            _currentId = item.Id;
            Invoke((Action)(() => InputTextBox.Text = item.Text));
        }
    }
}
