using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace ProyectoCOM_03
{
    public static class C
    {
        public const Double V = 230;
        public const Double cond = 0.0178;         //Parametros para cableado de cobre
        public const Double alpha = 0.00392;
        public const Double L = 18;

        public static readonly Double[] Secc = 
                { 1.5, 2.5,  4,  6, 10, 16, 25,  35,  50,  70,  95, 129, 159, 185, 240, 300 };
        public static readonly Double[,,] IMM = { {
                { 14.5, 20, 26, 34, 46, 63, 82, 101, 122, 155, 187, 216, 247, 281,   0,   0,   0 },//H07Z1-K 3x
                { 14.5, 20, 26, 34, 46, 63, 82, 101, 122, 155, 187, 216, 247, 281,   0,   0,   0 } },//H07Z1-K 5x1x
              { { 24  , 32, 42, 53, 70, 91, 96, 117, 138, 170, 202, 230, 260, 291, 336, 380,   0 },//RZ1-K 3x
                { 21  , 27, 35, 44, 58, 75, 96, 117, 138, 170, 202, 230, 260, 291, 336, 380, 446 } } };//RZ1-K 5x1x


       
    }
    public class COMPONENTE
    {
        public COMPONENTE PADRE;
        public List<COMPONENTE> LIST = new List<COMPONENTE>();
        public TreeNode NODO;

        public double IC = 0;
        public bool trif = false;

        public char F = 'R';        // "R" "S" "T" // "U"
        public Double IR = 0;
        public Double IS = 0;
        public Double IT = 0;

        public Double IMR = 0;
        public Double IMS = 0;
        public Double IMT = 0;


        public COMPONENTE GETLIST(int i)
        {
            return this.LIST.ElementAt(i);
        }
        public COMPONENTE DeepClone()
        {
            COMPONENTE C1 = this.Clone();
            for(int i=0; i< C1.LIST.Count; i++)
            {
                COMPONENTE P2 = this.LIST.ElementAt(i);
                COMPONENTE C2;
                if (this.LIST.ElementAt(i).LIST != null)
                {
                    C2 = DeepClone();
                }
                else
                {
                    C2 = P2.Clone();
                }
                C1.LIST.Add(C2);
                C2.PADRE = C1;
            }

            return C1;
        }
        public COMPONENTE Clone()
        {
            TERMICO T1 = this as TERMICO;
            DIFF D1 = this as DIFF;
            CONDUCTOR L1 = this as CONDUCTOR;
            CIRCUIT C1 = this as CIRCUIT;
            if (T1 != null)
            {
                return T1.Clone();
            }else if(D1 != null)
            {
                return D1.Clone();
            }else if(L1 != null)
            {
                return L1.Clone();
            }
            else if(C1 != null)
            {
                return C1.Clone();
            } else {
                return null;
            }

        }
        public void SUM_I(bool iNOM=true)
        {
            if (this.GetType() == typeof(CIRCUIT))
            {
                CIRCUIT CTEMP = this as CIRCUIT;
                Double IM = double.PositiveInfinity;
                if (iNOM)
                {
                    IM = CTEMP.IM();
                }

                switch (F)
                {
                    case 'U':
                        IR = IS = IT = CTEMP.P / (C.V * 3);
                        IMR = IMS = IMT = IM;
                        break;
                    case 'T':
                        IT = CTEMP.P / C.V;
                        IMT = IM;
                        IMR = IMS = IR = IS = 0;
                        break;
                    case 'S':
                        IS = CTEMP.P / C.V;
                        IMS = IM;
                        IMR = IMT = IR = IT = 0;
                        break;
                    default:
                        IR = CTEMP.P / C.V;
                        IMR = IM;
                        IMS = IMT = IS = IT = 0;
                        break;
                }
            }
            else
            {
                IR = IS = IT = IMR = IMS = IMT = 0;
                for (int i = 0; i < LIST.Count; i++)
                {           //SUMEM TOTES LES INTENSITATS DELS FILLS
                    IR += LIST.ElementAt(i).IR;
                    IS += LIST.ElementAt(i).IS;
                    IT += LIST.ElementAt(i).IT;
                    IMR += LIST.ElementAt(i).IMR;
                    IMS += LIST.ElementAt(i).IMS;
                    IMT += LIST.ElementAt(i).IMT;
                }
                if (((IMR == 0) && (IMS == 0)) || ((IMT == 0) && ((IMR != 0) ^ (IMS != 0))))
                {
                    trif = false;
                    F = 'R';
                    F = IMS != 0 ? 'S' : F;
                    F = IMT != 0 ? 'T' : F;
                }
                else
                {
                    trif = true;
                    F = 'U';
                }
            }
        }
        public double IM()
        {
            return Math.Max(IMR, Math.Max(IMS, IMT));
        }
        public double IN()
        {
            return Math.Max(IR, Math.Max(IS, IT));
        }

        public double IM_M()
        {
            double[] args = new double[] { IMR, IMS, IMT };
            return args.Where(c => c != 0).Average();
        }
        public double IN_M()
        {
            double[] args = new double[] {IR,IS,IT};
            return args.Where(c => c != 0).Average();
        }
    }

    public class TERMICO : COMPONENTE
    {
        public string CURVA/* { get; set; }*/="C";
        public double ICC /*{ get; set; }*/ = 0;
        public TERMICO(double ic = 10, bool tri = false, double icc = 4.5,string C= "C")
        {
            DIM(ic);
            ICC = icc;
            this.trif = tri;
            CURVA = C;
        }
        public TERMICO Clone()
        {
            TERMICO C1 = new TERMICO(IC, trif, ICC, CURVA);
            C1.IR = IR;
            C1.IS = IS;
            C1.IT = IT;
            C1.IMR = IMR;
            C1.IMS = IMS;
            C1.IMT = IMT;
            C1.F = F;
            return C1;
        }

        public void DIM(double IN = 0)
        {
            IN = (IN == 0) ? Math.Max(IR, Math.Max(IS, IT)) : IN;
            int[] IList = {10, 16, 25, 32, 40, 50, 63, 80, 100, 125, 160, 180, 200 };
            int k = 0;
            for (; k < IList.Length && IN > IC; k++)
            {
                IC = IList[k];
            }

           /* IMR = Math.Min(IMR, IC);
            IMS = Math.Min(IMS, IC);
            IMT = Math.Min(IMT, IC);*/
        }
    }
    public class DIFF : COMPONENTE
    {
        public int IDD { get; set; } = 30;
        public string TIPO = "AC";
        public DIFF(double ic = 40, bool tri = false, int idd = 30,string TI="AC")
        {
            DIM(ic);
            IDD = idd;
            trif = tri;
            TIPO = TI;
        }
        public DIFF Clone()
        {
            DIFF C1 = new DIFF(IC, trif, IDD, TIPO);
            C1.IR = IR;
            C1.IS = IS;
            C1.IT = IT;
            C1.IMR = IMR;
            C1.IMS = IMS;
            C1.IMT = IMT;
            C1.F = F;
            return C1;
        }
        public void DIM(double IN = 0, double cds=1)
        {
            IN = IN == 0 ? Math.Max(IR, Math.Max(IS, IT)) : IN;
            IN = IN * cds;
            int[] IList = { 25, 40, 63, 80, 100, 125, 160, 180, 200 };
            int k = 0;
            for (; k < IList.Length && IN > IC; k++)
            {
                IC = IList[k];
            }

        }
    }
    public class CIRCUIT : COMPONENTE
    {
        public string NAME { get; set; }
        public int P { get; set; }                       //POTENCIA INSTALADA -> SUMA POTENCIAS CIRCUITOS
        public int tipo { get; set; } 
        //  llegenda
        //  1	iluminació
        //  2	Endolls Generals
        //  3	Cuina i Forn
        //  4	Lavador, Lavavaix
        //  5	Bany
        //  6	Ilum. Ext
        //  7	Endolls Ext
        //  8	Calef
        //  9	AA
        // 10	Secadora
        // 11	domotica
        // 12	Altres circuits de Força
        // 13	PRVE
        // 14	Altres circuits amb diferencial independent
        // 15   Subcuadre
        // 16   Instalació generadora

        public CIRCUIT(string NAM = "RESERVA", int PP = 0, bool Tri = false, int tip = 12)
        {
            NAME = NAM;
            P = PP;
            tipo = tip;
            trif = Tri;
            DIM();
            F = trif ?'U' : F;
        }
        public CIRCUIT Clone()
        {
            CIRCUIT C1 = new CIRCUIT(NAME,P,trif,tipo);
            C1.IR = IR;
            C1.IS = IS;
            C1.IT = IT;
            C1.IMR = IMR;
            C1.IMS = IMS;
            C1.IMT = IMT;
            C1.F = F;
            return C1;
        }
        public void DIM()
        {
            if (trif)
            {
                F = 'U';
            }
            switch (F)
            {
                case 'U':
                    IR = IS = IT = P / (C.V * 3);
                    IMR = IMS = IMT = IM();
                    break;
                case 'S':
                    IS = P / (C.V);
                    IMS = IM();
                    IR = IT = IMR = IMT = 0;
                    break;
                case 'T':
                    IT = P / (C.V);
                    IMT = IM();
                    IR = IS = IMR = IMT = 0;
                    break;
                default:
                    IR = P / (C.V);
                    IMR = IM();
                    IS = IT = IMS = IMT = 0;
                    F = 'R';
                    break;
            }
        }
        public CIRCUIT(F_TREE TROOT, string NAM = "RESERVA", int II = 10, int PP = -1, bool Tri = false, int tip = 12, double L =C.L)
        {
            NAME = NAM;
            tipo = tip;
            trif = Tri;
            if (Tri)
            {
                P = P < 0 ? Convert.ToInt32(II * C.V * 0.8 * 3) : PP;
                IMR = IMS = IMT = II;
            }
            else
            {
                P = P < 0 ? Convert.ToInt32(II * C.V * 0.8) : PP;
                IMR = II;
            }
            switch (tip)
            {
                case 1://iluminació
                    TROOT.LIST_1_2_3_4_5_10_11.Add(this);
                    IMR = 10;
                    break;
                case 2://endolls generals
                    TROOT.LIST_1_2_3_4_5_10_11.Add(this);
                    IMR = 16;
                    break;
                case 3://cuina i forn
                    TROOT.LIST_1_2_3_4_5_10_11.Add(this);
                    IMR = 25;
                    break;
                case 4://lavadora
                    TROOT.LIST_1_2_3_4_5_10_11.Add(this);
                    IMR = 16;
                    break;
                case 5://endolls bany i cuina
                    TROOT.LIST_1_2_3_4_5_10_11.Add(this);
                    IMR = 16;
                    break;
                case 6: //ilum ext
                    TROOT.LIST_6_7.Add(this);
                    IMR = 10;
                    break;
                case 7:  //endolls ext
                    TROOT.LIST_6_7.Add(this);
                    IMR = 16;
                    break;
                case 8://calefacció
                    TROOT.LIST_8_9_12.Add(this);
                    IMR = 10;
                    break;
                case 9://AA
                    TROOT.LIST_8_9_12.Add(this);
                    IMR = 10;
                    break;
                case 10://secadora
                    TROOT.LIST_1_2_3_4_5_10_11.Add(this);
                    IMR = 16;
                    break;
                case 11://domotica
                    TROOT.LIST_1_2_3_4_5_10_11.Add(this);
                    IMR = 10;
                    break;
                case 12://altres
                    TROOT.LIST_8_9_12.Add(this);
                    IMR = 10;
                    break;
                case 13://PRVE
                    TROOT.LIST_13.Add(this);
                    IMR = 10;
                    break;
                case 14://Altres DI
                    TROOT.LIST_14.Add(this);
                    IMR = 10;
                    break;
                case 15://subcuadres
                    TROOT.LIST_15.Add(this);
                    IMR = 10;
                    break;
                case 16://FV
                    TROOT.LIST_16.Add(this);
                    IMR = 10;
                    break;
                default://altres
                    TROOT.LIST_8_9_12.Add(this);
                    IMR = 10;
                    break;
            }

            CONDUCTOR c1 = new CONDUCTOR(L, 0, false, Tri);
            PADRE = c1;
            c1.LIST.Add(this);
            TERMICO T1 = new TERMICO(II, Tri);
            c1.PADRE = T1;
            T1.LIST.Add(c1);
            DIM();
        }
    }
    public class CONDUCTOR : COMPONENTE
    {
        public Double L { get; set; } = 0;             //Longitud 
        public Double S = 1.5;                      //Sección
        public Double cdtv=-2;
        public int Aisl= 0; /* 
        0 -> H07Z1-K    (PVC)
        1 -> RZ1-K      (XLPE)*/
       // public Double[] Secc;

        public CONDUCTOR(Double Lon = 15, double Sec = 1.5,bool aislamiento = false,bool TRI=false)
        {
            L = Lon;
            Aisl = aislamiento ? 1 : 0;
            S = Sec;
            this.trif = TRI;
           // IMM = T1.Imax;
            //Secc = T1.Secc;

        }
        public CONDUCTOR Clone()
        {
            CONDUCTOR C1 = new CONDUCTOR(L, S, true, trif);
            C1.Aisl= Aisl;
            C1.IR = IR;
            C1.IS = IS;
            C1.IT = IT;
            C1.IMR = IMR;
            C1.IMS = IMS;
            C1.IMT = IMT;
            C1.F = F;
            return C1;
        }
        public void DIM(double cdtpmax=0.02)
        {
            int T_MAX = Aisl == 0 ? 70:90;
            int T_AMB = 20;     // agafar de taula enterrat etc.

            double y;
            int i = 0;
            int tri = this.trif ? 1 : 0;
            S = 1.5;
            double IN = (Math.Max(IR, Math.Max(IS, IT)));
            if (Aisl == 0 && (tri == 0 && IN > 160 || tri == 1 && IN > 202))
            {//en caso de que sea un cable de 750V miramos si puede llevar esa intensidad, ni no se pasa a manguera
                Aisl = 1;
            }
            int r = C.IMM.GetLength(2);
            double ima = C.IMM[Aisl, tri, i];
            while ((Math.Max(this.IM(),this.IN()) > C.IMM[Aisl,tri,i]) && i<C.IMM.GetLength(2))      //SECCIÓN MINIMA POR INTENSIDAD
            {
                i++;
                S = C.Secc[i];
            }
            IC = C.IMM[Aisl,tri,i];
            //S=1.5;
            y = 1 / (C.cond * (1 + C.alpha * (T_MAX - T_AMB) * (IN / IC) * (IN / IC)));
            cdtv = tri == 1 ? Math.Sqrt(3) * L * IN / (S * y) : 2*L*IN/(S * y);
            double cdtp = tri == 0 ? cdtv / 230 : cdtv / 400;
            while (cdtp >= cdtpmax && cdtpmax != 0 && i<C.Secc.Length)      //SECCION MINIMA POR CDT
            {
                y = 1/(C.cond*(1+C.alpha*(T_MAX-T_AMB)*(IN/IC)*(IN/IC)));
                cdtv = tri == 1 ? Math.Sqrt(3) * L * IN / (S * y) : 2 * L * IN / (C.Secc[i] * y);
                cdtp = tri == 0 ? cdtv / C.V : cdtv/400 ;
                i++;
            }
            if (i < C.Secc.Length)
            {
                S = C.Secc[i];
            }
            else
            {
                S = C.Secc[C.Secc.Length-1];
            }
        }
    }
}
