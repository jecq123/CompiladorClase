using CompiladorClase.AnalisisLexico;
using CompiladorClase.Trasnversal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilador
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AnalizadorLexico analisador = AnalizadorLexico.crear();
            ComponenteLexico componente = analisador.devolderSiguienteComponente();

            while (!CategoriaGramatical.FIN_ARCHIVO.Equals(componente.obtenerCategoria()))
            {
                MessageBox.Show(componente.formarComponente());
                componente = analisador.devolderSiguienteComponente();
            }
        }
    }
}
