using DLWMS.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLWMS.WinForms.IspitIB200264
{
    public partial class frmPretraga200264 : Form
    {
        List<StudentiPredmeti> studentiPredmeti= new List<StudentiPredmeti>();
        List<Student> students= new List<Student>();

        DLWMSDbContext db = new DLWMSDbContext();
        public frmPretraga200264()
        {
            InitializeComponent();
            dgvPretraga.AutoGenerateColumns = false;
            Ucitaj();
        }

        private void Ucitaj()
        {
            var lista = db.Studenti.ToList();

            students.Clear();
            students = lista.Where(st => Filtriraj(st)).ToList();


            DataTable tabela = new DataTable();
            tabela.Columns.Add("ImePrezime");
            tabela.Columns.Add("Godina");
            tabela.Columns.Add("Prosjek");

            for (int i = 0; i < students.Count(); i++)
            {
                var red = tabela.NewRow();
                var student = students[i];
                red["ImePrezime"] = student.StudentImePrezime();
                red["Godina"] = student.GodinaStudija;
                red["Prosjek"] = IzracunajProsjek(student);
                tabela.Rows.Add(red);
            }
            
            BindingSource binding = new BindingSource();
            binding.DataSource = tabela;
            dgvPretraga.DataSource = binding;

        }

        private double IzracunajProsjek(Student student)
        {
            var lst = db.studentiPredmeti.Where(st => st.StudentId == student.Id).ToList();
            if (lst.Count == 0)
                return 5;
            var prosjek = Math.Round(lst.Average(st => st.Ocjena), 2);
            return prosjek;
        }

        private bool Filtriraj(Student st)
        {

            return FiltrirajPretragu(st) && ValidirajGodinu(st);
            
        }

        private bool ValidirajGodinu(Student st)
        {
            int godina = cmbGodina.SelectedIndex + 1;
            if (godina == 0)
                return true;
            else           
                return st.GodinaStudija == godina;
            
            
        }

        private bool FiltrirajPretragu(Student st)
        {
            string filter = txtPretraga.Text.ToLower();
            return string.IsNullOrEmpty(txtPretraga.Text) ||
                   st.Ime.ToLower().Contains(filter) ||
                   st.Prezime.ToLower().Contains(filter);
        }

        private void txtPretraga_TextChanged(object sender, EventArgs e)
        {
            Ucitaj();
        }

        private void cmbGodina_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ucitaj();
        }

        private void dgvPretraga_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 3)
            {
               //dgv data source je DataTable čiji indeksi redova odgovaraju indeksu liste studenata

                var student = students[e.RowIndex];

                var konsultacije = new frmKonsultacijeIB200264(student);
                konsultacije.ShowDialog();
            }
        }
    }
}
