using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace JetCode.SendEmail
{
    public partial class FormBase : Form
    {
        public FormBase()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.DesignMode)
                return;

            this.InitializeForm();
        }

        protected virtual void InitializeForm()
        {
        }

        protected void Serialize(string path, object obj)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                new BinaryFormatter().Serialize(fs, obj);
            }
        }

        protected object Deserialize(string path)
        {
            if (!File.Exists(path))
                return null;

            object obj;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                obj = new BinaryFormatter().Deserialize(fs);
            }

            return obj;
        }

        protected void ShowErrorMessage(string error)
        {
            MessageBox.Show(error, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected void ShowMessage(string message)
        {
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
