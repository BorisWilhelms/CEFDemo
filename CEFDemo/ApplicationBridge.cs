using CefSharp;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;

namespace CEFDemo
{
    public class ApplicationBridge
    {
        private readonly IFrame _frame;
        private readonly Action<ToDoItem> _onEdit;

        public ApplicationBridge(IFrame frame, Action<ToDoItem> onEdit)
        {
            _frame = frame;
            _onEdit = onEdit;
        }

        public void Edit(ToDoItem item) =>
            _onEdit?.Invoke(item);

        public bool Delete(ToDoItem item)
        {
            var result = MessageBox.Show(
                $"Do you really want to delete the Item: \n {item.Text}",
                "Delete item",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            return result == DialogResult.Yes;
        }

        public void UpdateItem(ToDoItem item)
        {
            var json = JsonConvert.SerializeObject(item);
            _frame.ExecuteJavaScriptAsync($"(function(){{ updateItem({json}) }})();");
        }
    }
}
