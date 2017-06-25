using System;
using System.Windows.Forms;

namespace WindowWatcher
{
    public class CursorScope : IDisposable
    {
        public CursorScope(Cursor cursor)
        {
            Cursor.Current = cursor;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Cursor.Current = Cursors.Default;
        }

        #endregion
    }
}