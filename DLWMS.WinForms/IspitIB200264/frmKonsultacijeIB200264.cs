using DLWMS.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLWMS.WinForms.IspitIB200264
{
    public partial class frmKonsultacijeIB200264 : Form
    {
        public Student _student { get; set; }
        DLWMSDbContext db = new DLWMSDbContext();
        List<StudentiKonsultacije> studentiKonsultacije = new List<StudentiKonsultacije>();
        string info;
        
        public frmKonsultacijeIB200264(Student st)
        {
            InitializeComponent();
            this._student= st;
            lblStudent.Text = _student.StudentImePrezime();
            dgvKonsultacije.AutoGenerateColumns = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmKonsultacijeIB200264_Load(object sender, EventArgs e)
        {
            Ucitaj();
        }

        private void Ucitaj()
        {
            studentiKonsultacije = db.studentiKonsultacije.Where(st => st.StudentId == _student.Id).Include(st => st._predmet).ToList();
            cmbPredmet.DataSource = db.Predmeti.ToList();
            DataTable tabela = GenerisiTabelu();            
            BindingSource binding = new BindingSource();
            binding.DataSource = tabela;
            dgvKonsultacije.DataSource = binding;
        }

        private DataTable GenerisiTabelu()
        {
            DataTable tabela = new DataTable();
            tabela.Columns.Add("Predmet");
            tabela.Columns.Add("Vrijeme");
            tabela.Columns.Add("Napomena");
            for (int i = 0; i < studentiKonsultacije.Count(); i++)
            {
                var red = tabela.NewRow();
                var predmet = studentiKonsultacije[i]._predmet.Naziv;
                var vrijeme = studentiKonsultacije[i].Datum;
                var napomena = studentiKonsultacije[i].Napomena;

                red["Predmet"] = predmet;
                red["Vrijeme"] = vrijeme;
                red["Napomena"] = napomena;

                tabela.Rows.Add(red);
            }
            return tabela;
        }

        private void dgvKonsultacije_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 3)
            {
                var konsultacija = studentiKonsultacije[e.RowIndex];
                if(konsultacija.Datum > DateTime.Now)
                {
                    if(MessageBox.Show("Jeste li sigurni da zelite obrisati zapis?","Upozorenje!", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                        db.studentiKonsultacije.Remove(konsultacija);
                        db.SaveChanges();
                        MessageBox.Show("Konsultacija obrisana!");
                        }
                   
                    Ucitaj();
                }

                else
                {
                    MessageBox.Show("Nije moguće obrisati konsultacije datuma starijeg od trenutnog!");

                }
            }
        }

        private void DodajZahtjev_Click(object sender, EventArgs e)
        {
            var studentid = _student.Id;
            var nova = new frmNovaKonsultacijaIB200264(studentid);
            nova.ShowDialog();
            Ucitaj();
        }

        private void Printaj_Click(object sender, EventArgs e)
        {
          var izvjestaj = new frmIzvjestajIB200264(_student);
          izvjestaj.ShowDialog();
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            Task.Run(() => {
                string informacija = "";
                var brojZahtjeva = int.Parse(txtBrojZahtjeva.Text);
                var predmet = cmbPredmet.SelectedItem as Predmet;
                for (int i = 0; i < brojZahtjeva; i++)
                {
                    StudentiKonsultacije zahtjev = new StudentiKonsultacije()
                    {
                        StudentId = _student.Id,
                        PredmetId = predmet.Id,
                        Datum = DateTime.Now,
                        Napomena = $"Genericka napomena za predmet {(cmbPredmet.SelectedItem as Predmet).Naziv}",

                    };
                    db.studentiKonsultacije.Add(zahtjev);
                    informacija += $"Za {_student.StudentImePrezime()} dodat zahtjev za konsultacije - > {predmet.Naziv} {zahtjev.Datum} {Environment.NewLine}";
                    BeginInvoke(() => { DodajInfo(informacija); });
                    Thread.Sleep(500);
                }

                db.SaveChanges();
            });
            
        }

        private void DodajInfo(string informacija)
        {
            txtInfo.Text += informacija;
            txtInfo.SelectionStart= txtInfo.Text.Length;
            txtInfo.ScrollToCaret();
        }
    }
}
