using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RWilliams_CIT234_Final
{
    class LessThanTen : Exception
    {
        public LessThanTen() { }
        public LessThanTen(string ex)
        {
            MessageBox.Show(ex);
        }
        public LessThanTen(string ex, string innerEx)
        {
            MessageBox.Show(ex + "\n" + innerEx);
        }
    }
}
