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
                var selectedDoc = (Models.Document)dgvDocuments.CurrentRow.DataBoundItem;
                int id = selectedDoc.Id;
                string filePath = selectedDoc.FilePath;
                string fileName = selectedDoc.Title;
                try
                {
                    if (MessageBox.Show($"Вы уверены, что хотите удалить {fileName}", "Подтвердите удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
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
                catch (IOException ioEx)
                {
                    MessageBox.Show($"Не удалось удалить файл с диска. Возможно, он открыт в другой программе.\n\nТехнические детали:\n{ioEx.Message}",
                                    "Ошибка доступа к файлу",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при удалении документа:\n\n{ex.Message}",
                                    "Системная ошибка",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void dgvDocuments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDocuments.RowCount != 0)
            {
                var selectedDoc = (Models.Document)dgvDocuments.CurrentRow.DataBoundItem;
                txtTitle.Text = selectedDoc.Title;
                txtTags.Text = selectedDoc.Tags;
                string filePath = selectedDoc.FilePath;

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
                var selectedDoc = (Models.Document)dgvDocuments.CurrentRow.DataBoundItem;
                string filePath = selectedDoc.FilePath;
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
                var selectedDoc = (Models.Document)dgvDocuments.CurrentRow.DataBoundItem;
                int id = selectedDoc.Id;
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
