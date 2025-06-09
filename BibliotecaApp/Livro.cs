using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaApp
{
    public class Livros
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Autor { get; set; }
        public int Quantidade { get; set; }
        public string CodigoBarras { get; set; }
        public bool Disponibilidade { get; set; }
        public string Genero { get; set; } // Ex: "Ficção", "Não-ficção", "Fantasia", etc.
        public ICollection<Emprestimo> Emprestimo { get; set; }
    }
    
    
}
