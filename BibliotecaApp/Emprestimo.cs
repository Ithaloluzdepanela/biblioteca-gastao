using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaApp
{
    internal class Emprestimos
    {
        public int Id { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }
        public bool DataRealDevolucao { get; set; }
        // Relacionamento com usuário
        public Usuario Usuario { get; set; }
        public int UsuarioId { get; set; } // Chave estrangeira
        // Relacionamento com livro
        public Livros Livro { get; set; }
        public int LivroId { get; set; } // Chave estrangeira
    }
}
