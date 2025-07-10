using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaApp
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LivrosForm());
            //Repetição para quando clicar no logout reiniciar a aplicação
            while (true)
            {
                using (LoginForm login = new LoginForm())
                {
                    if (login.ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new MainForm());
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
