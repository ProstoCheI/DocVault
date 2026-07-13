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
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Документы|*.jpg;*.jpeg;*.png;*.pdf";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");
                System.IO.Directory.CreateDirectory(storagePath);
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ofd.FileName);
                string fullFileName = Path.Combine(storagePath, fileName);
                File.Copy(ofd.FileName, fullFileName);
                new DatabaseHelper().AddDocument(txtTitle.Text, txtTags.Text, Path.GetExtension(ofd.FileName), fullFileName);
                MessageBox.Show("Успешно!");
                UpdateTable();
            }
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
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка удаления!");
                }
            }
        }

        private void txtFields_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtTitle.Text) &&
                     !string.IsNullOrWhiteSpace(txtTags.Text);
        }

        private void dgvDocuments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDocuments.RowCount != 0)
            {
                txtTitle.Text = dgvDocuments.CurrentRow.Cells[1].Value?.ToString();
                txtTags.Text = dgvDocuments.CurrentRow.Cells[2].Value?.ToString();
                string filePath = dgvDocuments.CurrentRow.Cells[4].Value?.ToString();

                // Проверяем, существует ли файл и является ли он картинкой (а не PDF, например)
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath) && (filePath.EndsWith(".jpg") || filePath.EndsWith(".png") || filePath.EndsWith(".jpeg")))
                {
                    // Безопасная загрузка без блокировки файла
                    using (var ms = new MemoryStream(File.ReadAllBytes(filePath)))
                    {
                        pbPreview.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    // Если это не картинка или файла нет, очищаем PictureBox
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
    }
}
