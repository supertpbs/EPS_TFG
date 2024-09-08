using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;//per donar missatjes a l'usuari

namespace ProyectoCOM_03
{
    public partial class F_PARAM : Form
    {
        bool elevada;
        public F_PARAM()
        {
            InitializeComponent();
            RE_GEN.Hide();
            elevada = false;
        }
        private void BT_Avance(object sender, EventArgs e)
        {
            MainForm DOCKPARENT = this.Parent.Parent as MainForm;
            F_TREE TROOT = DOCKPARENT.OpenForm<F_TREE>() as F_TREE;
            if (RE_GEN.Visible == false)
            {
                createcircuits(TROOT);

                bool fas = TROOT.treegen();
                TROOT.DIMTREE(fas);
                TROOT.ACTNODES();
            }
        }
        
        private int nlineas(decimal n, int nummax)
        {
            int nn = Convert.ToInt32(n);
            if (nn == 0)
            {
                return 0;
            }else
            {
                int j = nn / nummax;
                if ((n % nummax) != 0)
                {
                    j = j + 1;
                }
                return j;
            }
        }
        
        public string lmult(string str, decimal numeric, int nummax, int j)
        {
            int nn = Convert.ToInt32(numeric);
            if (nn == 0)
            {
                j = 0;
            }
            else
            {
                j = nn / nummax;
                if ((numeric % nummax) != 0)
                {
                    j = j + 1;
                }
            }

            j = j + 1;
            if (j > 1)   // al menos 1 obligatorio
            {
                str = str + " " + j.ToString();
            }
            return str;
        }
        private void addlist(F_TREE ROOT, string name, NumericUpDown num, int maxn=10, int ii=10, bool trif=false, int type=12, int Long=15,int P=0, bool oblig =false)
        {

            int maxc = Convert.ToInt32(num.Value);
            for (int i = 0; i <maxc; i++)  //determinem el nombre de circuits neesaris i els creem
            {
                CIRCUIT C = new CIRCUIT(ROOT, lmult(name, num.Value, 30, i), ii, P,false, type, Long);//PLLUM
            }
        }
        private void createcircuits(F_TREE TROOT)
        {
            TROOT.LIST_1_2_3_4_5_10_11.Clear();
            TROOT.LIST_6_7.Clear();
            TROOT.LIST_8_9_12.Clear();//calefacció i AA
            TROOT.LIST_13.Clear();//PRVE
            TROOT.LIST_14.Clear();//Altres DI
            TROOT.LIST_15.Clear();//subcuadres
            TROOT.LIST_16.Clear();
            /*
            TROOT.LIST_1_2_3_4_5_10_11 = new List<CIRCUIT>();//iluminació  endolls generals  cuina i forn  endolls bany i cuina lavadora secadora
            TROOT.LIST_6_7 = new List<CIRCUIT>();//iluminació ext i Endolls exteriors
            TROOT.LIST_8_9_12 = new List<CIRCUIT>();//calefacció i AA
            TROOT.LIST_13 = new List<CIRCUIT>();//PRVE
            TROOT.LIST_14 = new List<CIRCUIT>();//Altres DI
            TROOT.LIST_15 = new List<CIRCUIT>();//subcuadres
            */
            for (int i = 0; i < num_ilum.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Enllumenat",10, 1100, false, 1); }
            for (int i = 0; i < num_ench.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Endolls generals",   16, 1800, false, 2); }
            for (int i = 0; i < num_forn.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Vitro/Forn",         25, 3000, false, 3); }
            for (int i = 0; i < num_rent.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Rentadora",          16, 1800, false, 4); }
            for (int i = 0; i < num_termo.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Termo", 16, 2000, false, 4); }
            for (int i = 0; i < num_enchcocina.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Endolls Bany/Cuina", 16, 2200, false, 5); }
            for (int i = 0; i < num_ilum_ext.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Enllum. Exterior",   10,  900, false, 6); }
            for (int i = 0; i < num_ench_ext.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Endolls Exterior", 16, 1200, false, 7); }

            for (int i = 0; i < num_calef.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Calefacció",         16, 2500, ch_calef.Checked, 8); }
            for (int i = 0; i < num_aa_int.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "a/A ud. Int.", 16, 1200, ch_aa_int.Checked, 9); }
            for (int i = 0; i < num_aa_ext.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "A/A ud. Ext.", 16, 1200, ch_aa_ext.Checked, 9); }
            for (int i = 0; i < num_sec.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Secadora", 16, 1200, false, 10); }

            if (ch_domot.Checked)
            {
                for (int i = 0; i < 1; i++) { CIRCUIT C = new CIRCUIT(TROOT, "DOMOTICA", 16, 300, false, 11, 5); }                      //modificar
            }
            if (ch_cctv.Checked)
            {
                for (int i = 0; i < 1; i++) { CIRCUIT C = new CIRCUIT(TROOT, "CCTV", 10, 500, false, 11,5); }                      //modificar
            }
            if (ch_rack.Checked)
            {
                for (int i = 0; i < 1; i++) { CIRCUIT C = new CIRCUIT(TROOT, "TELEC.", 10,300, false, 11, 5); }                      //modificar
            }
            for (int i = 0; i < num_cort.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Cortiners", 10, 900, false, 12); }
            for (int i = 0; i < num_prve.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "PRVE", 25,Convert.ToInt32(p_prve.Value),ch_prve.Checked, 13); }
            for (int i = 0; i < num_bomba.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Grup Pressió", 25, 2000, false, 13); }
            for (int i = 0; i < num_motor.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "Motor Porta", 16, 1500, false,12); }
            for (int i = 0; i < num_scpis.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "SC Piscina", 20, Convert.ToInt32(p_scpis.Value), ch_scpis.Checked, 15); }
            for (int i = 0; i < num_scsm.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "SC SM", 25, Convert.ToInt32(p_scsm.Value), ch_scsm.Checked, 15); }
            for (int i = 0; i < num_scclima.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "SC Clima", 25, Convert.ToInt32(p_scclima.Value), ch_scclima.Checked, 15); }

            for (int i = 0; i < num_scfv.Value; i++) { CIRCUIT C = new CIRCUIT(TROOT, "SC FV", 25, Convert.ToInt32(p_scfv.Value), ch_scfv.Checked, 16); }
        }

        private void ParametersChanged(object sender, EventArgs e)
        {

        }
        /////////Descripcions/////////
        private void L_sup_MouseEnter(object sender, EventArgs e)
        {
            Descripcio.Text = "Si la superficie es major a 160m2\nes passa a Electrificació elevada";
        }
        private void L_ilum_MouseEnter(object sender, EventArgs e)
        {
            Descripcio.Text = "Si hi  ha més de 30 punts de llum\nes passa a Electrificació elevada";
        }
        private void L_sec_MouseEnter(object sender, EventArgs e)
        {
            Descripcio.Text = "En cas de que hi hagui previsió de:\n - Secadora\n - Instalació Aire Acondicionat\n" +
                " - Instalació Calefacció Elèctrica\n - Instalació Domòtica\n" +
                " - Punt de Recarga de Vehicle Elèctric\n\nAutomàticament es pasarà a Electrificació Elevada";
        }
        private void L_ench_MouseEnter(object sender, EventArgs e)
        {
            Descripcio.Text = "Si hi ha més de 20 punts de utilització d'endolls\n es passa a Electrificació elevada\n" +
                "\nConta com un punt d'utilització tant si l'endoll es simple, doble o triple\n" +
                "\nCom a mínim per una vivenda s'han de complir les seguents condicions:\n" +
                " - Minim un endoll a cada estància tancada\n" +
                " - Endolls dobles vora de les tomes de Televisió";
        }
        private void L_cocina_MouseEnter(object sender, EventArgs e)
        {
            Descripcio.Text = "Si hi ha més de 6 punts de utilització d'endolls auxiliars en Banys i Cuina\n es passa a Electrificació elevada\n" +
                "\nConta com un punt d'utilització tant si l'endoll es simple, doble o triple\n" +
                " - L'endoll de 25A del Forn o la Cuina no conta com a endoll auxilliar";
        }
        private void RE_GEN_Click(object sender, EventArgs e)
        {

        }
    }
}
