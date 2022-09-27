using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RWilliams_CIT234_Final
{
    public class NoNegatives : Exception
    {
        public NoNegatives() { }

        public NoNegatives(string ex)
        {
            MessageBox.Show(ex);
        }

        public NoNegatives(string ex, string innerEx)
        {
            MessageBox.Show(ex + "\n" + innerEx);
        }
    }
}
