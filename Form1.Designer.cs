namespace DocVaultLocal
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            lblSearch = new Label();
            txtSearch = new TextBox();
            dgvDocuments = new DataGridView();
            btnDelete = new Button();
            pbPreview = new PictureBox();
            txtTags = new TextBox();
            lblTags = new Label();
            txtTitle = new TextBox();
            lblTitle = new Label();
            btnOpenFile = new Button();
            btnSave = new Button();
            btnAdd = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDocuments).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbPreview).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(lblSearch);
            splitContainer1.Panel1.Controls.Add(txtSearch);
            splitContainer1.Panel1.Controls.Add(dgvDocuments);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(btnDelete);
            splitContainer1.Panel2.Controls.Add(pbPreview);
            splitContainer1.Panel2.Controls.Add(txtTags);
            splitContainer1.Panel2.Controls.Add(lblTags);
            splitContainer1.Panel2.Controls.Add(txtTitle);
            splitContainer1.Panel2.Controls.Add(lblTitle);
            splitContainer1.Panel2.Controls.Add(btnOpenFile);
            splitContainer1.Panel2.Controls.Add(btnSave);
            splitContainer1.Panel2.Controls.Add(btnAdd);
            splitContainer1.Size = new Size(800, 450);
            splitContainer1.SplitterDistance = 460;
            splitContainer1.TabIndex = 0;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(12, 8);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(42, 15);
            lblSearch.TabIndex = 2;
            lblSearch.Text = "Поиск";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 26);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Поиск по названию или #тегам";
            txtSearch.Size = new Size(188, 23);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // dgvDocuments
            // 
            dgvDocuments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDocuments.Dock = DockStyle.Bottom;
            dgvDocuments.Location = new Point(0, 55);
            dgvDocuments.Name = "dgvDocuments";
            dgvDocuments.ReadOnly = true;
            dgvDocuments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDocuments.Size = new Size(460, 395);
            dgvDocuments.TabIndex = 0;
            dgvDocuments.SelectionChanged += dgvDocuments_SelectionChanged;
            // 
            // btnDelete
            // 
            btnDelete.Enabled = false;
            btnDelete.Location = new Point(184, 119);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(140, 23);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Удалить";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // pbPreview
            // 
            pbPreview.BorderStyle = BorderStyle.FixedSingle;
            pbPreview.Dock = DockStyle.Bottom;
            pbPreview.Location = new Point(0, 148);
            pbPreview.Name = "pbPreview";
            pbPreview.Size = new Size(336, 302);
            pbPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pbPreview.TabIndex = 7;
            pbPreview.TabStop = false;
            // 
            // txtTags
            // 
            txtTags.Enabled = false;
            txtTags.Location = new Point(2, 88);
            txtTags.Name = "txtTags";
            txtTags.Size = new Size(176, 23);
            txtTags.TabIndex = 6;
            // 
            // lblTags
            // 
            lblTags.AutoSize = true;
            lblTags.Location = new Point(3, 70);
            lblTags.Name = "lblTags";
            lblTags.Size = new Size(32, 15);
            lblTags.TabIndex = 5;
            lblTags.Text = "Теги";
            // 
            // txtTitle
            // 
            txtTitle.Enabled = false;
            txtTitle.Location = new Point(2, 27);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(176, 23);
            txtTitle.TabIndex = 4;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(3, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(59, 15);
            lblTitle.TabIndex = 3;
            lblTitle.Text = "Название";
            // 
            // btnOpenFile
            // 
            btnOpenFile.Enabled = false;
            btnOpenFile.Location = new Point(184, 88);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(140, 23);
            btnOpenFile.TabIndex = 2;
            btnOpenFile.Text = "Откртыть файл";
            btnOpenFile.UseVisualStyleBackColor = true;
            btnOpenFile.Click += btnOpenFile_Click;
            // 
            // btnSave
            // 
            btnSave.Enabled = false;
            btnSave.Location = new Point(184, 57);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(140, 23);
            btnSave.TabIndex = 1;
            btnSave.Text = "Сохранить изменения";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(184, 26);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(140, 23);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Добавить скан";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Text = "Хранилище документов";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDocuments).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbPreview).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private TextBox txtSearch;
        private DataGridView dgvDocuments;
        private Button btnOpenFile;
        private Button btnSave;
        private Button btnAdd;
        private PictureBox pbPreview;
        private TextBox txtTags;
        private Label lblTags;
        private TextBox txtTitle;
        private Label lblTitle;
        private Label lblSearch;
        private Button btnDelete;
    }
}
