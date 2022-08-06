using System.ComponentModel;
using System.Diagnostics;

namespace WingetUI
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetDataGridDataSource();
        }

        private void SetDataGridDataSource()
        {
            var updates = Winget.GetUpdates();
            var bindingList = new BindingList<Winget.Entity>(updates);
            var source = new BindingSource(bindingList, null);

            UpdatesDataGrid.DataSource = source;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            Cursor = Cursors.WaitCursor;

            if (UpdatesDataGrid.SelectedRows.Count > 0)
            {
                var selected = (Winget.Entity)UpdatesDataGrid.SelectedRows[0].DataBoundItem;

                Debug.WriteLine(selected);

                if (Winget.UpgradeSoftware(selected) == true)
                {
                    btnUpdate.BackColor = Color.LightGreen;
                    SetDataGridDataSource();
                } else
                {
                    btnUpdate.BackColor = Color.Red;
                }

            }

            Cursor = Cursors.Default;
            btnUpdate.Enabled = true;
        }

        private void UpdatesDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            btnUpdate.BackColor = SystemColors.Control;
        }
    }
}