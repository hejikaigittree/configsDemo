using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml.Serialization;

namespace MSC.Common
{
    public class ChangePasswordEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (service != null)
            {
                PersistPassword password = (value as PersistPassword).Clone() as PersistPassword;    // 使用Clone对象，触发PropertyValueChanged消息
                ChangePasswordForm dialog = new ChangePasswordForm(password);
                if (service.ShowDialog(dialog) == DialogResult.OK)
                {
                    password.Set(dialog.NewPassword);
                    return password;
                }
            }
            return value;
        }
    }
}
