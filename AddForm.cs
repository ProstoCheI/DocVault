using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocVaultLocal
{
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();
        }

        private void txtFields_TextChanged(object sender, EventArgs e)
        {
            btnSelectFile.Enabled = !string.IsNullOrWhiteSpace(txtTitle.Text) &&
                     !string.IsNullOrWhiteSpace(txtTags.Text);
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
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
                this.Close();
            }
        }
    }
}
