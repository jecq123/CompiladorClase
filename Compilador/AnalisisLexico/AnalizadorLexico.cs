using System;
using System.Collections.Generic;
using System.Text;
using CompiladorClase.Cache;
using CompiladorClase.Trasnversal;

namespace CompiladorClase.AnalisisLexico
{
    class AnalizadorLexico
    {
        private int numeroLineaActual;
        private int apuntador;
        private String caracterActual;
        private String contenidoLineaActual;
        private AnalizadorLexico()
        {
            cargarNuevaLinea();
        }

        public static AnalizadorLexico crear()
        {
            return new AnalizadorLexico();
        }

        private void cargarNuevaLinea()
        {
            numeroLineaActual += 1;
            Linea lineaActual = ProgramaFuente.obtenerProgramaFuente().obtenerLinea(numeroLineaActual);
            contenidoLineaActual = lineaActual.obtenerContenido();
            numeroLineaActual = lineaActual.obtenerNumeroLinea();
            apuntador = 1;
        }

        private void leerSiguienteCaracter()
        {

            if (CategoriaGramatical.FIN_ARCHIVO.Equals(contenidoLineaActual))
            {
                caracterActual = contenidoLineaActual;
            }
            else if (apuntador > contenidoLineaActual.Length)
            {
                caracterActual = CategoriaGramatical.FIN_LINEA;
            }
            else
            {
                caracterActual = contenidoLineaActual.Substring(apuntador - 1, 1);
                apuntador++;
            }
        }
        private void devolverPuntero()
        {
            apuntador--;
        }

        private bool esIgual(String cadena,String cadena2)
        {
            if (cadena == null && cadena2 == null)
            {
                return true;
            }
            else if (cadena == null)
            {
                return false;
            }
            return cadena.Equals(cadena2);
        }
        private bool esLetra()
        {
            return Char.IsLetter(caracterActual.ToCharArray()[0]);
        }
        private bool esDigito()
        {
            return Char.IsDigit(caracterActual.ToCharArray()[0]);
        }
        private bool esPesos()
        {
            return "$".Equals(caracterActual);
        }

        private bool esGuionBajo()
        {
            return "_".Equals(caracterActual);
        }

        private bool esFinLinea()
        {
            return esIgual(CategoriaGramatical.FIN_LINEA, caracterActual);
        }

        public ComponenteLexico devolderSiguienteComponente()
        {
            ComponenteLexico retorno= null;
            int estadoactual = 0;
            string lexema = "";
            bool continuarAnalisis=true;
            while (continuarAnalisis)
            {
                if (estadoactual == 0)
                {
                    leerSiguienteCaracter();
                    while(" ".Equals(caracterActual))
                    {
                        leerSiguienteCaracter();
                    }
                    if (esLetra()||esPesos()||esGuionBajo())
                    {
                        estadoactual = 4;
                        lexema = lexema + caracterActual;
                    }
                    else if (esDigito())
                    {
                        estadoactual = 1;
                        lexema = lexema + caracterActual;
                    }
                    else if (esFinLinea())
                    {
                        estadoactual = 13;
                    }
                    else
                    {
                        estadoactual = 18;
                    }
                }
                else if (estadoactual == 1)
                {

                }
                else if (estadoactual == 2)
                {

                }
                else if (estadoactual == 3)
                {

                }
                else if (estadoactual == 4)
                {
                    leerSiguienteCaracter();
                    if (esLetra() || esDigito() || esGuionBajo() || esPesos())
                    {
                        estadoactual = 4;
                        lexema = lexema + caracterActual;
                    }
                    else
                    {
                        estadoactual = 16;
                    }
                }
                else if (estadoactual == 5)
                {

                }
                else if (estadoactual == 6)
                {

                }
                else if (estadoactual == 7)
                {

                }
                else if (estadoactual == 8)
                {

                }
                else if (estadoactual == 9)
                {

                }
                else if (estadoactual == 10)
                {

                }
                else if (estadoactual == 11)
                {

                }
                else if (estadoactual == 12)
                {

                }
                else if (estadoactual == 13)
                {
                    cargarNuevaLinea();
                    estadoactual = 0;
                }
                else if (estadoactual == 14)
                {

                }
                else if (estadoactual == 15)
                {

                }
                else if (estadoactual == 16)
                {
                    continuarAnalisis = false;
                    devolverPuntero();
                    retorno = ComponenteLexico.crear(numeroLineaActual, apuntador - lexema.Length, apuntador - 1,CategoriaGramatical.IDENTIFICADOR,lexema);

                }
                else if (estadoactual == 17)
                {
                    //falta gestor de errores
                    continuarAnalisis = false;
                    devolverPuntero();
                    retorno = ComponenteLexico.crear(numeroLineaActual, apuntador - lexema.Length, apuntador - 1, CategoriaGramatical.DECIMAL, lexema + "0");
                }
                else if (estadoactual == 18)
                {
                    throw new Exception("Error critico de tipo lexico: Se detuvo el analisis!!!!");
                }
            }
            return retorno;
        }
    }
}
