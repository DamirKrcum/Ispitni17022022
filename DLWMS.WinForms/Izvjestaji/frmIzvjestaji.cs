using Microsoft.Reporting.WinForms;

namespace DLWMS.WinForms.Izvjestaji
{
    public partial class frmIzvjestajiIB20264 : Form
    {
        public frmIzvjestajiIB20264()
        {
            InitializeComponent();
        }
        private void frmIzvjestaji_Load(object sender, EventArgs e)
        {           
            reportViewer1.RefreshReport();
        }
    }
}
