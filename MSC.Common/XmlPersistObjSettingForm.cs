using System;
using System.IO;
using System.Windows.Forms;

namespace MSC.Common
{
    public partial class XmlPersistObjSettingForm<TClass> : Form where TClass : IPersistObj, new()
    {
        private string title;
        private string xmlFile;
        private bool needSave = false;

        private string FormTitle { get { return title + " - " + xmlFile + (needSave ? "*" : ""); } }

        public TClass Setting { get; private set; }

        public XmlPersistObjSettingForm(string title, string xmlFile, bool collapse = true)
        {
            InitializeComponent();

            this.title = title;
            this.xmlFile = Path.GetFullPath(xmlFile);
            this.Setting = Tools.DeserializeFromXmlFile<TClass>(this.xmlFile);
            this.Text = FormTitle;
            this.propertyGrid.SelectedObject = this.Setting;
            if (collapse)
            {
                this.propertyGrid.CollapseAllGridItems();
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            this.Setting = Tools.DeserializeFromXmlFile<TClass>(this.xmlFile);
            this.propertyGrid.SelectedObject = this.Setting;
            needSave = false;
            Text = FormTitle;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Setting.OnSave();
            Tools.SerializeToXmlFile(this.Setting, this.xmlFile);
            needSave = false;
            Text = FormTitle;
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            this.propertyGrid.SelectedObject = this.Setting;
            needSave = true;
            Text = FormTitle;
        }

        private void XmlPersistObjSettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (needSave)
            {
                if (MessageBox.Show("设置未保存，是否保存？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    btnSave_Click(sender, e);
                }
            }
        }
    }

    public interface IPersistObj
    {
        void OnSave();
    }
}
