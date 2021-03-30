using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Data.Mapeamento
{
    public class PessoaMaper : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            // difine a chave primaria do banco de dados, ou seja o registro que nunca poderá se repetir para pessoa e servirá para identifica-la
            builder.HasKey(pessoa => pessoa.Id);
            // defini que o nome deverá possuir no maximo 100 caracteries
            builder.Property(pessoa => pessoa.Nome).HasMaxLength(100);
            // defini que o nome é obrigatorio
            builder.Property(pessoa => pessoa.Nome).IsRequired();
            // defini que o telefone deverá possuir no maximo 100 caracteries
            builder.Property(pessoa => pessoa.Telefone).HasMaxLength(11);
            // defini que o telefone é obrigatorio
            builder.Property(pessoa => pessoa.Telefone).IsRequired();
        }
    }
}
