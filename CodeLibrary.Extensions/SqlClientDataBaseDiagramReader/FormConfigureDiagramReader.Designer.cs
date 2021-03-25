
namespace CodeLibrary.Extensions.SqlClientDataBaseDiagramReader
{
    partial class FormConfigureDiagramReader
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
            this.tbConnectionString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btTestConnection = new System.Windows.Forms.Button();
            this.tbFilterTableNames = new System.Windows.Forms.TextBox();
            this.tbFilterColumnNames = new System.Windows.Forms.TextBox();
            this.tbFilterTypeNames = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dialogButton1 = new CodeLibrary.Controls.DialogButton();
            this.cbHeader = new System.Windows.Forms.CheckBox();
            this.cbIncludeForeignKeys = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbConnectionString
            // 
            this.tbConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConnectionString.Location = new System.Drawing.Point(12, 25);
            this.tbConnectionString.Name = "tbConnectionString";
            this.tbConnectionString.Size = new System.Drawing.Size(646, 20);
            this.tbConnectionString.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Connection String";
            // 
            // btTestConnection
            // 
            this.btTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btTestConnection.Location = new System.Drawing.Point(583, 51);
            this.btTestConnection.Name = "btTestConnection";
            this.btTestConnection.Size = new System.Drawing.Size(75, 23);
            this.btTestConnection.TabIndex = 3;
            this.btTestConnection.Text = "Test";
            this.btTestConnection.UseVisualStyleBackColor = true;
            this.btTestConnection.Click += new System.EventHandler(this.btTestConnection_Click);
            // 
            // tbFilterTableNames
            // 
            this.tbFilterTableNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilterTableNames.Location = new System.Drawing.Point(12, 100);
            this.tbFilterTableNames.Name = "tbFilterTableNames";
            this.tbFilterTableNames.Size = new System.Drawing.Size(646, 20);
            this.tbFilterTableNames.TabIndex = 4;
            // 
            // tbFilterColumnNames
            // 
            this.tbFilterColumnNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilterColumnNames.Location = new System.Drawing.Point(12, 147);
            this.tbFilterColumnNames.Name = "tbFilterColumnNames";
            this.tbFilterColumnNames.Size = new System.Drawing.Size(646, 20);
            this.tbFilterColumnNames.TabIndex = 5;
            // 
            // tbFilterTypeNames
            // 
            this.tbFilterTypeNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilterTypeNames.Location = new System.Drawing.Point(12, 192);
            this.tbFilterTypeNames.Name = "tbFilterTypeNames";
            this.tbFilterTypeNames.Size = new System.Drawing.Size(646, 20);
            this.tbFilterTypeNames.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Filter Table Names";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Filter Column Names";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Filter Type Names";
            // 
            // dialogButton1
            // 
            this.dialogButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogButton1.ButtonMode = CodeLibrary.Controls.DialogButton.DialogButtonMode.OkCancel;
            this.dialogButton1.Location = new System.Drawing.Point(502, 302);
            this.dialogButton1.Name = "dialogButton1";
            this.dialogButton1.Size = new System.Drawing.Size(156, 23);
            this.dialogButton1.TabIndex = 0;
            this.dialogButton1.TextCancel = "Cancel";
            this.dialogButton1.TextIgnore = "Ignore";
            this.dialogButton1.TextNo = "No";
            this.dialogButton1.TextOk = "Ok";
            this.dialogButton1.TextRetry = "Retry";
            this.dialogButton1.TextYes = "Yes";
            this.dialogButton1.DialogButtonClick += new CodeLibrary.Controls.DialogButton.DialogButtonClickEventHandler(this.dialogButton1_DialogButtonClick);
            // 
            // cbHeader
            // 
            this.cbHeader.AutoSize = true;
            this.cbHeader.Location = new System.Drawing.Point(12, 223);
            this.cbHeader.Name = "cbHeader";
            this.cbHeader.Size = new System.Drawing.Size(99, 17);
            this.cbHeader.TabIndex = 10;
            this.cbHeader.Text = "Include Header";
            this.cbHeader.UseVisualStyleBackColor = true;
            // 
            // cbIncludeForeignKeys
            // 
            this.cbIncludeForeignKeys.AutoSize = true;
            this.cbIncludeForeignKeys.Location = new System.Drawing.Point(12, 249);
            this.cbIncludeForeignKeys.Name = "cbIncludeForeignKeys";
            this.cbIncludeForeignKeys.Size = new System.Drawing.Size(125, 17);
            this.cbIncludeForeignKeys.TabIndex = 11;
            this.cbIncludeForeignKeys.Text = "Include Foreign Keys";
            this.cbIncludeForeignKeys.UseVisualStyleBackColor = true;
            this.cbIncludeForeignKeys.Visible = false;
            // 
            // FormConfigureDiagramReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 337);
            this.Controls.Add(this.cbIncludeForeignKeys);
            this.Controls.Add(this.cbHeader);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbFilterTypeNames);
            this.Controls.Add(this.tbFilterColumnNames);
            this.Controls.Add(this.tbFilterTableNames);
            this.Controls.Add(this.btTestConnection);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbConnectionString);
            this.Controls.Add(this.dialogButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormConfigureDiagramReader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SqlClient Diagram Reader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.DialogButton dialogButton1;
        private System.Windows.Forms.TextBox tbConnectionString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btTestConnection;
        private System.Windows.Forms.TextBox tbFilterTableNames;
        private System.Windows.Forms.TextBox tbFilterColumnNames;
        private System.Windows.Forms.TextBox tbFilterTypeNames;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbHeader;
        private System.Windows.Forms.CheckBox cbIncludeForeignKeys;
    }
}