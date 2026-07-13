namespace DocVaultLocal
{
    partial class AddForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblTags = new Label();
            txtTitle = new TextBox();
            txtTags = new TextBox();
            btnSelectFile = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(12, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(59, 15);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Название";
            // 
            // lblTags
            // 
            lblTags.AutoSize = true;
            lblTags.Location = new Point(12, 70);
            lblTags.Name = "lblTags";
            lblTags.Size = new Size(32, 15);
            lblTags.TabIndex = 1;
            lblTags.Text = "Теги";
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(12, 27);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(177, 23);
            txtTitle.TabIndex = 2;
            txtTitle.TextChanged += txtFields_TextChanged;
            // 
            // txtTags
            // 
            txtTags.Location = new Point(12, 88);
            txtTags.Name = "txtTags";
            txtTags.Size = new Size(177, 23);
            txtTags.TabIndex = 3;
            txtTags.TextChanged += txtFields_TextChanged;
            // 
            // btnSelectFile
            // 
            btnSelectFile.Enabled = false;
            btnSelectFile.Location = new Point(12, 132);
            btnSelectFile.Name = "btnSelectFile";
            btnSelectFile.Size = new Size(177, 23);
            btnSelectFile.TabIndex = 4;
            btnSelectFile.Text = "Выбрать файл и сохранить";
            btnSelectFile.UseVisualStyleBackColor = true;
            btnSelectFile.Click += btnSelectFile_Click;
            // 
            // AddForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(886, 439);
            Controls.Add(btnSelectFile);
            Controls.Add(txtTags);
            Controls.Add(txtTitle);
            Controls.Add(lblTags);
            Controls.Add(lblTitle);
            Name = "AddForm";
            Padding = new Padding(0, 0, 12, 12);
            Text = "AddForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblTags;
        private TextBox txtTitle;
        private TextBox txtTags;
        private Button btnSelectFile;
    }
}