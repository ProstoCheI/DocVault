using System.IO;
using System.Diagnostics;

namespace DocVaultLocal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            new DatabaseHelper().InitializeDatabase();
            UpdateTable();
        }

        private void UpdateTable()
        {
            dgvDocuments.DataSource = new DatabaseHelper().GetAllDocuments();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddForm())
            {
                addForm.ShowDialog();
            }
            UpdateTable();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDocuments.RowCount != 0)
            {
                int id = Convert.ToInt32(dgvDocuments.CurrentRow.Cells[0].Value);
                string filePath = dgvDocuments.CurrentRow.Cells[4].Value.ToString();
                string fileName = dgvDocuments.CurrentRow.Cells[1].Value.ToString();
                try
                {
                    if (MessageBox.Show($"Вы уверены что хотите удалить {fileName}", "Удаление!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        new DatabaseHelper().DeleteDocument(id);
                        UpdateTable();
                        if (dgvDocuments.RowCount == 0)
                        {
                            txtTitle.Text = "";
                            txtTags.Text = "";
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка удаления!");
                }
            }
        }

        private void dgvDocuments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDocuments.RowCount != 0)
            {
                txtTitle.Text = dgvDocuments.CurrentRow.Cells[1].Value?.ToString();
                txtTags.Text = dgvDocuments.CurrentRow.Cells[2].Value?.ToString();
                string filePath = dgvDocuments.CurrentRow.Cells[4].Value?.ToString();

                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath) && (filePath.EndsWith(".jpg") || filePath.EndsWith(".png") || filePath.EndsWith(".jpeg")))
                {
                    using (var ms = new MemoryStream(File.ReadAllBytes(filePath)))
                    {
                        pbPreview.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pbPreview.Image = null;
                }
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (dgvDocuments.RowCount != 0)
            {
                string filePath = dgvDocuments.CurrentRow.Cells[4].Value?.ToString();
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    var info = new ProcessStartInfo();
                    info.FileName = filePath;
                    info.UseShellExecute = true;
                    Process.Start(info);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvDocuments.RowCount != 0)
            {
                int id = Convert.ToInt32(dgvDocuments.CurrentRow.Cells[0].Value);
                string title = txtTitle.Text;
                string tags = txtTags.Text;
                new DatabaseHelper().UpdateDocument(id, title, tags);
                UpdateTable();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                dgvDocuments.DataSource = new DatabaseHelper().SearchDocuments(txtSearch.Text);
            }
            else
            {
                UpdateTable();
            }
        }
    }
}
