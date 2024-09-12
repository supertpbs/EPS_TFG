using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;//per donar missatjes a l'usuari
using System;

namespace ProyectoCOM_03
{

    public class interfaz
    {
        [CommandMethod("QUADRE")]
        public void InterfazGrafica()
        {
            MainForm inter01 = new MainForm();
            inter01.Show();
        }


    }
    public class Draw
    {
        public static Point3d oorigin = new Point3d(0, 0, 0);
        public static Vector3d normal = new Vector3d(0, 0, 1);
        int cnum = 1;



        //******MetodosGUINOaccesibles******//
        public void DrawTREE(COMPONENTE ROOT, double zoom = 1,double cs=0.3, string mod ="Armari d'Empotrar",string cont = "a Comptador",string prot = "NO")
        {
            Draw d1 = new Draw();
            Point3d S0 = d1.Cargarpunto("Seleccóna el punt d'insercció");
            Point3d S1 = new Point3d(S0.X - 40, S0.Y, 0);
            d1.DrawText(new Point3d(S0.X - 25, S0.Y + 2.5, 0), "a "+cont,2);
            d1.DrawLine(S0, S1);

            Point3d S2 = new Point3d(S0.X - 1, S0.Y+1, 0);
            
            Point3d S3 = new Point3d(S0.X - 1, S0.Y-1, 0);
            d1.DrawLine(S0, S2);
            d1.DrawLine(S0, S3);

            Point3d S4 = new Point3d(S1.X, S1.Y - 5, 0);
            d1.DrawLine(S1, S4);
            d1.DIB(S4, ROOT);    //CG


            if (prot != "NO")
            {
                Point3d Pt1 = new Point3d(S4.X, S4.Y, 0);
                Point3d Pt2 = new Point3d(Pt1.X-20, Pt1.Y, 0);
                d1.DrawLine(Pt1,Pt2);
                Point3d Pt3 = new Point3d(Pt2.X, Pt2.Y-5, 0);
                d1.DrawLine(Pt2, Pt3);

                Point3d PtL = new Point3d(Pt3.X-2, Pt3.Y, 0);        
                Point3d PtB = new Point3d(Pt3.X, Pt3.Y - 10, 0);

                d1.DrawRect(PtL, 10, 4);
                Point3d PtR = new Point3d(Pt2.X + 2, Pt3.Y, 0);
                d1.DrawLine(PtL,PtB);
                d1.DrawLine(PtR, PtB);
                Point3d Ptb2 = new Point3d(PtB.X,PtB.Y-4,0);
                Point3d PttR = new Point3d(Ptb2.X + 2,Ptb2.Y, 0);
                Point3d PttL = new Point3d(Ptb2.X - 2, Ptb2.Y, 0);
                d1.DrawLine(Ptb2, PtB);

                d1.DrawLine(PttL, PttR);
                Point3d PttR2 = new Point3d(PttR.X - 0.75, PttR.Y-1.5, 0);
                Point3d PttL2 = new Point3d(PttL.X + 0.75, PttL.Y-1.5, 0);
                d1.DrawLine(PttL2, PttR2);
                Point3d PttR3 = new Point3d(PttR2.X - 0.75, PttR2.Y-1.5, 0);
                Point3d PttL3 = new Point3d(PttL2.X + 0.75, PttL2.Y-1.5, 0);
                d1.DrawLine(PttL3, PttR3);

                PtR = new Point3d(PtR.X+2,PtR.Y-1, 0);
                d1.DrawText(PtR,"Prot.\nSobret.\n"+prot,1.6);
            }
            //Protector sobretensions
            COMPONENTE CTEMP = ROOT;

            //
            string CUADRO = "Quadre Elèctric";
            d1.DrawText(new Point3d(S4.X-30,S4.Y+25,0),"QUADRE ELÈCTRIC",6,0,0,false,300);
            string PPM = "Pot. Màxima = " + Convert.ToString(Math.Round(ROOT.trif ? ROOT.IC * C.V * 3 : ROOT.IC * C.V, 2)) + " W\n";
            double PN = Math.Round((ROOT.IR + ROOT.IS + ROOT.IT) * C.V);
            PPM = PPM + "Pot. Nominal = " + PN.ToString()+" * "+Convert.ToString(Math.Round(cs/100,2))+" = " +Convert.ToString(Math.Round(cs*PN/100,0))  + " W\n";
            if (ROOT.trif)
            {
                double Imin = Math.Min(ROOT.IR, Math.Min(ROOT.IS, ROOT.IT));
                double Imax = ROOT.IN();
                PPM = PPM + "Desfase ";
                if (Imin == ROOT.IR)
                {
                    PPM = PPM + "R-";  
                }
                else if (Imin == ROOT.IS)
                {
                    PPM = PPM + "S-";
                }
                else if (Imin == ROOT.IT)
                {
                    PPM = PPM + "T-";
                }

                if (Imax == ROOT.IR)
                {
                    PPM = PPM + "R ";
                }
                else if (Imax == ROOT.IS)
                {
                    PPM = PPM + "S ";
                }
                else if (Imax == ROOT.IT)
                {
                    PPM = PPM + "T ";
                }
                PPM = PPM + "= " + Convert.ToString(Math.Round(cs/100*(Imax - Imin) * C.V, 2)) + "W ("+Math.Round(Imin,2)+" A-"+Math.Round(Imax,2)+" A)";
            }
            d1.DrawText(new Point3d(S0.X - 20, S0.Y - 6, 0), PPM, 1.6, 0, 0, false, 60);
            d1.DrawText(new Point3d(S0.X +25, S0.Y - 4, 0), mod, 1.6, 0, 0, false, 60);
            (int depth, int width) = RECORRER(CTEMP);
            
            Point3d PTEMP = new Point3d(S4.X-20*width/2,S4.Y,0);

            Point3d DF = d1.DRAWLOOP(PTEMP, CTEMP,depth);
            double LL = DF.X-PTEMP.X+30+25;
            d1.DrawRect(new Point3d(S0.X - 70, S0.Y + 10, 0), depth*20+30,Math.Max(LL,140));
        }
        private (int, int) RECORRER(COMPONENTE CTEMP2, (int, int) size = default, int depth = 0)
        {
            depth = depth + 1;
            if (CTEMP2.LIST.Count != null)
            {
                for (int r = 0; r < CTEMP2.LIST.Count; r++)
                {
                    if (CTEMP2.LIST != null && CTEMP2.GetType() != typeof(CIRCUIT))
                    {
                        size = RECORRER(CTEMP2.GETLIST(r), size, depth);
                    }

                }
            }
            else
            {
                size.Item2 += 1;
            }
            return (Math.Max(size.Item1, depth), size.Item2);   
        }


