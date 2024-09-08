namespace ProyectoCOM_03
{
    partial class F_CIRCUITS
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.gentree = new System.Windows.Forms.Button();
            this.PARAM_BT = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.TaulaCircuits = new System.Windows.Forms.TableLayoutPanel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(68)))), ((int)(((byte)(83)))));
            this.panel1.Controls.Add(this.PARAM_BT);
            this.panel1.Controls.Add(this.gentree);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(943, 30);
            this.panel1.TabIndex = 0;
            // 
            // gentree
            // 
            this.gentree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(68)))), ((int)(((byte)(83)))));
            this.gentree.Dock = System.Windows.Forms.DockStyle.Right;
            this.gentree.FlatAppearance.BorderSize = 0;
            this.gentree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gentree.Font = new System.Drawing.Font("Verdana", 7.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gentree.ForeColor = System.Drawing.Color.White;
            this.gentree.Location = new System.Drawing.Point(793, 0);
            this.gentree.Name = "gentree";
            this.gentree.Size = new System.Drawing.Size(150, 30);
            this.gentree.TabIndex = 2;
            this.gentree.Text = "CIRCUITOS >>";
            this.gentree.UseVisualStyleBackColor = false;
            this.gentree.Click += new System.EventHandler(this.gentree_Click);
            // 
            // PARAM_BT
            // 
            this.PARAM_BT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(68)))), ((int)(((byte)(83)))));
            this.PARAM_BT.Dock = System.Windows.Forms.DockStyle.Left;
            this.PARAM_BT.FlatAppearance.BorderSize = 0;
            this.PARAM_BT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PARAM_BT.Font = new System.Drawing.Font("Segoe UI", 7.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PARAM_BT.ForeColor = System.Drawing.Color.White;
            this.PARAM_BT.Location = new System.Drawing.Point(0, 0);
            this.PARAM_BT.Name = "PARAM_BT";
            this.PARAM_BT.Size = new System.Drawing.Size(97, 30);
            this.PARAM_BT.TabIndex = 4;
            this.PARAM_BT.Text = "<< Enrrera";
            this.PARAM_BT.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.TaulaCircuits);
            this.panel2.Location = new System.Drawing.Point(20, 54);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(899, 371);
            this.panel2.TabIndex = 1;
            // 
            // TaulaCircuits
            // 
            this.TaulaCircuits.ColumnCount = 2;
            this.TaulaCircuits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.05263F));
            this.TaulaCircuits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.94737F));
            this.TaulaCircuits.Location = new System.Drawing.Point(22, 20);
            this.TaulaCircuits.Name = "TaulaCircuits";
            this.TaulaCircuits.RowCount = 2;
            this.TaulaCircuits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TaulaCircuits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TaulaCircuits.Size = new System.Drawing.Size(843, 64);
            this.TaulaCircuits.TabIndex = 0;
            // 
            // F_CIRCUITS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(166)))), ((int)(((byte)(176)))));
            this.ClientSize = new System.Drawing.Size(943, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "F_CIRCUITS";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button gentree;
        private System.Windows.Forms.Button PARAM_BT;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel TaulaCircuits;
    }
}