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
    public partial class F_CIRCUITS : Form
    {
        public F_CIRCUITS()
        {
            InitializeComponent();
        }

        private void gentree_Click(object sender, EventArgs e)
        {

        }
        public void newrow()
        {
            
            TaulaCircuits.ColumnCount = 3;
            TaulaCircuits.RowCount = 5;
            TaulaCircuits.Controls.Add(new TextBox(), 1, TaulaCircuits.RowCount-1);
            TaulaCircuits.Controls.Add(new NumericUpDown(), 2, TaulaCircuits.RowCount - 1);
         //   NumericUpDown N1 = TaulaCircuits.Controls.Container as NumericUpDown;
        }
    }
}
