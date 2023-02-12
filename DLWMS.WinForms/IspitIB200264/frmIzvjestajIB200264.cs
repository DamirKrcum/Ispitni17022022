using DLWMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.WinForms;
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
    public partial class frmIzvjestajIB200264 : Form
    {
        public Student _Student { get; }
        List<StudentiKonsultacije> studentiKonsultacije;
        DLWMSDbContext db = new DLWMSDbContext();

        public frmIzvjestajIB200264(Student student)
        {
            InitializeComponent();
           _Student = student;
           studentiKonsultacije = db.studentiKonsultacije.Where(st=>st.StudentId== _Student.Id).Include(pr=>pr._predmet).ToList();
           var brojKonsultacija = studentiKonsultacije.Count();
           
           DataTable tabela = GenerisiTabelu();
           ReportDataSource rds = new ReportDataSource();
           rds.Name = "DataSet1";
           rds.Value = tabela;
               
           ReportParameterCollection rpc = new ReportParameterCollection();
           rpc.Add(new ReportParameter("pImePrezime", _Student.StudentImePrezime()));
           rpc.Add(new ReportParameter("pBrojZahtjeva", brojKonsultacija.ToString()));
           //rpc.Add(new ReportParameter("pBrojZahtjeva", brojKonsultacija.ToString()));
           
           //rpc.Add(new ReportParameter("pIndeks", _Student.BrojIndeksa));
           
           reportViewer1.LocalReport.SetParameters(rpc);
           reportViewer1.LocalReport.DataSources.Add(rds);
           reportViewer1.RefreshReport();
        }

        private DataTable GenerisiTabelu()
        {
            DataTable tabela = new DataTable();
            tabela.Columns.Add("Rb");
            tabela.Columns.Add("Predmet");
            tabela.Columns.Add("Vrijeme");
            tabela.Columns.Add("Napomena");
            for (int i = 0; i < studentiKonsultacije.Count(); i++)
            {
                var red = tabela.NewRow();
                var predmet = studentiKonsultacije[i]._predmet.Naziv;
                var vrijeme = studentiKonsultacije[i].Datum;
                var napomena = studentiKonsultacije[i].Napomena;

                red["Rb"] = (i + 1).ToString();
                red["Predmet"] = predmet;
                red["Vrijeme"] = vrijeme;
                red["Napomena"] = napomena;

                tabela.Rows.Add(red);

            }
            return tabela;
        }
    }
}
