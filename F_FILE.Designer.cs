namespace ProyectoCOM_03
{
    partial class F_FILE
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
            this.F1_PANLFT = new System.Windows.Forms.Panel();
            this.B_SAVEAS = new System.Windows.Forms.Button();
            this.B_SAVE = new System.Windows.Forms.Button();
            this.B_OPEN = new System.Windows.Forms.Button();
            this.B_NEW = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.F1_PANLFT.SuspendLayout();
            this.SuspendLayout();
            // 
            // F1_PANLFT
            // 
            this.F1_PANLFT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(51)))));
            this.F1_PANLFT.Controls.Add(this.B_SAVEAS);
            this.F1_PANLFT.Controls.Add(this.B_SAVE);
            this.F1_PANLFT.Controls.Add(this.B_OPEN);
            this.F1_PANLFT.Controls.Add(this.B_NEW);
            this.F1_PANLFT.Dock = System.Windows.Forms.DockStyle.Left;
            this.F1_PANLFT.Location = new System.Drawing.Point(0, 0);
            this.F1_PANLFT.Name = "F1_PANLFT";
            this.F1_PANLFT.Size = new System.Drawing.Size(200, 450);
            this.F1_PANLFT.TabIndex = 0;
            // 
            // B_SAVEAS
            // 
            this.B_SAVEAS.Dock = System.Windows.Forms.DockStyle.Top;
            this.B_SAVEAS.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.B_SAVEAS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_SAVEAS.Font = new System.Drawing.Font("Segoe UI", 9.969231F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_SAVEAS.ForeColor = System.Drawing.Color.White;
            this.B_SAVEAS.Location = new System.Drawing.Point(0, 150);
            this.B_SAVEAS.Name = "B_SAVEAS";
            this.B_SAVEAS.Size = new System.Drawing.Size(200, 50);
            this.B_SAVEAS.TabIndex = 3;
            this.B_SAVEAS.Text = "Guardar Com";
            this.B_SAVEAS.UseVisualStyleBackColor = true;
            this.B_SAVEAS.Click += new System.EventHandler(this.B_SAVEAS_Click);
            // 
            // B_SAVE
            // 
            this.B_SAVE.Dock = System.Windows.Forms.DockStyle.Top;
            this.B_SAVE.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.B_SAVE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_SAVE.Font = new System.Drawing.Font("Segoe UI", 9.969231F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_SAVE.ForeColor = System.Drawing.Color.White;
            this.B_SAVE.Location = new System.Drawing.Point(0, 100);
            this.B_SAVE.Name = "B_SAVE";
            this.B_SAVE.Size = new System.Drawing.Size(200, 50);
            this.B_SAVE.TabIndex = 2;
            this.B_SAVE.Text = "Guardar";
            this.B_SAVE.UseVisualStyleBackColor = true;
            this.B_SAVE.Click += new System.EventHandler(this.B_SAVE_Click);
            // 
            // B_OPEN
            // 
            this.B_OPEN.Dock = System.Windows.Forms.DockStyle.Top;
            this.B_OPEN.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.B_OPEN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_OPEN.Font = new System.Drawing.Font("Segoe UI", 9.969231F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_OPEN.ForeColor = System.Drawing.Color.White;
            this.B_OPEN.Location = new System.Drawing.Point(0, 50);
            this.B_OPEN.Name = "B_OPEN";
            this.B_OPEN.Size = new System.Drawing.Size(200, 50);
            this.B_OPEN.TabIndex = 1;
            this.B_OPEN.Text = "Obrir";
            this.B_OPEN.UseVisualStyleBackColor = true;
            this.B_OPEN.Click += new System.EventHandler(this.B_OPEN_Click);
            // 
            // B_NEW
            // 
            this.B_NEW.Dock = System.Windows.Forms.DockStyle.Top;
            this.B_NEW.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.B_NEW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_NEW.Font = new System.Drawing.Font("Segoe UI", 9.969231F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_NEW.ForeColor = System.Drawing.Color.White;
            this.B_NEW.Location = new System.Drawing.Point(0, 0);
            this.B_NEW.Name = "B_NEW";
            this.B_NEW.Size = new System.Drawing.Size(200, 50);
            this.B_NEW.TabIndex = 0;
            this.B_NEW.Text = "Nou";
            this.B_NEW.UseVisualStyleBackColor = true;
            this.B_NEW.Click += new System.EventHandler(this.B_NEW_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // F_FILE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(68)))), ((int)(((byte)(83)))));
            this.ClientSize = new System.Drawing.Size(182, 450);
            this.Controls.Add(this.F1_PANLFT);
            this.Name = "F_FILE";
            this.Text = "F_FILE";
            this.F1_PANLFT.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel F1_PANLFT;
        private System.Windows.Forms.Button B_OPEN;
        private System.Windows.Forms.Button B_NEW;
        private System.Windows.Forms.Button B_SAVEAS;
        private System.Windows.Forms.Button B_SAVE;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}