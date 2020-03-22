using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSC.Common
{
    public partial class ChangePasswordForm : Form
    {
        private PersistPassword oldPassword;
        public string NewPassword
        {
            get
            {
                return this.txtPassword.Text;
            }
        }

        public ChangePasswordForm(PersistPassword oldPassword)
        {
            InitializeComponent();
            this.oldPassword = oldPassword;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!oldPassword.Verify(this.txtOld.Text))
            {
                toolTip.Show("密码不正确", txtOld);
                return;
            }
            if (this.txtPassword.Text != this.txtConfirm.Text)
            {
                toolTip.Show("密码不一致", txtConfirm);
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtOld_TextChanged(object sender, EventArgs e)
        {
            toolTip.Hide(txtOld);
        }

        private void txtConfirm_TextChanged(object sender, EventArgs e)
        {
            toolTip.Hide(txtConfirm);
        }
    }
}
