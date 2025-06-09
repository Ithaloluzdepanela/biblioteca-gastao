using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaApp
{
    
    public class Usuario
    {
        
        public int Id { get; set; }

        
        public string Nome { get; set; }

        public string Email { get; set; }

        
        public string Senha { get; set; }

        public string CPF { get; set; }


        public DateTime DataNascimento { get; set; }

        public string Turma { get; set; }

        public string Telefone { get; set; }

        public string TipoUsuario { get; set; } // Ex: "Aluno", "Professor", "Bibliotecário", "Administrador"

        // Relacionamento com empréstimos
        public ICollection<Emprestimo> Emprestimos { get; set; }
    }
}


