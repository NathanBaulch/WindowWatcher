using System.Windows.Forms;

namespace WindowWatcher
{
    public class BaseView : UserControl
    {
        public virtual void Initialize(LogRepository repository)
        {
        }

        public virtual void Selected()
        {
        }

        public virtual void Destroy()
        {
        }
    }
}