        Point3d DRAWLOOP(Point3d pte, COMPONENTE CT, int df=0, int di=0)             //bucle per anar DIBUIXANT
        {
            di=di+1;
            pte = new Point3d(pte.X, pte.Y - 10, 0);
            Point3d pt2 = new Point3d(pte.X, pte.Y - 5, 0);
            if (CT.GetType()!=typeof(CIRCUIT))
            {
                DrawLine(pte, pt2);
                pte = pt2;
            }

            for (int r = 0; r < CT.LIST.Count; r++)
            {
                pt2 = new Point3d(pte.X, pte.Y - 5, 0);
                DrawLine(pte, pt2);
                COMPONENTE CI = CT.GETLIST(r);
                Point3d ptee = this.DIB(pt2, CI,df-di); //dibujmos componentes hijos
                if (CT.LIST != null&&CT.GetType()!=typeof(CIRCUIT)) //en caso de que este tenga sus propios hijos los dibujamos
                {
                    Point3d pt3 = new Point3d(DRAWLOOP(ptee, CI,df,di).X, pte.Y, 0);
                    if (CT.LIST.Count > 1 && (r != (CT.LIST.Count - 1)))
                    {
                        pt3 = new Point3d(pt3.X + 20, pt3.Y, 0);// dibujamos barra horizontal
                        this.DrawLine(pte, pt3,0, LineWeight.LineWeight040);
                    }
                    pte = pt3;
                }
            }
            di = di - 1;
            return pte;
        }
       public void DIBINT(Point3d pt1, COMPONENTE CTEMP)
        {
            Point3d S5 = new Point3d(pt1.X+3, pt1.Y + 20, 0);
            string str = Math.Round(CTEMP.IR, 2).ToString()+" / "+ Math.Round(CTEMP.IS, 2).ToString() + " / " + Math.Round(CTEMP.IT, 2).ToString();
            DrawText(S5, str ); 
            S5 = new Point3d(S5.X, S5.Y - 1.5, 0);
            str = Math.Round(CTEMP.IMR, 2).ToString() + " / " + Math.Round(CTEMP.IMS, 2).ToString() + " / " + Math.Round(CTEMP.IMT, 2).ToString();
            DrawText(S5, str);

        }
        public Point3d DIB(Point3d pt1, COMPONENTE CTEMP, int dd=0)
        {
            TERMICO TTEMP = CTEMP as TERMICO;
            DIFF DTEMP = CTEMP as DIFF;
            CONDUCTOR LTEMP = CTEMP as CONDUCTOR;
            CIRCUIT CCTEMP = CTEMP as CIRCUIT;
            //Point3d pt = new Point3d(pt1.X + 1, pt1.Y-16, 0);     //descomentar para DEBUG intensidades
            //DIBINT(pt, CTEMP);
            if (TTEMP != null)
            {

                if (TTEMP.trif)
                {
                    InsertBlock(pt1, "TERMICO-4P");
                }
                else
                {
                    InsertBlock(pt1, "TERMICO-2P");
                }
                Point3d pt2 = new Point3d(pt1.X + 6, pt1.Y - 1.7, 0);
                string str = Polos(TTEMP.trif) + "/" + TTEMP.IC.ToString() + "A\nCurva: "+TTEMP.CURVA.ToString()+"\nIcc: " + TTEMP.ICC.ToString() + "kA";

                DrawText(pt2, str, 1.6);
            }
            else if (DTEMP != null){
                if (DTEMP.trif)
                {
                    InsertBlock(pt1, "DIFF-4P");
                }
                else
                {
                    InsertBlock(pt1, "DIFF-2P");
                }

                Point3d pt2 = new Point3d(pt1.X + 6, pt1.Y - 1.7, 0);
                string str = Polos(DTEMP.trif) + "/" + DTEMP.IC.ToString() + " A\n" + DTEMP.IDD + " mA\nTipo: "+DTEMP.TIPO.ToString();
                DrawText(pt2, str, 1.6);

            }
            else if (LTEMP != null){
                if (LTEMP.trif)
                {
                    InsertBlock(pt1, "CONDUCTOR-5");
                }
                else
                {
                    InsertBlock(pt1, "CONDUCTOR-3");
                }

                Point3d pt2 = new Point3d(pt1.X + 2, pt1.Y - 15, 0);
                double cdtp = LTEMP.cdtv / C.V;
                int polos;
                if (LTEMP.trif)
                {
                    polos = 4;
                }else{
                    polos = 2;
                }
                string str="";
                switch (LTEMP.Aisl)
                {
                    case 0:
                        str = "RZ1-K Cu";
                        break;
                    case 1:
                        str = "H07Z1-K Cu";
                        break;
                    default:
                        break;
                }
                str = str+"\nL: " + LTEMP.L + "ml. " + polos.ToString() + "x" + LTEMP.S.ToString() + "mm + TT \nIn:"+Math.Round(LTEMP.IN(),2).ToString()+" A / Imax: " + LTEMP.IC.ToString() + " A\ncdt: " + Math.Round(LTEMP.cdtv, 2).ToString() + "V (" + Math.Round(cdtp*100, 2).ToString() + "%)";
                DrawText(pt2, str, 1.4, 0, 1.571,false,30);
            }
            else if (CCTEMP != null){
  
                Point3d pt2 = new Point3d(pt1.X, pt1.Y - 20 * (dd - 1), 0);
                if (dd != 0)
                {
                    DrawLine(pt1, pt2);
                }
                string fase;
                InsertBlock(pt2, "CIRCUIT");
                Point3d pt3 = new Point3d(pt2.X - 5, pt2.Y - 12, 0);
                string vv = CTEMP.trif ?"400": "230";
                DrawText(pt3, "P: " + CCTEMP.P + "W\n" + Math.Round(Math.Max(CTEMP.IR, Math.Max(CTEMP.IS, CTEMP.IT)), 2) + "A / "+vv+"V\n" + CCTEMP.NAME + "\n", 1.8);
                Point3d pt4 = new Point3d(pt3.X +5.75, pt3.Y +8.25, 0);
                int col = 0;
                switch (CTEMP.F)
                {
                    case 'U' :
                        fase = "RST";
                        col = 76;
                        break;
                    case 'T' :
                        fase = "T";
                        col = 5;
                        break;
                    case 'S':
                        fase = "S";
                        col = 212;
                        break;
                    default:
                        fase = "R";
                        col = 1;
                        break;

                }
                DrawText(pt4, fase,1.8,col);
                Point3d pt5 = new Point3d(pt3.X+5,pt3.Y+4,0);
                DrawText(pt5, cnum.ToString(),1.2,0,0,true);
                cnum += 1;
            }
            Point3d ptz = new Point3d(pt1.X, pt1.Y, 0);
            return ptz;
        }
        //******DIBUJO COMP******//    
        public string Polos(bool vv)
        {
            if (vv)
            {
                return "4P";
            }
            else
            {
                return "2P";
            }
        }
        //******DIBUJO METODOS INTERNOS******
        public void DrawLine(Point3d pt1, Point3d pt2, int c =0, LineWeight w= LineWeight.LineWeight000)
        {
            Document Doc = Application.DocumentManager.MdiActiveDocument;
            Database db = Doc.Database;
            Editor edt = Doc.Editor;
            using (DocumentLock acLckDoc = Doc.LockDocument())
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    try
                    {
                        BlockTable bt;//le decimos donde tiene que dibujar
                        bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord btr;
                        btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                        //      edt.WriteMessage("\nDrawing a line Object: ");                    //send object to user
                        Line ln = new Line(pt1, pt2);
                        ln.ColorIndex = c;
                        ln.LineWeight = w;
                        btr.AppendEntity(ln);//guardamos el objeto
                        trans.AddNewlyCreatedDBObject(ln, true);
                        trans.Commit();
                    }
                    catch (System.Exception ex)
                    {
                        edt.WriteMessage("Error encountered: " + ex.Message);
                        trans.Abort();
                    }
                }
            }
         //   db.LineWeightDisplay = true;
        }
        public void DrawCircle(Point3d pt1, double R = 1, int c =0)
        {
            Document Doc = Application.DocumentManager.MdiActiveDocument;
            Database db = Doc.Database;
            Editor edt = Doc.Editor;
            using (DocumentLock acLckDoc = Doc.LockDocument())
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    try
                    {
                        BlockTable bt;//le decimos donde tiene que dibujar
                        bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord btr;
                        btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;                    //send object to user
                        Vector3d normal = new Vector3d(0, 0, 1);
                        Circle cr = new Circle(pt1, normal, R);
                        cr.ColorIndex = c;
                        btr.AppendEntity(cr);
                        trans.AddNewlyCreatedDBObject(cr, true);
                        trans.Commit();
                    }
                    catch (System.Exception ex)
                    {
                        edt.WriteMessage("Error encountered: " + ex.Message);
                        trans.Abort();
                    }
                }
            }
        }
        public void DrawText(Point3d pt1, string str = "TEXTE", double w = 1.2,int col =0, double rot=0, bool center=false, int width = 20)
        {
            Document Doc = Application.DocumentManager.MdiActiveDocument;
            Database db = Doc.Database;
            Editor edt = Doc.Editor;
            using (DocumentLock acLckDoc = Doc.LockDocument())
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    try
                    {
                        BlockTable bt;//le decimos donde tiene que dibujar
                        bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord btr;
                        btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;                    //send object to user
                        MText txt = new MText(); //creamos el objeto i definimos sus propiedades
                        txt.Location = pt1;
                        txt.TextHeight = w;
                        txt.Contents = str;
                        txt.ColorIndex = col;
                        txt.Rotation = rot;
                        txt.Width = width;
                        if (center)
                        {
                            txt.Attachment = AttachmentPoint.MiddleCenter;
                        }

                        btr.AppendEntity(txt);
                        trans.AddNewlyCreatedDBObject(txt, true);
                        trans.Commit();
                    }
                    catch (System.Exception ex)
                    {
                        edt.WriteMessage("Error encountered: " + ex.Message);
                        trans.Abort();
                    }
                }
            }
        }
        public void DrawRect(Point3d pt0,double H, double L,int c = 0)
        {
            Document Doc = Application.DocumentManager.MdiActiveDocument;
            Database db = Doc.Database;
            Editor edt = Doc.Editor;
            using (DocumentLock acLckDoc = Doc.LockDocument())
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    try
                    {
                        BlockTable bt;//le decimos donde tiene que dibujar
                        bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord btr;
                        btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;                    //send object to user
                        Point2d pt1 = new Point2d(pt0.X, pt0.Y);
                        Point2d pt2 = new Point2d(pt1.X+L, pt1.Y);
                        Point2d pt3 = new Point2d(pt1.X+L, pt1.Y-H);
                        Point2d pt4 = new Point2d(pt1.X, pt1.Y-H);
                        Polyline Rec = new Polyline();
                        Rec.AddVertexAt(0, pt1, 0, 0, 0);
                        Rec.AddVertexAt(1, pt2, 0, 0, 0);
                        Rec.AddVertexAt(2, pt3, 0, 0, 0);
                        Rec.AddVertexAt(3, pt4, 0, 0, 0);
                        Rec.AddVertexAt(4, pt1, 0, 0, 0);
                        Rec.ColorIndex = c;
                        btr.AppendEntity(Rec);
                        trans.AddNewlyCreatedDBObject(Rec, true);
                        trans.Commit();
                    }
                    catch (System.Exception ex)
                    {
                        edt.WriteMessage("Error encountered: " + ex.Message);
                        trans.Abort();
                    }
                }
            }
        }
        //*****BLOQUES*****//
        public void InsertBlock(Point3d insPt, string blockName)
        {
            var Doc = Application.DocumentManager.MdiActiveDocument;
            var db = Doc.Database;
            var edt = Doc.Editor;
            using (DocumentLock acLckDoc = Doc.LockDocument())
            {
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    var bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);                // check if the block table already has the 'blockName'" block
                    if (!bt.Has(blockName))
                    {
                        try
                        {
                            // search for a dwg file named 'blockName' in AutoCAD search paths
                            var filename = HostApplicationServices.Current.FindFile(blockName + ".dwg", db, FindFileHint.Default);
                            // add the dwg model space as 'blockName' block definition in the current database block table
                            using (var sourceDb = new Database(false, true))
                            {
                                sourceDb.ReadDwgFile(filename, FileOpenMode.OpenForReadAndAllShare, true, "");
                                db.Insert(blockName, sourceDb, true);
                            }
                        }
                        catch
                        {
                            edt.WriteMessage($"\nCreant Bloc: '{blockName}' ");
                            var sourceDb = new Database(false, true);
                            CreateBlock(blockName);
                        //    db.Insert(blockName,sourceDb, true);
                          //  return;
                        }
                    }
                    using (var br = new BlockReference(insPt, bt[blockName]))                // create a new block reference
                    {
                        var space = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                        space.AppendEntity(br);
                        tr.AddNewlyCreatedDBObject(br, true);
                    }
                    tr.Commit();
                }
            }
        }
        [CommandMethod("CREARBLOCS")]
        public void CreateAll()
        {
            CreateBlock("TERMICO-2P");
            CreateBlock("TERMICO-4P");
            CreateBlock("DIFF-2P");
            CreateBlock("DIFF-4P");
            CreateBlock("CONDUCTOR-3");
            CreateBlock("CONDUCTOR-5");
            CreateBlock("CIRCUIT");
        }
        
        public void CreateBlock(string blockName)
        {
            Database acCurDb;
            acCurDb = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;// Open the Block table for read
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                using (BlockTableRecord acBlkTblRec = new BlockTableRecord())
                {
                    acBlkTblRec.Name = blockName;
                    //idTerm = acBlkTblRec.Id;
                    acBlkTblRec.Origin = new Point3d(0, 0, 0);
                    using (Line3d ln0=new Line3d())
                    {
                        switch (blockName)
                        {
                            case "TERMICO-4P":
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0.991, -3.8777, 0), new Point3d(2.6022, -4.4933, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0.1451, -5.9932, 0), new Point3d(1.8362, -6.6089, 0)));

                                acBlkTblRec.AppendEntity(new Line(new Point3d(0.6582, -4.5837, 0), new Point3d(2.3493, -5.1994, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0.3979, -5.2872, 0), new Point3d(2.0891, -5.9028, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7362, -1.4825, 0), new Point3d(0, -9, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7362, -1.4825, 0), new Point3d(3.3, -1.6877, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7869, -3.0972, 0), new Point3d(3.3, -1.6877, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7869, -3.0972, 0), new Point3d(2.2231, -2.892, 0)));
                                acBlkTblRec.AppendEntity(new Circle(new Point3d(0, -9.5, 0), normal, .5));
                                acBlkTblRec.AppendEntity(new Circle(new Point3d(0, -.5, 0), normal, .5));
                                break;
                            case "TERMICO-2P":
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0.6582, -4.5837, 0), new Point3d(2.3493, -5.1994, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0.3979, -5.2872, 0), new Point3d(2.0891, -5.9028, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7362, -1.4825, 0), new Point3d(0, -9, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7362, -1.4825, 0), new Point3d(3.3, -1.6877, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7869, -3.0972, 0), new Point3d(3.3, -1.6877, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7869, -3.0972, 0), new Point3d(2.2231, -2.892, 0)));
                                acBlkTblRec.AppendEntity(new Circle(new Point3d(0, -9.5, 0), normal, .5));
                                acBlkTblRec.AppendEntity(new Circle(new Point3d(0, -.5, 0), normal, .5));
                                break;
                            case "DIFF-4P":
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0.991, -3.8777, 0), new Point3d(2.6022, -4.4933, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0.1451, - 5.9932, 0), new Point3d(1.8362, -6.6089, 0)));

                                acBlkTblRec.AppendEntity(new Line(new Point3d(0.6582, -4.5837, 0), new Point3d(2.3493, -5.1994, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0.3979, -5.2872, 0), new Point3d(2.0891, -5.9028, 0)));

                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7362, -1.4825, 0), new Point3d(0, -9, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7362, -1.4825, 0), new Point3d(3.3, -1.6877, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7869, -3.0972, 0), new Point3d(3.3, -1.6877, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7869, -3.0972, 0), new Point3d(2.2231, -2.892, 0)));
                                acBlkTblRec.AppendEntity(new Circle(new Point3d(0, -9.5, 0), normal, .5));
                                acBlkTblRec.AppendEntity(new Circle(new Point3d(0, -.5, 0), normal, .5));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(3, -5.5, 0), new Point3d(1.2739, -5.5, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(.5, -9.5, 0), new Point3d(4, -9.5, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(4, -9.5, 0), new Point3d(4, -6.5, 0)));
                                Point3d PP1 = new Point3d(3, -4.5, 0);

                                acBlkTblRec.AppendEntity(new Line(PP1, new Point3d(PP1.X + 2, PP1.Y, 0)));
                                acBlkTblRec.AppendEntity(new Line(PP1, new Point3d(PP1.X, PP1.Y - 2, 0)));
                                PP1 = new Point3d(PP1.X + 2, PP1.Y - 2, 0);
                                acBlkTblRec.AppendEntity(new Line(PP1, new Point3d(PP1.X - 2, PP1.Y, 0)));
                                acBlkTblRec.AppendEntity(new Line(PP1, new Point3d(PP1.X, PP1.Y + 2, 0)));
                                break;

                            case "DIFF-2P":
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0.6582, -4.5837, 0), new Point3d(2.3493, -5.1994, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0.3979, -5.2872, 0), new Point3d(2.0891, -5.9028, 0)));

                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7362, -1.4825, 0), new Point3d(0, -9, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7362, -1.4825, 0), new Point3d(3.3, -1.6877, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7869, -3.0972, 0), new Point3d(3.3, -1.6877, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(2.7869, -3.0972, 0), new Point3d(2.2231, -2.892, 0)));
                                acBlkTblRec.AppendEntity(new Circle(new Point3d(0, -9.5, 0), normal, .5));
                                acBlkTblRec.AppendEntity(new Circle(new Point3d(0, -.5, 0), normal, .5));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(3, -5.5, 0), new Point3d(1.2739, -5.5, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(.5, -9.5, 0), new Point3d(4, -9.5, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(4, -9.5, 0), new Point3d(4, -6.5, 0)));
                                Point3d PP11 = new Point3d(3, -4.5, 0);

                                acBlkTblRec.AppendEntity(new Line(PP11,new Point3d(PP11.X+2,PP11.Y,0)));
                                acBlkTblRec.AppendEntity(new Line(PP11, new Point3d(PP11.X, PP11.Y-2, 0)));
                                PP11= new Point3d(PP11.X+2,PP11.Y-2, 0);
                                acBlkTblRec.AppendEntity(new Line(PP11, new Point3d(PP11.X - 2, PP11.Y, 0)));
                                acBlkTblRec.AppendEntity(new Line(PP11, new Point3d(PP11.X, PP11.Y+2, 0)));

                                break;
                            case "CONDUCTOR-5":
                                acBlkTblRec.AppendEntity(new Line(new Point3d(-1, -6.75, 0), new Point3d(1, -4.75, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(-1, -7.5, 0), new Point3d(1, -5.5, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(-1, -3.5, 0), new Point3d(1, -1.5, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(-1, -4.25, 0), new Point3d(1, -2.25, 0)));

                                acBlkTblRec.AppendEntity(new Line(new Point3d(-1, -6, 0), new Point3d(1, -4, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0, -10, 0), new Point3d(0, 0, 0)));
                                break;
                            case "CONDUCTOR-3":
                                acBlkTblRec.AppendEntity(new Line(new Point3d(-1, -3.5, 0), new Point3d(1, -1.5, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(-1, -4.25, 0), new Point3d(1, -2.25, 0)));

                                acBlkTblRec.AppendEntity(new Line(new Point3d(-1, -6, 0), new Point3d(1, -4, 0)));
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0, -10, 0), new Point3d(0, 0, 0)));
                                break;

                                break;
                            case "CIRCUIT":
                                acBlkTblRec.AppendEntity(new Line(new Point3d(0, -6, 0), new Point3d(0, 0, 0)));
                                acBlkTblRec.AppendEntity(new Circle(new Point3d(0, -8, -0), normal, 2));
                                break;
                            default:
                                break;
                        }

                        acBlkTbl.UpgradeOpen();
                        acBlkTbl.Add(acBlkTblRec);
                        acTrans.AddNewlyCreatedDBObject(acBlkTblRec, true);
                    }
                }
                acTrans.Commit();// Save the new object to the database
            }
        }


        //******MetodosGUIaccesibles******//
       /*
        [CommandMethod("INSERTCOMP")]
        public void InsertCOMP()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;
            PromptIntegerOptions pIntOpts = new PromptIntegerOptions("");
            pIntOpts.Message = "\nTipo:";
            pIntOpts.Keywords.Add("Termic");
            pIntOpts.Keywords.Add("Diferencial");
            pIntOpts.Keywords.Add("Cable");
            pIntOpts.Keywords.Add("Circuit");
            pIntOpts.Keywords.Default = "Termic";
            pIntOpts.AllowNone = true;

            PromptIntegerResult pIntRes = ed.GetInteger(pIntOpts);
            Point3d p1 = Cargarpunto("Insereix Punt:");
            switch (pIntRes.StringResult)
            {
                case "Termic":
                    int vv = estrif();
                    int ii = Cargarint("I:");
                    int iicc = Cargarint("ICC:");
                    //InsertTerm(p1, ii, vv, iicc);
                    break;
                case "Diferencial":
                    int vv2 = estrif();
                    int ii2 = Cargarint("I:");
                    int iidd = Cargarint("ID:");
                    //InsertDiff(p1, ii2, vv2, iidd);
                    break;
                case "Cable":
                    int vv3 = estrif();
                    bool RZZ = Cargarbool("Tipo:", "RZ1-K", "HZ07-K");
                    int ss = Cargarint("S:");
                    //InsertCond(p1, ss, vv3);
                    break;
                case "Circuit":
                    int vv4 = estrif();
                    int P = Cargarint("P:");
                    //InsertCir(p1, P, vv4);
                    break;
                default:
                    break;
            }
        }*/
        //******MetodosGUIinternos******//
        public void CMDline(string str)
        {
            Document Doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor edt = Doc.Editor;
            edt.WriteMessage(str);
        }
        public Point3d Cargarpunto(string str = "P")
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;
            PromptPointResult promptResult = ed.GetPoint(str);
            Point3d Pinsert = promptResult.Value;

            if (promptResult.Status != PromptStatus.OK)
                return new Point3d(0, 0, 0);

            return Pinsert;
        }
        public int Cargarint(string str = "I")
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;

            PromptIntegerOptions pIntOpts = new PromptIntegerOptions("");
            pIntOpts.Message = str;
            pIntOpts.AllowZero = false;            // Restrict input to positive and non-negative values
            pIntOpts.AllowNegative = false;

            return ed.GetInteger(pIntOpts).Value;
        }
      
        public bool Cargarbool(string str = "Confirmar:", string str1 = "S", string str2 = "N")
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;

            PromptIntegerOptions pIntOpts = new PromptIntegerOptions("");
            pIntOpts.Message = str;
            pIntOpts.Keywords.Add(str1);
            pIntOpts.Keywords.Add(str2);

            return ed.GetInteger(pIntOpts).StringResult == str1;

        }

    }
}
