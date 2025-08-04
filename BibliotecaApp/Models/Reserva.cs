using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaApp.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int LivroId { get; set; }
        public DateTime DataReserva { get; set; }
        public DateTime? DataNotificacao { get; set; }
        public bool Notificado { get; set; }
        public string Status { get; set; } // "Ativa", "Cancelada", "Concluída"
    }
}
