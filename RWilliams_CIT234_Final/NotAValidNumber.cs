using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RWilliams_CIT234_Final
{
    class NotAValidNumber : Exception
    {
        public NotAValidNumber() { }
        public NotAValidNumber(string ex)
        {
            MessageBox.Show(ex);
        }
        public NotAValidNumber(string ex, string innerEx)
        {
            MessageBox.Show(ex + "\n" + innerEx);
        }
    }
}
