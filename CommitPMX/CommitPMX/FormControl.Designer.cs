
namespace CommitPMX
{
    partial class FormControl
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
            this.textBoxCommitComment = new System.Windows.Forms.TextBox();
            this.labelCommitComment = new System.Windows.Forms.Label();
            this.buttonCommit = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxAmend = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxCommitComment
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxCommitComment, 2);
            this.textBoxCommitComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCommitComment.Location = new System.Drawing.Point(4, 34);
            this.textBoxCommitComment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxCommitComment.Name = "textBoxCommitComment";
            this.textBoxCommitComment.AutoSize = false;
            this.textBoxCommitComment.Size = new System.Drawing.Size(361, 118);
            this.textBoxCommitComment.TabIndex = 0;
            // 
            // labelCommitComment
            // 
            this.labelCommitComment.AutoSize = true;
            this.labelCommitComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCommitComment.Location = new System.Drawing.Point(4, 0);
            this.labelCommitComment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCommitComment.Name = "labelCommitComment";
            this.labelCommitComment.Size = new System.Drawing.Size(130, 29);
            this.labelCommitComment.TabIndex = 1;
            this.labelCommitComment.Text = "コメント";
            this.labelCommitComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonCommit
            // 
            this.buttonCommit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCommit.Enabled = false;
            this.buttonCommit.Location = new System.Drawing.Point(3, 160);
            this.buttonCommit.Name = "buttonCommit";
            this.buttonCommit.Size = new System.Drawing.Size(132, 37);
            this.buttonCommit.TabIndex = 2;
            this.buttonCommit.Text = "コミット";
            this.buttonCommit.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 138F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.buttonCommit, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxCommitComment, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelCommitComment, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxAmend, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(369, 200);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // checkBoxAmend
            // 
            this.checkBoxAmend.AutoSize = true;
            this.checkBoxAmend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxAmend.Location = new System.Drawing.Point(148, 160);
            this.checkBoxAmend.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.checkBoxAmend.Name = "checkBoxAmend";
            this.checkBoxAmend.Size = new System.Drawing.Size(218, 37);
            this.checkBoxAmend.TabIndex = 3;
            this.checkBoxAmend.Text = "修正";
            this.checkBoxAmend.UseVisualStyleBackColor = true;
            // 
            // FormControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 200);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormControl";
            this.Text = "CommitPMX";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxCommitComment;
        private System.Windows.Forms.Label labelCommitComment;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonCommit;
        private System.Windows.Forms.CheckBox checkBoxAmend;
    }
}