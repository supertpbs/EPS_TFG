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
    public partial class F_PLOT : Form
    {
        public COMPONENTE ROOT;
        public double MOD;
        public F_PLOT()
        {
            InitializeComponent();

        }
        private void F_PLOT_Load(object sender, EventArgs e)
        {
            NMOD();
            if (ATYPE.Text == "")
            {
                ATYPE.SelectedIndex = 1;
            }
            if(AMOD.Text == "")
            {
                AMOD.SelectedIndex = 4;
            }


        }
        private void B_PLOT_Click(object sender, EventArgs e)
        {
            MainForm DOCKPARENT = this.Parent.Parent as MainForm;
            F_TREE TROOT = DOCKPARENT.FACTIVE<F_TREE>() as F_TREE;
            ROOT = TROOT.IG;
            Draw D1 = new Draw();
            DOCKPARENT.WindowState = FormWindowState.Minimized;

            bool superf = false;
            string MMM = ATYPE.Text + "\n";
            int M = Convert.ToInt32(AMOD.Text.ToString());
            double res = Convert.ToDouble(N_RES.Value);

            double F = Math.Ceiling(MOD * (1 + res/100)/M);
                
            MMM = MMM + F.ToString() + " Files " + M.ToString() + " Mòd. DIN \n"+MOD.ToString()+ " Mòd. DIN Aparamenta\n" + Convert.ToString(Math.Round(MOD*res/100))+ " Mòd. DIN Reserva\n" + Convert.ToString(Math.Round(MOD*(1+res/100)))+ " Mòd. DIN Totals";
            string str2 = c_cont.Text;
            string str3 = c_prot.Text;

            D1.DrawTREE(ROOT,1,TROOT.CSS(),MMM,str2,str3);

        }
        private void TREE_BT_Click(object sender, EventArgs e)
        {
            MainForm MF = this.Parent.Parent as MainForm;
            MF.OpenForm<F_TREE>();
        }

        private void N_RES_ValueChanged(object sender, EventArgs e)
        {
            NMOD();
        }
        public void NMOD()
        {
            M_AP.Text = MOD.ToString()+" DIN";
            double r= Math.Round(MOD*Convert.ToDouble(N_RES.Value)/100,1);
            M_RES.Text =  r.ToString()+ " DIN";
            r = Convert.ToDouble(MOD) + r;
            M_TOT.Text=r.ToString()+" DIN";
        }

        private void B_Esq_Click(object sender, EventArgs e)
        {

        }

        private void c_cont_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
