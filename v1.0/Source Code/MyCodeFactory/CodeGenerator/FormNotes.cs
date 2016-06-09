using System.Windows.Forms;

namespace CodeGenerator
{
    public partial class FormNotes : Form
    {
        public FormNotes()
        {
            InitializeComponent();
        }

        public FormNotes(string notes)
        {
            InitializeComponent();

            this.txtNotes.Text = notes;
        }
    }
}
