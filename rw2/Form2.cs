using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoLSetupWizard
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.Text = "Ne";
            button2.Text = "Ano";
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button1.Text = "Ano";
            button2.Text = "Ne";
        }

        private void GlobalButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
