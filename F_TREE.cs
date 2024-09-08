using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoCOM_03
{
    public partial class F_TREE : Form
    {
        public COMPONENTE CSEL;
        public COMPONENTE IG;
        public int MOD = 0;
        public ICollection<CIRCUIT> LIST_1_2_3_4_5_10_11 = new List<CIRCUIT>();//iluminació  endolls generals  cuina i forn  endolls bany i cuina lavadora secadora
        public ICollection<CIRCUIT> LIST_6_7 = new List<CIRCUIT>();//iluminació ext i Endolls exteriors
        public ICollection<CIRCUIT> LIST_8_9_12 = new List<CIRCUIT>();//calefacció i AA
        public ICollection<CIRCUIT> LIST_13 = new List<CIRCUIT>();//PRVE
        public ICollection<CIRCUIT> LIST_14 = new List<CIRCUIT>();//Altres DI
        public ICollection<CIRCUIT> LIST_15 = new List<CIRCUIT>();//subcuadres
        public ICollection<CIRCUIT> LIST_16 = new List<CIRCUIT>();//instalació generadora


      //  List<COMPONENTE> com = new List<COMPONENTE>();
        public F_TREE()
        {
            InitializeComponent();
            Close_Rpan();
        }
        //***********BOTONES**************//
        private void REGEN_Click(object sender, EventArgs e)
        {
            bool fas = treegen(Convert.ToInt32(NDMAX.Value));
            DIMTREE(fas);
            MessageBox.Show("Regenerado Correctamente", "REGENERAR");
        }
        public void button2_Click(object sender, EventArgs e)
        {
            DIMTREE();
            MessageBox.Show("Dimensionado Correctamente", "DIMENSIONAR");
        }
        public double CSS()
        {
            return Convert.ToDouble(CS.Value);
        }

        //***********DIMENSIONAR**************//
        public void DIMTREE(bool fas = false)
        {
            prot(fas);
            I1LOOP(this.IG, false);                         //dimensionamos los conductores, diferenciales(y se preotegen en caso de que seea necesario=
            I2LOOP(this.IG, Convert.ToDouble(CS.Value) / 100 * this.IG.IN());
            this.ACTNODES();
        }
        public void prot(bool fas=false)
        {
            if (fas)
            {
                I1LOOP(this.IG,false);
                var fases = new[] { 'R', 'S', 'T' };
                int i = 0;
                for (int r = 0; r < this.IG.LIST.Count; r++)
                {
                    COMPONENTE CT = this.IG.LIST.ElementAt(r);
                    if (CT.trif == false)
                    {
                        asignar_fase(fases[i], CT);
                        i = i < 2 ? (i + 1) : 0;
                    }
                }
            }
            DIMLOOP(this.IG,Convert.ToDouble(CS.Value)/100);    //dimensionamos los magnetotermicos
            TERMICO IGA = this.IG as TERMICO;
            IGA.SUM_I();                        //dimensionamos el interruptor general
            IGA.DIM(Convert.ToDouble(CS.Value) / 100 * IGA.IN());
            IGA.IMR = Math.Min(IGA.IMR, IGA.IC);
            IGA.IMS = Math.Min(IGA.IMS, IGA.IC);
            IGA.IMT = Math.Min(IGA.IMT, IGA.IC);

        }

        private void DIMLOOP(COMPONENTE CP,double csim)
        {
            for (int r = 0; r < CP.LIST.Count; r++)
            {
                COMPONENTE CT = CP.LIST.ElementAt(r);
                if (CT != null)
                {
                    DIMLOOP(CT,csim);
                }
                TERMICO TTEMP = CT as TERMICO;//dimensionamos los termicos y diferenciales
                DIFF DTEMP = CT as DIFF;
                CT.SUM_I();   //miramos cuales son las intensidades nominales i máximas limitadas de forma ascendiente
                if (DTEMP != null)
                {
                    DTEMP.DIM(Math.Max(DTEMP.IM()*csim,DTEMP.IN()));
                    DTEMP.IMR = Math.Min(DTEMP.IMR, DTEMP.IC);
                    DTEMP.IMS = Math.Min(DTEMP.IMS, DTEMP.IC);
                    DTEMP.IMT = Math.Min(DTEMP.IMT, DTEMP.IC);
                }
                else if (TTEMP != null)
                {
                    TTEMP.DIM(TTEMP.IM());
                }
            }
        }
        private void I1LOOP(COMPONENTE CP,bool IM = true)      //calculamos las intensidades de manera ascendente
        {
            for (int r = 0; r < CP.LIST.Count; r++)
            {
                COMPONENTE CT = CP.LIST.ElementAt(r);
                if (CT.LIST != null)
                {
                    I1LOOP(CP.LIST.ElementAt(r));
                }
                CT.SUM_I(IM);
                if (CT.GetType() == typeof(TERMICO))
                {
                    CT.IMR = Math.Min(CT.IMR, CT.IC);
                    CT.IMS = Math.Min(CT.IMS, CT.IC);
                    CT.IMT = Math.Min(CT.IMT, CT.IC); 
                }
            }
        }
        private void I2LOOP(COMPONENTE CP,Double CDT,bool prot=true)      //calculamos las intensidades de manera descendente i la comparamos
        {
            bool cambio = false;
            
            for (int r = 0; r < CP.LIST.Count; r++)
            {
                COMPONENTE CT = CP.LIST.ElementAt(r);

                if (CT.GetType() == typeof(CONDUCTOR))
                {
                    bool f = true;
                }

                CT.IMR = Math.Min(CT.PADRE.IMR, CT.IMR);
                CT.IMS = Math.Min(CT.PADRE.IMS, CT.IMS);
                CT.IMT = Math.Min(CT.PADRE.IMT, CT.IMT);

                if (CT.GetType() == typeof(TERMICO))
                {
                    CT.IMR = Math.Min(CT.IMR, CT.IC);
                    CT.IMS = Math.Min(CT.IMS, CT.IC);
                    CT.IMT = Math.Min(CT.IMT, CT.IC);
                    MOD = CT.trif ? MOD + 4:MOD + 2;
                }else if (CT.GetType() == typeof(DIFF))
                {
                    if ((CT.IC<CT.IM())&&prot)        //en caso de que sea necesario protegemos el diferencial
                    {
                        TERMICO T1 = new TERMICO(CT.IC, CT.trif);
                        var elementIndex =CT.PADRE.LIST.FindIndex(i => i == CT);
                        CT.PADRE.LIST[elementIndex] = T1;
                        T1.PADRE = CT.PADRE;
                        CT.PADRE = T1;
                        T1.LIST.Add(CT);
                        T1.IR = CT.IR;
                        T1.IS = CT.IS;
                        T1.IT = CT.IT;
                        T1.IMR = CT.IMR = Math.Min(CT.IMR, T1.IC);
                        T1.IMS = CT.IMS = Math.Min(CT.IMS, T1.IC);
                        T1.IMT = CT.IMT = Math.Min(CT.IMT, T1.IC);
                    }
                    MOD = CT.trif ? MOD + 4 : MOD + 2;
                }
                else if(CT.GetType() == typeof(CONDUCTOR))
                {
                    CONDUCTOR LT = CT as CONDUCTOR;
                    LT.DIM(CDT);
                }
                if (CT.LIST != null)
                {
                    I2LOOP(CT,CDT);
                }
            }
        }

        //***********TREE**************/
        public bool treegen(int NDIF = 5)
        {
            MainForm DOCKPARENT = this.Parent.Parent as MainForm;
            F_TREE TROOT = DOCKPARENT.OpenForm<F_TREE>() as F_TREE;
            TROOT.IG = new TERMICO();                //generam larbre
            TROOT.IG.PADRE = null;
            bool t = false;
            t = t | circuitsadd(TROOT, LIST_16, 0);                     //afegim instalació generadora
            t = t | circuitsadd(TROOT, LIST_15, 0);                     //afegim subcuadres
            t = t | circuitsadd(TROOT, LIST_14, 1);                     //afegim circuits amb diferencial unitari
            t = t | circuitsadd(TROOT, LIST_8_9_12, NDIF);          //afegim  circuits de força calefacció i aire AA
            t = t | circuitsadd(TROOT, LIST_1_2_3_4_5_10_11, NDIF); //afegim  circuits d'interior
            t = t | circuitsadd(TROOT, LIST_6_7);             //afegim circuits d'exterior
            t = t | circuitsadd(TROOT, LIST_13, 1);                     //afegim PRVE
            return t;
        }
        private void asignar_fase(char f, COMPONENTE CTEMP)
        {
            for (int r = 0; r < CTEMP.LIST.Count; r++)
            {
                COMPONENTE CT = CTEMP.LIST.ElementAt(r);
                CT.F = f;
                if (CT.LIST != null)
                {
                    asignar_fase(f, CT);
                }
            }
        }
        private bool circuitsadd(F_TREE TROOT, ICollection<CIRCUIT> LIST, int DMAX = 5) 
        {
            bool trifasico = false;
            if (LIST != null & LIST.Count != 0)
            {
                LIST.OrderBy(CIRCUIT => CIRCUIT.trif);          //miramos si hay algún circuito trifasico
                                                                //   trifasico = LIST.ElementAt(LIST.Count - 1).trif ? true : false;
                                                                //   trifasico = LIST.ElementAt(LIST.Count - 1).trif ? true : false;
                trifasico = LIST.ElementAt(0).trif ? true : false;
                LIST.OrderByDescending(CIRCUIT => CIRCUIT.P);

                if (LIST != null & LIST.Count != 0)
                {
                    if (DMAX != 0)
                    {
                        int NDIF = LIST.Count / DMAX;          //MIRAM QUANTS DE DIFERENCIALS FAN FALTA
                        if ((LIST.Count % DMAX) != 0 /*|| LIST.Count == 1*/)
                        {
                            NDIF = NDIF + 1;
                        }

                        ICollection<DIFF> tempdiff = new List<DIFF>();
                        for (int i = 0; i < NDIF; i++)     //CREEM ELS DIFERENCIALS
                        {
                            DIFF D1 = new DIFF(25, true);
                            D1.PADRE = TROOT.IG;
                            TROOT.IG.LIST.Add(D1);
                            tempdiff.Add(D1);
                        }
                        int IDDIFF = 0;
                        for (int i = 0; i < LIST.Count; i++)        //mirar que tempdiff puede ser igual a 0
                        {
                            LIST.ElementAt(i).PADRE.PADRE.PADRE = tempdiff.ElementAt(IDDIFF);
                            tempdiff.ElementAt(IDDIFF).LIST.Add(LIST.ElementAt(i).PADRE.PADRE);
                            IDDIFF++;
                            if (IDDIFF >= (NDIF))
                            {
                                IDDIFF = 0;
                            }
                        }
                    }
                    else   // Afegim subcuadres i circuits sense diferencial
                    {
                        for (int i = 0; i < LIST.Count; i++)
                        {
                            LIST.ElementAt(i).PADRE.PADRE.PADRE = TROOT.IG;
                            TROOT.IG.LIST.Add(LIST.ElementAt(i).PADRE.PADRE);
                        }
                    }
                }
            }
            return trifasico;
        }
        public void ACTINFO(COMPONENTE C0, TreeView A0)
        {
            TreeNode N0 = new TreeNode(getname(C0));
            A0.Nodes.Clear();
            A0.Nodes.Add(N0);
            A0.Nodes[0].Tag = C0;
            C0.NODO = A0.Nodes[0];
            ADDLIST(C0, N0);
            void ADDLIST(COMPONENTE CP, TreeNode NP)             //bucle per anar creant els fills
            {
                for (int r = 0; r < CP.LIST.Count; r++)
                {
                    COMPONENTE CH = CP.LIST.ElementAt(r);
                    TreeNode NH = new TreeNode(getname(CH));
                    NP.Nodes.Add(NH);
                    NP.Nodes[r].Tag = CH;// r per 0
                    CH.NODO = NH;
                    if (CH.LIST != null)
                    {
                        ADDLIST(CH, NH);
                    }
                }
            }
            A0.ExpandAll();
        }
        public void ACTNODES()
        {
            ACTINFO(this.IG, this.arbol1);
        }
        private string getname(COMPONENTE temp)
        {
            string str = "componente";
            TERMICO TTEMP = temp as TERMICO;
            DIFF DTEMP = temp as DIFF;
            CIRCUIT CTEMP = temp as CIRCUIT;
            CONDUCTOR LTEMP = temp as CONDUCTOR;
            if (TTEMP != null)
            {
                str = "P.I.A." + TTEMP.IC + "A ";
                str = TTEMP.trif ? str + "4P" : str + "2P";
            }
            else if (DTEMP != null)
            {
                str = "DIFF." + DTEMP.IC + "A ";
                str = DTEMP.trif ? str + "4P" : str + "2P";
            }
            else if (CTEMP != null)
            {
                str = CTEMP.NAME;
            }
            else if (LTEMP != null)
            {
                str = LTEMP.Aisl == 1 ? "RZ1-K" : "H07Z1-K";
                str = LTEMP.trif ? str + " 5x" : str + " 3x";
                str = str + LTEMP.S + " mm2 " + LTEMP.L.ToString() + " ml.";
            }
            return str;
        }
        //********Node Selection**************//
        private void NODE_SELECT(object sender, TreeNodeMouseClickEventArgs e)
        {
            P_EC.Show();
         
            CSEL = e.Node.Tag as COMPONENTE;
            CIRCUIT CTEMP = CSEL as CIRCUIT;
            TERMICO TTEMP = CSEL as TERMICO;
            DIFF DTEMP = CSEL as DIFF;
            CONDUCTOR LTEMP = CSEL as CONDUCTOR;
            if (CTEMP != null)
            {
                E_CIR.Show();
                E_TERM.Hide();
                E_DIFF.Hide();
                E_COND.Hide();
                EC_1.Checked = CTEMP.trif;
                EC_2.Value = CTEMP.P;
                EC_3.Text = CTEMP.NAME;
                EC_4.SelectedIndex = CTEMP.tipo - 1;
            }
            else if (TTEMP != null)
            {
                E_TERM.Show();
                E_DIFF.Hide();
                E_CIR.Hide();
                E_COND.Hide();
                ET_1.Checked = TTEMP.trif;
                ET_2.Text = TTEMP.IC.ToString();
                ET_3.Text = TTEMP.ICC.ToString();
                ET_4.Text = TTEMP.CURVA.ToString();
            }
            else if (DTEMP != null)
            {
                E_DIFF.Show();
                E_TERM.Hide();
                E_CIR.Hide();
                E_COND.Hide();
                ED_1.Checked = DTEMP.trif;
                ED_2.Text = DTEMP.IC.ToString();
                ED_3.Text = DTEMP.IDD.ToString();
                ED_4.Text = DTEMP.TIPO.ToString();
            }
            else if (LTEMP != null)
            {
                E_COND.Show();
                E_TERM.Hide();
                E_DIFF.Hide();
                E_CIR.Hide();
                EL_1.Checked = LTEMP.trif;
                EL_2.Text = LTEMP.S.ToString();
                EL_3.Value = Convert.ToDecimal(LTEMP.L);
                EL_4.SelectedIndex = LTEMP.Aisl;
            }
        }
        private void DEL_COMP_Click(object sender, EventArgs e)
        {
            if (CSEL.LIST != null)
            {
                for (int i = 0; i < (CSEL.LIST.Count); i++)
                {
                    CSEL.LIST.ElementAt(i).PADRE = CSEL.PADRE;
                    CSEL.PADRE.LIST.Add(CSEL.LIST.ElementAt(i));
                    
               //     CSEL.PADRE.NODO.Nodes.Add(CSEL.NODO.Nodes[i]);

                }
             //   CSEL.NODO.Remove();
            }
            CSEL.PADRE.LIST.Remove(CSEL);
            CIRCUIT CTEMP = CSEL as CIRCUIT;
            if (CTEMP != null)
            {
                LIST_1_2_3_4_5_10_11.Remove(CTEMP);
                LIST_6_7.Remove(CTEMP);
                LIST_8_9_12.Remove(CTEMP);
                LIST_13.Remove(CTEMP);
                LIST_14.Remove(CTEMP);
                LIST_15.Remove(CTEMP);
                LIST_16.Remove(CTEMP);
            }
       //     this.arbol1.Nodes.Remove(CSEL.NODO);
            this.ACTNODES();
        }
        private void ACT_COMP_Click(object sender, EventArgs e)
        {
            if (E_TERM.Visible)
            {
                TERMICO TTEMP = CSEL as TERMICO;
                TTEMP.trif = ET_1.Checked;
                TTEMP.IC = Convert.ToInt32(ET_2.Text.ToString());
                TTEMP.ICC = Convert.ToDouble(ET_3.Text.ToString());
                TTEMP.CURVA = ET_4.Text.ToString();

            }
            else if (E_DIFF.Visible)
            {

                DIFF DTEMP = CSEL as DIFF;
                DTEMP.trif = ED_1.Checked;
                DTEMP.IC = Convert.ToInt32(ED_2.Text.ToString());
                DTEMP.IDD = Convert.ToInt32(ED_2.Text.ToString());
                DTEMP.TIPO = ED_4.Text.ToString();
            }
            else if (E_CIR.Visible)
            {
                CIRCUIT CTEMP = CSEL as CIRCUIT;
                CTEMP.trif = EC_1.Checked;
                CTEMP.P = Convert.ToInt32(EC_2.Value);
                CTEMP.NAME = EC_3.Text.ToString();
                CTEMP.tipo = Convert.ToInt32(EC_4.SelectedIndex) + 1;
            }
            else if (E_COND.Visible)
            {
                CONDUCTOR LTEMP = CSEL as CONDUCTOR;
                LTEMP.trif = EL_1.Checked;
                LTEMP.S = Convert.ToDouble(EL_2.SelectedIndex);
                LTEMP.L = Convert.ToInt32(EL_3.Value);
                LTEMP.Aisl = EL_4.SelectedIndex;
            }
            CSEL.NODO.Text = getname(CSEL);
        }
        //********Right sidebar**************
        private void Close_Rpan()
        {
            P_EC.Hide();
        }
        private void Open_Rpan(Panel PP)
        {
            if (PP.Visible)
            {
                PP.Hide();
            }
            else
            {
                Close_Rpan();
                PP.Show();
            }
        }
        private void B_EC_Click(object sender, EventArgs e)
        {
            if (CSEL != null)
            {
                Open_Rpan(P_EC);
            }
        }
        private void PLOT_BT_Click(object sender, EventArgs e)
        {
            MOD = 0;
            I1LOOP(this.IG);
            I2LOOP(this.IG, Convert.ToDouble(CS.Value) / 100);

            MainForm DOCKPARENT = this.Parent.Parent as MainForm;
            F_PLOT TP = DOCKPARENT.OpenForm<F_PLOT>() as F_PLOT;
            TP.ROOT = this.IG;
            TP.MOD = this.MOD;
            TP.NMOD();
        }

        private void PARAM_BT_Click(object sender, EventArgs e)
        {
            MainForm MF = this.Parent.Parent as MainForm;
            MF.OpenForm<F_PARAM>();
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }
        //*********DragDrop********//
        private void arbol1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Move the dragged node when the left mouse button is used.  
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }

            // Copy the dragged node when the right mouse button is used.  
            else if (e.Button == MouseButtons.Right)
            {
                DoDragDrop(e.Item, DragDropEffects.Copy);
            }
        }

        private void arbol1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void arbol1_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the mouse position.  
            Point targetPoint = arbol1.PointToClient(new Point(e.X, e.Y));

            // Select the node at the mouse position.  
            arbol1.SelectedNode = arbol1.GetNodeAt(targetPoint);
        }

        private void arbol1_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.  
            Point targetPoint = arbol1.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.  
            TreeNode targetNode = arbol1.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.  
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // Confirm that the node at the drop location is not   
            // the dragged node or a descendant of the dragged node.  
            if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode))
            {
                COMPONENTE CD = draggedNode.Tag as COMPONENTE;
                COMPONENTE CT = targetNode.Tag as COMPONENTE;
                // If it is a move operation, remove the node from its current   
                // location and add it to the node at the drop location.  
                if (e.Effect == DragDropEffects.Move)
                {
                    CD.PADRE.LIST.Remove(CD);
                    CD.PADRE = CT;
                    CT.LIST.Add(CD);
                    draggedNode.Remove();
                    targetNode.Nodes.Add(draggedNode);
                }

                // If it is a copy operation, clone the dragged node   
                // and add it to the node at the drop location.  
                else if (e.Effect == DragDropEffects.Copy)
                {
                    TreeNode ndc= (TreeNode)draggedNode.Clone();      
                    ndc.Tag = CD.DeepClone();
                    CD.NODO = ndc;
                    CD.PADRE = CT;
                    CT.LIST.Add(CD);
                    targetNode.Nodes.Add(ndc);
                    ACTNODES(); ;
                }
                // Expand the node at the location   
                // to show the dropped node.  
                targetNode.Expand();
            }
        }

        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            // Check the parent node of the second node.  
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;

            // If the parent node is not null or equal to the first node,   
            // call the ContainsNode method recursively using the parent of   
            // the second node.  
            return ContainsNode(node1, node2.Parent);
        }
    }
}
