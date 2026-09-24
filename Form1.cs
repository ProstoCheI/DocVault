using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace DocVaultLocal
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource? _searchCts;
        private BindingList<Models.Document> _documentList = new BindingList<Models.Document>();
        private int _curentOffset = 0;
        private bool _isLoading = false;
        private bool _hasMoreData = true;
        private const int pageSize = 30;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            dgvDocuments.DataSource = _documentList;
            await new DatabaseHelper().InitializeDatabaseAsync();
            await LoadAllTableAsync(pageSize, _curentOffset);
        }

        private async Task LoadAllTableAsync(int limit, int offset)
        {
            if (_hasMoreData) {
                if (_curentOffset == 0)
                {
                    _documentList.Clear();
                }
                var items = await new DatabaseHelper().GetDocumentsAsync(limit, offset);
                foreach (var item in items) 
                { 
                    _documentList.Add(item); 
                }
                if (items.Count() < pageSize)
                {
                    _hasMoreData = false;
                }
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddForm())
            {
                addForm.ShowDialog();
            }
            _curentOffset = 0;
            await LoadAllTableAsync(pageSize, _curentOffset);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
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
                        await new DatabaseHelper().DeleteDocumentAsync(id);
                        _curentOffset = 0;
                        await LoadAllTableAsync(pageSize, _curentOffset);
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
            bool hasSelected = dgvDocuments.SelectedRows.Count > 0;
            btnSave.Enabled = hasSelected;
            btnDelete.Enabled = hasSelected;
            btnOpenFile.Enabled = hasSelected;
            txtTitle.Enabled = hasSelected;
            txtTags.Enabled = hasSelected;
            txtTitle.Text = !hasSelected ? "" : txtTitle.Text;
            txtTags.Text = !hasSelected ? "" : txtTags.Text;
            pbPreview.Image = !hasSelected ? null : pbPreview.Image;
            if (hasSelected)
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

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvDocuments.RowCount != 0)
            {
                var selectedDoc = (Models.Document)dgvDocuments.CurrentRow.DataBoundItem;
                int id = selectedDoc.Id;
                string title = txtTitle.Text;
                string tags = txtTags.Text;
                await new DatabaseHelper().UpdateDocumentAsync(id, title, tags);
                _curentOffset = 0;
                await LoadAllTableAsync(pageSize, _curentOffset);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private async void searchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            _curentOffset = 0;
            _hasMoreData = true;
            await PerformSearchAsync(txtSearch.Text.Trim());
        }

        private async Task PerformSearchAsync(string searchText)
        {
            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();
            try
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    if (_curentOffset == 0)
                    {
                        _documentList.Clear();
                    }
                    var items = await new DatabaseHelper().SearchDocumentsAsync(searchText, pageSize, _curentOffset, _searchCts.Token);
                    foreach (var item in items)
                    {
                        _documentList.Add(item);
                    }
                    if (items.Count() < pageSize)
                    {
                        _hasMoreData = false;
                    }
                }
                else
                {
                    _curentOffset = 0;
                    await LoadAllTableAsync(pageSize, _curentOffset);
                }
            }
            catch (OperationCanceledException) { }
        }

        private async void dgvDocuments_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll && (dgvDocuments.FirstDisplayedScrollingRowIndex + dgvDocuments.DisplayedRowCount(true) >= dgvDocuments.RowCount))
            {
                if (_hasMoreData)
                {
                    if (!_isLoading)
                    {
                        _isLoading = true;
                        _curentOffset += pageSize;
                        if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
                        {
                            await LoadAllTableAsync(pageSize, _curentOffset);
                        }
                        else
                        {
                            await PerformSearchAsync(txtSearch.Text.Trim());
                        }
                        _isLoading = false;
                    }
                }
            }
        }
    }
}
