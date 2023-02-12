using DLWMS.Data;
using DLWMS.WinForms.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLWMS.WinForms
{
    public partial class frmNovaKonsultacijaIB200264 : Form
    {
        DLWMSDbContext db = new DLWMSDbContext();
        int _studentId;



        public frmNovaKonsultacijaIB200264(int studentID)
        {
            InitializeComponent();
            cmbPredmeti.DataSource = db.Predmeti.ToList();
            _studentId = studentID;
  

           
        }

        private void btnSacuvaj_Click(object sender, EventArgs e)
        {
            if(Validator.ValidirajKontrolu(txtNapomena,err, Kljucevi.ObaveznaVrijednost) 
                && Validator.ValidirajKontrolu(cmbPredmeti, err, Kljucevi.ObaveznaVrijednost) 
                && Validator.ValidirajKontrolu(dtpDatumKonsultacija, err, Kljucevi.ObaveznaVrijednost))
            {

                var konsultacija = new StudentiKonsultacije()
                {
                    StudentId = _studentId,
                    PredmetId = (cmbPredmeti.SelectedItem as Predmet).Id,
                    Datum = dtpDatumKonsultacija.Value,
                    Napomena = txtNapomena.Text
                };
                db.studentiKonsultacije.Add(konsultacija);
                db.SaveChanges();
                MessageBox.Show("Konsultacija dodana!");
                Close();
            }
            
        }

        private void btnOdustani_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
