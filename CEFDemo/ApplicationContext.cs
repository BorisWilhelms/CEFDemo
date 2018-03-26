using System;
using System.Windows.Forms;

namespace CEFDemo
{
    public class ApplicationBridge
    {
        public Action<ToDoItem> OnEdit { get; set; }

        public void Edit(ToDoItem item)
        {
            OnEdit?.Invoke(item);
        }

        public bool Delete(ToDoItem item)
        {
            var result = MessageBox.Show(
                $"Do you really want to delete the Item: \n {item.Text}",
                "Delete item",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            return result == DialogResult.Yes;
        }
    }
}
