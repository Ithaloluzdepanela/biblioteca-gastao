using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace BibliotecaGastao.Models
{
    public class emprestimo

    {
        int id { get; set; }
        int usuarioId { get; set; }
        int livroId { get; set; }
        DateAndTime dataEmprestimo { get; set; }
        DateAndTime dataDevolucao { get; set; }
        DateAndTime dataRealDevolucao { get; set; }
        bool status { get; set; }

    }
}
