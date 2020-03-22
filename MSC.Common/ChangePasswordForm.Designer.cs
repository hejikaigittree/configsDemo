namespace MSC.Common
{
    partial class ChangePasswordForm
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
            this.components = new System.ComponentModel.Container();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnCancle = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.lblOldPassword = new System.Windows.Forms.Label();
            this.txtOld = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lblNewPassword
            // 
            this.lblNewPassword.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblNewPassword.AutoSize = true;
            this.lblNewPassword.Font = new System.Drawing.Font("宋体", 12F);
            this.lblNewPassword.Location = new System.Drawing.Point(36, 71);
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.Size = new System.Drawing.Size(64, 16);
            this.lblNewPassword.TabIndex = 6;
            this.lblNewPassword.Text = "新密码:";
            this.lblNewPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPassword.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPassword.Location = new System.Drawing.Point(124, 67);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(148, 26);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // btnCancle
            // 
            this.btnCancle.Font = new System.Drawing.Font("宋体", 12F);
            this.btnCancle.Location = new System.Drawing.Point(152, 151);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(100, 29);
            this.btnCancle.TabIndex = 4;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Font = new System.Drawing.Font("宋体", 12F);
            this.btnOK.Location = new System.Drawing.Point(36, 151);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 29);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtConfirm
            // 
            this.txtConfirm.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtConfirm.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtConfirm.Location = new System.Drawing.Point(124, 109);
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.Size = new System.Drawing.Size(148, 26);
            this.txtConfirm.TabIndex = 2;
            this.txtConfirm.UseSystemPasswordChar = true;
            this.txtConfirm.TextChanged += new System.EventHandler(this.txtConfirm_TextChanged);
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Font = new System.Drawing.Font("宋体", 12F);
            this.lblConfirmPassword.Location = new System.Drawing.Point(21, 113);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(80, 16);
            this.lblConfirmPassword.TabIndex = 7;
            this.lblConfirmPassword.Text = "确认密码:";
            // 
            // lblOldPassword
            // 
            this.lblOldPassword.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblOldPassword.AutoSize = true;
            this.lblOldPassword.Font = new System.Drawing.Font("宋体", 12F);
            this.lblOldPassword.Location = new System.Drawing.Point(36, 29);
            this.lblOldPassword.Name = "lblOldPassword";
            this.lblOldPassword.Size = new System.Drawing.Size(64, 16);
            this.lblOldPassword.TabIndex = 5;
            this.lblOldPassword.Text = "旧密码:";
            this.lblOldPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOld
            // 
            this.txtOld.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtOld.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOld.Location = new System.Drawing.Point(125, 25);
            this.txtOld.Name = "txtOld";
            this.txtOld.Size = new System.Drawing.Size(147, 26);
            this.txtOld.TabIndex = 0;
            this.txtOld.UseSystemPasswordChar = true;
            this.txtOld.TextChanged += new System.EventHandler(this.txtOld_TextChanged);
            // 
            // ChangePasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 199);
            this.Controls.Add(this.lblOldPassword);
            this.Controls.Add(this.lblNewPassword);
            this.Controls.Add(this.txtOld);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtConfirm);
            this.Controls.Add(this.lblConfirmPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ChangePasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "修改密码";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtConfirm;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.Label lblOldPassword;
        private System.Windows.Forms.TextBox txtOld;
        private System.Windows.Forms.ToolTip toolTip;
    }
}