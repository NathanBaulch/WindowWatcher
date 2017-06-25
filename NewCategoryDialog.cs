using System.Windows.Forms;

namespace WindowWatcher
{
    public partial class NewCategoryDialog : Form
    {
        public NewCategoryDialog()
        {
            InitializeComponent();
        }

        public string CategoryName => _nameBox.Text;
    }
}