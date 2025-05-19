using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGastao
{
    public class livro

    {
        int id;
        string nome { get; set; }
        string autor { get; set; }
        string genero { get; set; }
        int quantidade { get; set; }
        string codigoBarras { get; set; }
        bool disponibilidade { get; set; }
    }
}
