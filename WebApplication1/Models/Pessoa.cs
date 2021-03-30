using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Pessoa
    {
        
        // para o entityframework funcionar
        protected Pessoa()
        {

        }
        public Pessoa(Guid id, string nome, string telefone)
        {
            Id = id;
            Nome = nome;
            Telefone = telefone;
        }

        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Telefone { get; private set; }

        public void EditarPessoa(string nome, string telefone)
        {
            Nome = nome;
            Telefone = telefone;
        }
    }
}
