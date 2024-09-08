using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

//using System.IO;
//using System.Xml.Serialization;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;


namespace ProyectoCOM_03
{
    public partial class F_FILE : Form
    {
        private string DADRESS = null;
        public F_FILE()
        {
            InitializeComponent();
        }
        private void B_NEW_Click(object sender, EventArgs e)
        {
            MainForm MF = this.Parent.Parent as MainForm;
            MF.OpenForm<F_PARAM>();
        }
        private void B_OPEN_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter= "Esquema Electrico|*.xlsx";
            openFileDialog1.Title = "Obri el Esquema";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                MainForm MF = this.Parent.Parent as MainForm;
                F_TREE TF = MF.OpenForm<F_TREE>() as F_TREE;
                ConvertExceltoObj(TF,openFileDialog1.FileName);

            }
        }
        private void B_SAVE_Click(object sender, EventArgs e)
        {
            MainForm MF = this.Parent.Parent as MainForm;
            F_TREE TF = MF.FACTIVE<F_TREE>() as F_TREE;
            if(TF != null)
            {
                if (DADRESS != null)
                {
                    ConvertObjtoExcel(TF,DADRESS);
                }
                else
                {
                    B_SAVEAS_Click(sender, e);
                }
            }
        }
        private void B_SAVEAS_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Esquema Electrico|*.xlsx";
            saveFileDialog1.Title = "Guarda el Esquema";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                /* System.IO.FileStream fs =(System.IO.FileStream)saveFileDialog1.OpenFile();
                 this.button2.Image.Save(fs,System.Drawing.Imaging.ImageFormat.Jpeg);}
                  fs.Close();*/
                DADRESS = saveFileDialog1.FileName;
                MainForm MF = this.Parent.Parent as MainForm;
                F_TREE TF = MF.FACTIVE<F_TREE>() as F_TREE;
                ConvertObjtoExcel(TF, saveFileDialog1.FileName);
            }
        }
        //**********GUARDAR**********//
        public void ConvertObjtoExcel(F_TREE TROOT,string adress= "C:\\CE\\default.xlsx")
        {
            Excel.Application excel = new Microsoft.Office.Interop.Excel.Application(); //inicializar EXCEL i AUTOCAD
            object misvalue = System.Reflection.Missing.Value;
            Workbook wb = excel.Workbooks.Add(misvalue); //  wb = excel.Workbooks.Open(filename);
            Worksheet ws = wb.Worksheets[1];
            int i = 2;
            int level = 0;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                writeLIST(TROOT.IG, ref ws, ref level, ref i);
                level = level + 1;
                writeLOOP(TROOT.IG, ref ws, ref level, ref i);
                wb.SaveAs(adress);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("L'arxiu s'ha guardat correctament");
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("No s'ha pogut guardar l'arxiu");
            }
            wb.Close();
        }
        private void writeLOOP(COMPONENTE CP, ref Worksheet ws, ref int level, ref int i)
        {
            for (int r = 0; r < CP.LIST.Count; r++)
            {
                COMPONENTE CH=CP.LIST.ElementAt(r);
                writeLIST(CH,ref  ws, ref level, ref i);
                if (CH.LIST != null)
                {
                    level=level+1;
                    writeLOOP(CH, ref ws,ref  level,ref i);
                    level =level-1;
                }
            }
        }
        private void writeLIST(COMPONENTE CH,ref Worksheet ws, ref int level,ref int i)             //bucle per anar creant els fills
        {
            CIRCUIT CTEMP = CH as CIRCUIT;
            TERMICO TTEMP = CH as TERMICO;
            DIFF DTEMP = CH as DIFF;
            CONDUCTOR LTEMP = CH as CONDUCTOR;
            if (CH != null)
            {
                writecell(ws, level.ToString(), 'A', i);
                writecell(ws, CH.trif.ToString(), 'E', i);
                writecell(ws, CH.IR.ToString(), 'J', i);
                writecell(ws, CH.IS.ToString(), 'K', i);
                writecell(ws, CH.IT.ToString(), 'L', i);
                writecell(ws, CH.IMR.ToString(), 'M', i);
                writecell(ws, CH.IMS.ToString(), 'N', i);
                writecell(ws, CH.IMT.ToString(), 'O', i);
                if (CTEMP != null)
                {//circuito
                    writecell(ws, "C", 'C', i);
                    writecell(ws, CTEMP.NAME, 'F', i);
                    writecell(ws, CTEMP.P.ToString(), 'D', i);
                    writecell(ws, CTEMP.tipo.ToString(), 'G', i);
                    writecell(ws, CTEMP.IC.ToString(), 'H', i);
                }
                else if (TTEMP != null)
                {//Termico
                    writecell(ws, "T", 'C', i);
                    writecell(ws, TTEMP.IC.ToString(), 'D', i);
                    writecell(ws, TTEMP.ICC.ToString(), 'F', i);
                    writecell(ws, TTEMP.CURVA.ToString(), 'G', i);
                }
                else if (DTEMP != null)
                {//Diferencial
                    writecell(ws, "D", 'C', i);
                    writecell(ws, DTEMP.IC.ToString(), 'D', i);
                    writecell(ws, DTEMP.IDD.ToString(), 'F', i);
                    writecell(ws, DTEMP.TIPO.ToString(), 'G', i);
                }
                else if (LTEMP != null)
                {//Conductor
                    writecell(ws, "L", 'C', i);
                    writecell(ws, LTEMP.L.ToString(), 'F', i);
                    writecell(ws, LTEMP.S.ToString(), 'G', i);
                    writecell(ws, LTEMP.Aisl.ToString(), 'H', i);
                    writecell(ws, LTEMP.Aisl.ToString(), 'I', i);
                }
            }
            i = i + 1;
        }
        public void writecell(Worksheet ws, string str2="??", char c='A', int i=1)
        {
            string str = c.ToString() + i.ToString() + ':' + c.ToString() + i.ToString();
            ws.Range[str].Value= str2;
        }
        //**********OBRIR**********//
        void ConvertExceltoObj(F_TREE TROOT,string adress)
        {

            Excel.Application excel = new Microsoft.Office.Interop.Excel.Application(); //inicializar EXCEL i AUTOCAD
            object misvalue = System.Reflection.Missing.Value;
            Workbook wb = excel.Workbooks.Open(adress);
            Worksheet ws = wb.Worksheets[1];
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            int i = 2;
            try
            {
                COMPONENTE IG = new COMPONENTE();
                IG= READLIST(ref ws, ref i, ref TROOT);
                TROOT.IG = IG;
                IG.PADRE = null;
                int level = 0;
                i++;
                READLOOP2(ref TROOT.IG, ref ws, ref i ,ref level,ref TROOT);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("L'arxiu s'ha obert correctament");
                TROOT.ACTNODES();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("Format d'arxiu incorrecte");
            }
            wb.Close();
        }

        private void READLOOP(COMPONENTE CP,  Worksheet ws,  int i, F_TREE TROOT)
        {

            bool IG = true;
            COMPONENTE CH = new COMPONENTE();
            int level = int.Parse(readcell(ws, 'A', i));
            while (readcell(ws, 'A', i) != null && readcell(ws, 'A', i) == level.ToString())
            {

                switch (readcell(ws, 'C', i))
                {
                    case "T":   //Termico
                        double II = (Double.Parse(readcell(ws, 'D', i)));
                        bool trif = bool.Parse(readcell(ws, 'E', i));
                        double icc = double.Parse(readcell(ws, 'F', i));

                        string tip = readcell(ws, 'G', i);
                        TERMICO TTEMP = new TERMICO(Double.Parse(readcell(ws, 'D', i)), bool.Parse(readcell(ws, 'E', i)), double.Parse(readcell(ws, 'F', i)), readcell(ws, 'G', i));
                        TTEMP.IR = double.Parse(readcell(ws, 'J', i));
                        TTEMP.IS = double.Parse(readcell(ws, 'L', i));
                        TTEMP.IT = double.Parse(readcell(ws, 'L', i));
                        TTEMP.IMR = double.Parse(readcell(ws, 'M', i));
                        TTEMP.IMS = double.Parse(readcell(ws, 'N', i));
                        TTEMP.IMT = double.Parse(readcell(ws, 'O', i));
                        TTEMP.PADRE = CP;
                        CP.LIST.Add(TTEMP);
                        CH = TTEMP;
                        break;
                    case "D":    //Diferencial
                        DIFF DTEMP = new DIFF(int.Parse(readcell(ws, 'D', i)), bool.Parse(readcell(ws, 'E', i)), int.Parse(readcell(ws, 'F', i)), readcell(ws, 'G', i));
                        DTEMP.IR = double.Parse(readcell(ws, 'J', i));
                        DTEMP.IS = double.Parse(readcell(ws, 'L', i));
                        DTEMP.IT = double.Parse(readcell(ws, 'L', i));
                        DTEMP.IMR = double.Parse(readcell(ws, 'M', i));
                        DTEMP.IMS = double.Parse(readcell(ws, 'N', i));
                        DTEMP.IMT = double.Parse(readcell(ws, 'O', i));
                        DTEMP.PADRE = CP;
                        CP.LIST.Add(DTEMP);
                        CH = DTEMP;
                        break;
                    case "C":   //Circuito
                        CIRCUIT CTEMP = new CIRCUIT(readcell(ws, 'F', i), int.Parse(readcell(ws, 'D', i)), bool.Parse(readcell(ws, 'E', i)), int.Parse(readcell(ws, 'G', i)));
                        CTEMP.IR = double.Parse(readcell(ws, 'J', i));
                        CTEMP.IS = double.Parse(readcell(ws, 'L', i));
                        CTEMP.IT = double.Parse(readcell(ws, 'L', i));
                        CTEMP.IMR = double.Parse(readcell(ws, 'M', i));
                        CTEMP.IMS = double.Parse(readcell(ws, 'N', i));
                        CTEMP.IMT = double.Parse(readcell(ws, 'O', i));
                        CTEMP.PADRE = CP;
                        CP.LIST.Add(CTEMP);
                        CH = CTEMP;
                        break;
                    case "L":   //Conductor
                        CONDUCTOR LTEMP = new CONDUCTOR(double.Parse(readcell(ws, 'F', i)), double.Parse(readcell(ws, 'G', i)), int.Parse(readcell(ws, 'I', i)) == 1 ? true : false, bool.Parse(readcell(ws, 'E', i)));
                        LTEMP.IR = double.Parse(readcell(ws, 'J', i));
                        LTEMP.IS = double.Parse(readcell(ws, 'L', i));
                        LTEMP.IT = double.Parse(readcell(ws, 'L', i));
                        LTEMP.IMR = double.Parse(readcell(ws, 'M', i));
                        LTEMP.IMS = double.Parse(readcell(ws, 'N', i));
                        LTEMP.IMT = double.Parse(readcell(ws, 'O', i));
                        CP.PADRE = CP;
                        CP.LIST.Add(LTEMP);
                        CH = LTEMP;
                        break;
                }
                i += 1;
                if (IG&&CH.GetType()==typeof(TERMICO))
                {
                    IG = false;
                    TROOT.IG = CH as TERMICO;

                }
                if (readcell(ws, 'A', i) != null && level < int.Parse(readcell(ws, 'A', i)))
                {
                    READLOOP(CH,ws,i,TROOT);
                }

            }
        }
        private void READLOOP2(ref COMPONENTE CP, ref Worksheet ws, ref int i, ref int level, ref F_TREE TROOT)
        {
            level = int.Parse(readcell(ws, 'A', i));
            while (readcell(ws, 'A', i) != null && readcell(ws, 'A', i) == level.ToString())
            {
                COMPONENTE CH = READLIST(ref ws, ref i, ref TROOT);
                CH.PADRE = CP;
                CP.LIST.Add(CH);

                i ++;
                if (level < int.Parse(readcell(ws, 'A', i)))
                {
                    READLOOP2(ref CH, ref ws, ref i, ref level, ref TROOT);
                    i++;
                }
            }
            i--;
            level--;
        }
        private COMPONENTE READLIST(ref Worksheet ws, ref int i, ref F_TREE TROOT)
        {
            COMPONENTE CH = new COMPONENTE();
            switch (readcell(ws, 'C', i))
            {
                case "T":   //Termico
                    TERMICO TTEMP = new TERMICO(Double.Parse(readcell(ws, 'D', i)), bool.Parse(readcell(ws, 'E', i)), double.Parse(readcell(ws, 'F', i)), readcell(ws, 'G', i));
                    CH = TTEMP;
                    break;
                case "D":    //Diferencial
                    DIFF DTEMP = new DIFF(int.Parse(readcell(ws, 'D', i)), bool.Parse(readcell(ws, 'E', i)), int.Parse(readcell(ws, 'F', i)), readcell(ws, 'G', i));
                    CH = DTEMP;
                    break;
                case "C":   //Circuito
                    //CIRCUIT CTEMP = new CIRCUIT(readcell(ws, 'F', i), int.Parse(readcell(ws, 'D', i)), bool.Parse(readcell(ws, 'E', i)), int.Parse(readcell(ws, 'G', i)));
                    CIRCUIT CTEMP = new CIRCUIT(TROOT, readcell(ws, 'F', i), 10, int.Parse(readcell(ws, 'D', i)), bool.Parse(readcell(ws, 'E', i)), int.Parse(readcell(ws, 'G', i)));
                    CTEMP.IC = double.Parse(readcell(ws, 'H', i));
                    CH = CTEMP;
                    break;
                case "L":   //Conductor
                    CONDUCTOR LTEMP = new CONDUCTOR(double.Parse(readcell(ws, 'F', i)), double.Parse(readcell(ws, 'G', i)), int.Parse(readcell(ws, 'I', i)) == 1 ? true : false, bool.Parse(readcell(ws, 'E', i)));
                    CH = LTEMP;
                    break;
                default:
                    CH = new COMPONENTE();
                    break;
            }
            CH.IR = double.Parse(readcell(ws, 'J', i)); //Corrientes max i min
            CH.IS = double.Parse(readcell(ws, 'L', i));
            CH.IT = double.Parse(readcell(ws, 'L', i));
            CH.IMR = double.Parse(readcell(ws, 'M', i));
            CH.IMS = double.Parse(readcell(ws, 'N', i));
            CH.IMT = double.Parse(readcell(ws, 'O', i));
            return CH;
        }
        string readcell(Worksheet ws,char c, int i)
        {
            string str;
            str = c.ToString() + i.ToString() + ':' + c.ToString() + i.ToString();
            string str2;
                try
            {
                str2= ws.Range[str].Value.ToString();
            }
            catch
            {
                if (ws.Range[str].Value == null)
                {
                    str2 = "0";
                }
                else
                {
                    double jj = ws.Range[str].Value;
                    str2 = jj.ToString();
                }
            }
            return str2;
        }
        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
        }
    }
}
