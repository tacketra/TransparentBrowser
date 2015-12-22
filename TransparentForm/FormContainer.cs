using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransparentForm
{
    class FormContainer : Form
    {
        public FormContainer() : base()
        {
            this.IsMdiContainer = true;
            Form1 actualForm = new Form1();
            actualForm.MdiParent = this;
            actualForm.Show();
        }
    }
}
