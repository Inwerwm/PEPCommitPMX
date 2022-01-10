
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
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.labelMessage = new System.Windows.Forms.Label();
            this.buttonCommit = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonReCompress = new System.Windows.Forms.Button();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.groupBoxArchiveFormat = new System.Windows.Forms.GroupBox();
            this.radioButtonZip = new System.Windows.Forms.RadioButton();
            this.radioButton7z = new System.Windows.Forms.RadioButton();
            this.buttonReconstruction = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain.SuspendLayout();
            this.groupBoxArchiveFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxMessage
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxMessage, 2);
            this.textBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMessage.Location = new System.Drawing.Point(4, 34);
            this.textBoxMessage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(406, 97);
            this.textBoxMessage.TabIndex = 0;
            this.textBoxMessage.TextChanged += new System.EventHandler(this.textBoxMessage_TextChanged);
            this.textBoxMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMessage_KeyPress);
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelMessage, 2);
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMessage.Location = new System.Drawing.Point(4, 0);
            this.labelMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(406, 29);
            this.labelMessage.TabIndex = 1;
            this.labelMessage.Text = "メッセージ(文字以内)  Ctrl+Enterでコミット";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonCommit
            // 
            this.buttonCommit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCommit.Enabled = false;
            this.buttonCommit.Location = new System.Drawing.Point(3, 139);
            this.buttonCommit.Name = "buttonCommit";
            this.buttonCommit.Size = new System.Drawing.Size(132, 68);
            this.buttonCommit.TabIndex = 2;
            this.buttonCommit.Text = "コミット";
            this.buttonCommit.UseVisualStyleBackColor = true;
            this.buttonCommit.Click += new System.EventHandler(this.buttonCommit_Click);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 138F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.buttonCommit, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxMessage, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelMessage, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonReCompress, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxDescription, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxArchiveFormat, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonReconstruction, 0, 4);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 5;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(414, 321);
            this.tableLayoutPanelMain.TabIndex = 3;
            // 
            // buttonReCompress
            // 
            this.buttonReCompress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReCompress.Location = new System.Drawing.Point(3, 213);
            this.buttonReCompress.Name = "buttonReCompress";
            this.buttonReCompress.Size = new System.Drawing.Size(132, 68);
            this.buttonReCompress.TabIndex = 4;
            this.buttonReCompress.Text = "アーカイブを\r\n再圧縮";
            this.buttonReCompress.UseVisualStyleBackColor = true;
            this.buttonReCompress.Click += new System.EventHandler(this.buttonReCompress_Click);
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(141, 213);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.Size = new System.Drawing.Size(270, 68);
            this.textBoxDescription.TabIndex = 5;
            this.textBoxDescription.Text = "アーカイブを再圧縮することで、\r\nアーカイブの容量を減らすことが\r\n期待できます。\r\n";
            // 
            // groupBoxArchiveFormat
            // 
            this.groupBoxArchiveFormat.Controls.Add(this.radioButtonZip);
            this.groupBoxArchiveFormat.Controls.Add(this.radioButton7z);
            this.groupBoxArchiveFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxArchiveFormat.Location = new System.Drawing.Point(141, 139);
            this.groupBoxArchiveFormat.Name = "groupBoxArchiveFormat";
            this.groupBoxArchiveFormat.Size = new System.Drawing.Size(270, 68);
            this.groupBoxArchiveFormat.TabIndex = 6;
            this.groupBoxArchiveFormat.TabStop = false;
            this.groupBoxArchiveFormat.Text = "圧縮方式";
            // 
            // radioButtonZip
            // 
            this.radioButtonZip.AutoSize = true;
            this.radioButtonZip.Location = new System.Drawing.Point(66, 22);
            this.radioButtonZip.Name = "radioButtonZip";
            this.radioButtonZip.Size = new System.Drawing.Size(48, 25);
            this.radioButtonZip.TabIndex = 1;
            this.radioButtonZip.TabStop = true;
            this.radioButtonZip.Text = "zip";
            this.radioButtonZip.UseVisualStyleBackColor = true;
            this.radioButtonZip.CheckedChanged += new System.EventHandler(this.radioButtonZip_CheckedChanged);
            this.radioButtonZip.MouseLeave += new System.EventHandler(this.radioButtonZip_MouseLeave);
            this.radioButtonZip.MouseHover += new System.EventHandler(this.radioButtonZip_MouseHover);
            // 
            // radioButton7z
            // 
            this.radioButton7z.AutoSize = true;
            this.radioButton7z.Location = new System.Drawing.Point(6, 22);
            this.radioButton7z.Name = "radioButton7z";
            this.radioButton7z.Size = new System.Drawing.Size(44, 25);
            this.radioButton7z.TabIndex = 0;
            this.radioButton7z.TabStop = true;
            this.radioButton7z.Text = "7z";
            this.radioButton7z.UseVisualStyleBackColor = true;
            this.radioButton7z.CheckedChanged += new System.EventHandler(this.radioButton7z_CheckedChanged);
            this.radioButton7z.MouseEnter += new System.EventHandler(this.radioButton7z_MouseEnter);
            this.radioButton7z.MouseLeave += new System.EventHandler(this.radioButton7z_MouseLeave);
            // 
            // buttonReconstruction
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonReconstruction, 2);
            this.buttonReconstruction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReconstruction.Location = new System.Drawing.Point(3, 287);
            this.buttonReconstruction.Name = "buttonReconstruction";
            this.buttonReconstruction.Size = new System.Drawing.Size(408, 31);
            this.buttonReconstruction.TabIndex = 7;
            this.buttonReconstruction.Text = "復元ファイルの選択";
            this.buttonReconstruction.UseVisualStyleBackColor = true;
            this.buttonReconstruction.Click += new System.EventHandler(this.buttonReconstruction_Click);
            // 
            // FormControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 321);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormControl";
            this.Text = "CommitPMX";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormControl_FormClosing);
            this.Load += new System.EventHandler(this.FormControl_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.groupBoxArchiveFormat.ResumeLayout(false);
            this.groupBoxArchiveFormat.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonCommit;
        private System.Windows.Forms.Button buttonReCompress;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.GroupBox groupBoxArchiveFormat;
        private System.Windows.Forms.RadioButton radioButtonZip;
        private System.Windows.Forms.RadioButton radioButton7z;
        private System.Windows.Forms.Button buttonReconstruction;
    }
}