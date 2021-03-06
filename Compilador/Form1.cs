using Compilador.AnalisisSintactico;
using Compilador.Error;
using Compilador.TablaComponentes;
using CompiladorClase.AnalisisLexico;
using CompiladorClase.Cache;
using CompiladorClase.Trasnversal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        

        private void btnCompilar_Click(object sender, EventArgs e)
        {
            Tabla tabla = Tabla.obtenerTabla();
            tabla.Reiniciar();
            ManejadorError.obtenerManejadorError().reiniciar();
            ProgramaFuente cache = ProgramaFuente.obtenerProgramaFuente();

            txtConsole.Text = String.Empty;
            procesarTexto();
            foreach (Linea linea in cache.obtenerLineas())
            {
                txtConsole.AddLine(linea.obtenerNumeroLinea() + ">> " + linea.obtenerContenido());
            }
            

            try
            {
                
                    AnalizadorSintactico anaSin = new AnalizadorSintactico();
                //anaSin.analizar();
                MessageBox.Show(anaSin.analizar());
                
            } catch (Exception error)
            {
                MessageBox.Show("Error Stopper, no es posible coninuar la ejecución del programa");
                
            }
            
            agregarTablas();
            listarErrores();
        }

        private void procesarTexto()
        {
            ProgramaFuente cache = ProgramaFuente.obtenerProgramaFuente();
            cache.inicializar();

            if (rbtnText.Checked)
            {
                foreach (String valorLinea in txtLines.Lines)
                {
                    cache.agregarLinea(valorLinea);
                }
                cache.agregarLinea(CategoriaGramatical.FIN_ARCHIVO);
            }
            else if (rbtnFile.Checked)
            {
                if (label1.Text != "")
                {
                    string[] lines;


                    using (StreamReader sr = new StreamReader(label1.Text))
                    {
                        String file = sr.ReadToEnd();
                        lines = file.Split("\r\n");
                    }
                    foreach (String valorLinea in lines)
                    {
                        cache.agregarLinea(valorLinea);
                    }
                    cache.agregarLinea(CategoriaGramatical.FIN_ARCHIVO);
                }

            }
        }
        private void agregarTablas()
        {
            Tabla tabla = Tabla.obtenerTabla();
            var dummyData = tabla.ObtenerComponentes(Trasnversal.TipoComponente.DUMMY)
                .Select(r => new
                {
                    numeroLinea = r.obtenerNumeroLinea().ToString(),
                    posicionInicial = r.obtenerPosicionInicial().ToString(),
                    posicionFinal = r.obtenerPosicionFinal().ToString(),
                    categoria = r.obtenerCategoria(),
                    lexema = r.obtenerLexema()
                }).ToList();
            dataGridView1.DataSource = dummyData;
            var literalData = tabla.ObtenerComponentes(Trasnversal.TipoComponente.LITERAL)
                .Select(r => new
                {
                    numeroLinea = r.obtenerNumeroLinea().ToString(),
                    posicionInicial = r.obtenerPosicionInicial().ToString(),
                    posicionFinal = r.obtenerPosicionFinal().ToString(),
                    categoria = r.obtenerCategoria(),
                    lexema = r.obtenerLexema()
                }).ToList();
            dataGridView2.DataSource = literalData;
            var reservadaData = tabla.ObtenerComponentes(Trasnversal.TipoComponente.PALABRA_RESERVADA)
                .Select(r => new
                {
                    numeroLinea = r.obtenerNumeroLinea().ToString(),
                    posicionInicial = r.obtenerPosicionInicial().ToString(),
                    posicionFinal = r.obtenerPosicionFinal().ToString(),
                    categoria = r.obtenerCategoria(),
                    lexema = r.obtenerLexema()
                }).ToList();
            dataGridView3.DataSource = reservadaData;
            var simboloData = tabla.ObtenerComponentes(Trasnversal.TipoComponente.SIMBOLO)
                .Select(r => new
                {
                    numeroLinea = r.obtenerNumeroLinea().ToString(),
                    posicionInicial = r.obtenerPosicionInicial().ToString(),
                    posicionFinal = r.obtenerPosicionFinal().ToString(),
                    categoria = r.obtenerCategoria(),
                    lexema = r.obtenerLexema()
                }).ToList();
            dataGridView4.DataSource = simboloData;
        }

        private void listarErrores()
        {
            var erroresDGV = ManejadorError.obtenerManejadorError().obtenerErrores()
                .Select(r => new
                {
                    numeroLinea = r.obtenerNumeroLinea().ToString(),
                    posicionInicial = r.obtenerPosicionInicial().ToString(),
                    posicionFinal = r.obtenerPosicionFinal().ToString(),
                    causa = r.obtenerCausa(),
                    falla = r.obtenerFalla(),
                    solucion = r.obtenerSolucion(),
                    tipo = r.obtenerTipo().ToString()
                }).ToList();
            dataGridView5.DataSource = erroresDGV;
        }

        private void rbtnText_CheckedChanged(object sender, EventArgs e)
        {
            cerrarVistas();
            groupBox2.Show();
        }

        private void rbtnFile_CheckedChanged(object sender, EventArgs e)
        {
            cerrarVistas();
            groupBox1.Show();
        }

        private void cerrarVistas()
        {
            groupBox2.Hide();
            groupBox1.Hide();
            groupBox3.Hide();
            groupBox4.Hide();
        }

        private void ListaComponente_CheckedChanged(object sender, EventArgs e)
        {
            cerrarVistas();
            groupBox3.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            cerrarVistas();
            groupBox4.Show();
        }
    }
    public static class WinFormsExtensions
    {
        public static void AddLine(this TextBox source, string value)
        {
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText("\r\n" + value);
        }
    }
}
