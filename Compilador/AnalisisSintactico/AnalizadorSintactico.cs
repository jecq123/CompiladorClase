using Compilador.Error;
using CompiladorClase.AnalisisLexico;
using CompiladorClase.Trasnversal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Compilador.AnalisisSintactico
{
    public class AnalizadorSintactico
    {
        private ComponenteLexico componente;
        private AnalizadorLexico analizadorLexico =  AnalizadorLexico.crear();

        public void analizar()
        {
            pedirComponente();
            primera();

            if (ManejadorError.obtenerManejadorError().hayErrores())
            {
                MessageBox.Show("Hay errores de compilacion. Por favor verifique los reportes de error respectivos!!!");
            }

            else if ("FIN_ARCHIVO".Equals(componente.obtenerCategoria().ToUpper()))
            {
                MessageBox.Show("El programa se encuentra bien escrito");
            }
            else
            {
                MessageBox.Show("Faltaron componentes por evaluar sintacticamente");
            }
        }
        private void pedirComponente()
        {
            componente = analizadorLexico.devolderSiguienteComponente();
        }
        private void primera()
        {
            segunda();
            primeraPrima();

        }
        private void primeraPrima()
        {
            if ("SUMA".Equals(componente.obtenerCategoria().ToUpper()))
            {
                pedirComponente();
                primera();
            }
            else if ("RESTA".Equals(componente.obtenerCategoria().ToUpper()))
            {
                pedirComponente();
                primera();
            }
            else
            {
                //epsilon
            }

        }

        private void segunda()
        {

            tercera();
            segundaPrima();
        }

        private void segundaPrima()
        {
            if ("MULTIPLICACION".Equals(componente.obtenerCategoria().ToUpper()))
            {
                pedirComponente();
                segunda();
            }
            else if ("DIVISION".Equals(componente.obtenerCategoria().ToUpper()))
            {
                pedirComponente();
                segunda();
            }
            else
            {
                //epsilon
            }


        }

        private void tercera()
        {
            if ("ENTERO".Equals(componente.obtenerCategoria().ToUpper()))
            {
                pedirComponente();
            }
            else if ("DECIMAL".Equals(componente.obtenerCategoria().ToUpper()))
            {
                pedirComponente();
            }
            else if ("PARENTESIS_ABRE".Equals(componente.obtenerCategoria().ToUpper()))
            {
                pedirComponente();
                primera();

                if ("PARENTESIS_CIERRA".Equals(componente.obtenerCategoria().ToUpper()))
                {
                    pedirComponente();
                }
                else
                {
                    ManejadorError.obtenerManejadorError().agregar(
                        ComponenteError.crearErrorSintactico(componente.obtenerNumeroLinea(), componente.obtenerPosicionInicial(), componente.obtenerPosicionFinal(),
                        "Componente no esperado",
                        "no es parentesis cierra",
                        "Asegurese que en esta posicion exista un componente de tipo parentesis cierra"));
                    //Como reporto este error sintactico
                    //Falla: Componente no esperado
                    //causa: no es parentesis cierra
                    //Solucion: Asegurese que en esta posicion exista un componente de tipo parentesis cierra
                    //Reportar error
                    //throw new Exception("Se presento error");
                }
            }
            else
            {
                ManejadorError.obtenerManejadorError().agregar(
                        ComponenteError.crearErrorSintactico(componente.obtenerNumeroLinea(), componente.obtenerPosicionInicial(), componente.obtenerPosicionFinal(),
                        "Componente no esperado",
                        "no es entero, decimal o parentesis abre",
                        "Asegurese que en esta posicion exista un componente de tipo entero, decimal o parentesis abre"));
                //Como reporto este error sintactico
                //Falla: Componente no esperado
                //causa: no es entero, decimal o parentesis abre
                //Solucion: Asegurese que en esta posicion exista un componente de tipo entero, decimal o parentesis abre
                //Reportar error
                throw new Exception("Se presento un error de tipo stopper durante el analizis sintactico , por favor verifique la consola de errores!!!");
            }
        }
    }
}
