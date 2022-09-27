using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RWilliams_CIT234_Final
{
    class EmptyBox : Exception
    {
        public EmptyBox() { }
        public EmptyBox(string ex) 
        {
            MessageBox.Show(ex);
        }
        public EmptyBox(string ex, string innerEx)
        {
            MessageBox.Show(ex + "\n" + innerEx);
        }
    }
}
