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
        private Stack<Double> pila = new Stack<double>();
        private StringBuilder sb = new StringBuilder();

        private void formarEntrada(int numeroLlando, string nombreRegla)
        {
            for(int indice = 1; indice<= numeroLlando; indice++)
            {
                sb.Append("....");
            }
            sb.Append("INICIO->");
            sb.Append(nombreRegla);
            sb.Append("->Componente(").Append(componente.obtenerLexema()).Append(",").Append(componente.obtenerCategoria()).Append(")");
            sb.Append("\n");

        }
        private void formarSalida(int numeroLlando, string nombreRegla)
        {
            for (int indice = 1; indice <= numeroLlando; indice++)
            {
                sb.Append("....");
            }
            sb.Append("FIN->");
            sb.Append(nombreRegla);
            sb.Append("\n");


        }
        private void formarOperacion(int numeroLlando, double izquierdo, double derecho, string operacion)
        {
            for (int indice = 1; indice <= numeroLlando; indice++)
            {
                sb.Append("....");
            }
            sb.Append("OPERANDO->").Append(izquierdo).Append(operacion).Append(derecho);
            sb.Append("\n");


        }

        public string analizar()
        {
            pedirComponente();
            primera(0);

            if (ManejadorError.obtenerManejadorError().hayErrores())
            {
                MessageBox.Show("Hay errores de compilacion. Por favor verifique los reportes de error respectivos!!!");
            }

            else if (CategoriaGramatical.FIN_ARCHIVO.Equals(componente.obtenerCategoria().ToUpper()))
            {
                if (pila.Count == 1)
                {
                    MessageBox.Show("El programa se cuentra bien escrito. el resultado de la operacion es: " + pila.Pop());
                }
                else if(pila.Count>1)
                {
                    //MessageBox.Show(pil)
                    MessageBox.Show("Faltaron componentes por evaluar sintacticamente");
                }
                else
                {
                    MessageBox.Show("AUNQUE SE FINALIZO EXITOSAMENTE , NO SE TIENE UN RESULTADO FINAL.");

                }
                //MessageBox.Show("El programa se encuentra bien escrito");
            }
            else
            {
                MessageBox.Show("Faltaron componentes por evaluar sintacticamente");
            }
            return sb.ToString();
        }
        private void pedirComponente()
        {
            componente = analizadorLexico.devolderSiguienteComponente();
        }
        private void primera(int numeroLlamado)
        {
            formarEntrada(numeroLlamado, "<primer>");
            segunda(numeroLlamado + 1 );
            primeraPrima(numeroLlamado + 1);
            formarSalida(numeroLlamado, "</primer>");

        }

        private void primeraPrima(int numeroLlamado)
        {
            formarEntrada(numeroLlamado, "<primer>");
            if (CategoriaGramatical.SUMA.Equals(componente.obtenerCategoria().ToUpper()))
            {
                pedirComponente();
                primera(numeroLlamado+1);
                if (!ManejadorError.obtenerManejadorError().hayErrores())
                {
                    double derecho = pila.Pop();
                    double izquierdor = pila.Pop();

                    pila.Push(izquierdor + derecho);
                    formarOperacion(numeroLlamado + 1, izquierdor, derecho, "+");

                }
            }
            else if (CategoriaGramatical.RESTA.Equals(componente.obtenerCategoria().ToUpper()))
            {
                pedirComponente();
                primera(numeroLlamado + 1);
                if (!ManejadorError.obtenerManejadorError().hayErrores())
                {
                    double derecho = pila.Pop();
                    double izquierdor = pila.Pop();

                    pila.Push(izquierdor - derecho);
                    formarOperacion(numeroLlamado + 1, izquierdor, derecho, "-");
                }
            }
            else
            {
                //epsilon
            }

            formarSalida(numeroLlamado, "</primeraprima>");
        }

        private void segunda(int numeroLlamado)
        {
            formarEntrada(numeroLlamado, "<segundar>");
            tercera(numeroLlamado + 1);
            segundaPrima( numeroLlamado + 1);
            formarSalida(numeroLlamado, "</segunda>");

        }


        private void segundaPrima(int numeroLlamado)
        {
            formarEntrada(numeroLlamado, "<segundarprima>");
            if (CategoriaGramatical.MULTIPLICACION.Equals(componente.obtenerCategoria().ToUpper()))
            {
                pedirComponente();
                segunda(numeroLlamado + 1);
                if (!ManejadorError.obtenerManejadorError().hayErrores())
                {
                    double derecho = pila.Pop();
                    double izquierdor = pila.Pop();

                    pila.Push(izquierdor * derecho);
                    formarOperacion(numeroLlamado + 1, izquierdor, derecho, "*");
                }


            }
            else if (CategoriaGramatical.DIVISION.Equals(componente.obtenerCategoria().ToUpper()))
            {
                pedirComponente();

                segunda(numeroLlamado + 1);
                if (!ManejadorError.obtenerManejadorError().hayErrores())
                {
                    double derecho = pila.Pop();
                    double izquierdor = pila.Pop();

                    if(derecho == 0)
                    {
                        //Revisar los mensajes
                        ManejadorError.obtenerManejadorError().agregar(
                         ComponenteError.crearErrorLexico(componente.obtenerNumeroLinea(), componente.obtenerPosicionInicial(), componente.obtenerPosicionFinal(),
                         "Se esperaba un digito y se recibio " + componente,
                         "No es posible formar un numero decimal con caracteres diferentes a digitos",
                         "Asegurese que en la pocision indicada aparezca un digito"));

                    }else
                    {
                        pila.Push(izquierdor / derecho);

                        formarOperacion(numeroLlamado + 1, izquierdor, derecho, "/");

                    }
                    

                    //Stoper de division 

                }


            }
            else
            {
                //epsilon
            }

            formarSalida(numeroLlamado, "</segundaprima>");
        }

        private void tercera(int numeroLlamado)
        {
            formarEntrada(numeroLlamado, "<tercera>");

            if (CategoriaGramatical.ENTERO.Equals(componente.obtenerCategoria().ToUpper()))
            {
                pila.Push(Convert.ToInt32(componente.obtenerLexema()));
                pedirComponente();
            }
            else if (CategoriaGramatical.DECIMAL.Equals(componente.obtenerCategoria().ToUpper()))
            {
                pila.Push(Convert.ToDouble(componente.obtenerLexema()));
                pedirComponente();
            }
            else if (CategoriaGramatical.PARENTESIS_ABRE.Equals(componente.obtenerCategoria().ToUpper()))
            {
                pedirComponente();
                primera(numeroLlamado + 1);

                if (CategoriaGramatical.PARENTESIS_CIERRA.Equals(componente.obtenerCategoria().ToUpper()))
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
            formarSalida(numeroLlamado, "</tercera>");
        }
    }
}
