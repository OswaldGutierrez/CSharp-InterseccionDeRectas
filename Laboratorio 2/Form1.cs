using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;

namespace Laboratorio_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            radioButton1.Checked = true;
        }

        // Esta funci�n se ejecutar� cuando demos click en el bot�n 'Calcular' de la interfaz.
        private void buttonCalcular_Click(object sender, EventArgs e)
        {
            // Obtener las ecuaciones ingresadas en los TextBox
            string ecuacion1 = textBoxFuncion1.Text;
            string ecuacion2 = textBoxFuncion2.Text;

            // Llamar a la funci�n AnalizarRectasDesdeEcuaciones con las ecuaciones ingresadas
            string resultado = analizarRectasPendienteIntercepto(ecuacion1, ecuacion2);

            // Mostrar el resultado en un MessageBox
            MessageBox.Show(resultado);
        }

        private string analizarRectasPendienteIntercepto(string ecuacion1, string ecuacion2)
        {
            /**
             * Expresi�n regular para extraer la pendiente y el t�rmino independiente de una ecuaci�n en el formato "y = mx + b",
             * permitiendo n�meros decimales en la pendiente y el t�rmino independiente
            **/

            string patron = @"y\s*=\s*(-?\d+(\,\d+)?)x\s*([\+\-]\s*\d+(\,\d+)?)?$";


            // Almacenamos el patr� espec�fico en las variables match1 y match2
            Match match1 = Regex.Match(ecuacion1, patron);
            Match match2 = Regex.Match(ecuacion2, patron);

            if (match1.Success && match2.Success)
            {

                /** Obtener las pendientes y t�rminos independientes de las ecuaciones
                 * m1 = Pendiente de la primera ecuaci�n
                 * m2 = Pendiente de la segunda ecuaci�n
                 * b1 = T�rmino independiente de la primera ecuaci�n
                 * b2 = T�rmino independiente de la segunda ecuaci�n
                **/

                double m1 = double.Parse(match1.Groups[1].Value);
                double b1 = match1.Groups[3].Success ? double.Parse(match1.Groups[3].Value) : 0.0;

                double m2 = double.Parse(match2.Groups[1].Value);
                double b2 = match2.Groups[3].Success ? double.Parse(match2.Groups[3].Value) : 0.0;


                // Calcular el punto de intersecci�n (si las rectas se cruzan)
                if (Math.Abs(m1 - m2) > double.Epsilon)
                {
                    double xInterseccion = (b2 - b1) / (m1 - m2);
                    double yInterseccion = m1 * xInterseccion + b1;

                    // Verificar si son perpendiculares
                    if (Math.Abs(m1 * m2 + 1.0) < double.Epsilon)
                    {
                        return $"Las rectas son perpendiculares y se cruzan en el punto ({xInterseccion}, {yInterseccion}).";
                    }

                    else
                    {
                        return $"Las rectas se cruzan en el punto ({xInterseccion}, {yInterseccion}).";
                    }
                }

                else
                {
                    return "Las rectas son paralelas";
                }
            }

            else
            {
                return "Formato de ecuaci�n no v�lido. Use el formato 'y = mx + b'.";
            }
        }


        // Detalles est�ticos de la Interfaz
        private void buttonCalcular_MouseHover(object sender, EventArgs e)
        {
            buttonCalcular.BackColor = Color.CadetBlue;
        }

        private void buttonCalcular_MouseLeave(object sender, EventArgs e)
        {
            buttonCalcular.BackColor = Color.Transparent;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Habilitamos o inhabilitamos qu� panel est� activado.
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                panel1.Enabled = true;
                panel2.Enabled = false;
            }
            else
            {
                panel1.Enabled = false;
                panel2.Enabled = true;
            }
        }




        private void buttonCalcular2_Click(object sender, EventArgs e)
        {
            // Obtener las ecuaciones ingresadas en los cuadros de texto
            string ecuacion1 = textBoxFuncion3.Text;
            string ecuacion2 = textBoxFuncion4.Text;

            // Analizar las ecuaciones para extraer los valores de y1, m, x1
            double y1_1, m1_1, x1_1, y1_2, m1_2, x1_2;

            if (parseEcuacion(ecuacion1, out y1_1, out m1_1, out x1_1) &&
                parseEcuacion(ecuacion2, out y1_2, out m1_2, out x1_2))
            {
                // Calcular el valor de x donde las dos ecuaciones se intersectan
                if (m1_1 != m1_2)
                {
                    double x = (y1_2 - y1_1) / (m1_1 - m1_2);
                    double y = m1_1 * (x - x1_1) + y1_1;

                    // Mostrar el resultado en otro cuadro de texto o en una etiqueta
                    // Por ejemplo, si tienes un cuadro de texto llamado textBoxResultado:
                    MessageBox.Show ($"Punto de intersecci�n: ({x}, {y})");
                }
                else
                {
                    MessageBox.Show("Las ecuaciones son paralelas y no se intersectan.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingresa ecuaciones v�lidas en los cuadros de texto.");
            }
        }

        private bool parseEcuacion(string ecuacion, out double y1, out double m, out double x1)
        {
            y1 = m = x1 = 0;

            try
            {
                // Dividir la ecuaci�n en partes usando los operadores '+' y '='
                string[] partes = ecuacion.Split(new[] { '=', '+' }, StringSplitOptions.RemoveEmptyEntries);

                if (partes.Length == 2)
                {
                    // Analizar la parte izquierda de la ecuaci�n
                    string[] izquierda = partes[0].Split(new[] { '-', 'y', '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (izquierda.Length == 2)
                    {
                        // Extraer los valores de y1 y m
                        y1 = double.Parse(izquierda[0]);
                        m = double.Parse(izquierda[1]);
                    }

                    // Analizar la parte derecha de la ecuaci�n
                    string[] derecha = partes[1].Split(new[] { '(', '-', 'x', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    if (derecha.Length == 2)
                    {
                        // Extraer el valor de x1
                        x1 = double.Parse(derecha[1]);
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                // Si hay un error al analizar la ecuaci�n, se considera inv�lida.
            }

            return false;
        }
    }
}