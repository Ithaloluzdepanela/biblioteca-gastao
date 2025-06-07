using System;

public class Emprestimo
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int LivroId { get; set; }
    public string BibliotecariaResponsavel { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public DateTime? DataRealDevolucao { get; set; }
    public string Status { get; set; }
}