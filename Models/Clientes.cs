using System;

namespace ClienteAtv.Models
{
    public class Cliente
    {
        public string Nome { get; set; }
        public DateTime DataNasc { get; set; }
        public string Sexo { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public object DataNascimento { get; internal set; }
    }
}
