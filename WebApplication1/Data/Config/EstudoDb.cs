using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using WebApplication1.Data.Mapeamento;
using WebApplication1.Models;
using System.IO;
using System.Linq;
namespace WebApplication1.Data.Config
{
    public class EstudoDb : DbContext
    {
        // Liga a Classe pessoa ao nosso banco de dados
        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // especifica para o entity framerwork como deve ser a classe pessoa dentro do banco de dados
            modelBuilder.ApplyConfiguration(new PessoaMaper());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //le nosso arquivo appsettings.json
            var config = new ConfigurationBuilder()
                //diz para o entity framwork a pasta que esta o arquivo (neste caso na raiz do projeto (/))
                .SetBasePath(Directory.GetCurrentDirectory())
                // especifica o nome de arquivo que esta a configuração de conexao com o banco de dados, neste caso connectionstring (appsettings.json)
                .AddJsonFile("appsettings.json")
                // executa o processo para retornar como uma variavel em c#
                .Build();

            // neste ponto é ligado a conexao entre os dados de conexao com o banco dedados e nossa aplicação(api). É especificado que esta dentro da sessão defaultconnection do nosso appsettings.json
            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));

            //continua o processo padrao do entityFramework
            base.OnConfiguring(optionsBuilder);
        }
    }

    
}
