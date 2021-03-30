using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data.Config;
using WebApplication1.Models;
using WebApplication1.ViewModel;
using WebApplication1.ViewModel.Pessoas;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        // variavel utilizada  para acesar o banco de dados
        private readonly EstudoDb _db;

        // injeta nossa variavel db por injeção de dependencia (configurado na nossa startup)
        public PessoaController(EstudoDb db)
        {
            _db = db;
        }

        // metodo get = PEGAR: utilizado sempre que é necessario consultar algo na api
        [HttpGet]
        public IActionResult BuscarTodos()
        {
            //pesquiso todas as pessoas no banco de dados
            var pessoas = _db.Pessoas.ToList();

            //crio uma lista de view model que representará os dados das pessoas que podem ser disponibilizados na API
            var pessoasViewModel = new List<PessoaViewModel>();

            //converte as pessoas do banco de dados em pessoaviewmodel
            foreach (var pessoa in pessoas)
            {
                var pessoaViewModel = new PessoaViewModel()
                {
                    Id = pessoa.Id,
                    Nome = pessoa.Nome
                };
                pessoasViewModel.Add(pessoaViewModel);
            }

            //assim que todas as pessoas estiverem convertidas em view model, elas serão retornadas com sucesso
            return Ok(pessoasViewModel);

        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(string id)
        {
            var guidId = new Guid(id);
            var pessoa = _db.Pessoas.Where(pessoaDb => pessoaDb.Id == guidId).FirstOrDefault();

            var pessoaRegistrar = new PessoaRegistrarViewModel()
            {
                Nome = pessoa.Nome,
                Telefone = pessoa.Telefone
            };

            return Ok(pessoa);
        }
        [HttpPost]
        public IActionResult Registrar(PessoaRegistrarViewModel pessoaRegistrar)
        {
            //cria uma lista que armazenará os erros
            var erros = new List<string>();
            var novaPessoa = new Pessoa(Guid.NewGuid(), pessoaRegistrar.Nome, pessoaRegistrar.Telefone);

            if (novaPessoa.Telefone.Length > 11)
                erros.Add("O numero de telefone possui mais de 11 caracteries");
            if (novaPessoa.Nome.Length > 100)
                erros.Add("O nome possui mais de 100 caracteries");

            if (string.IsNullOrEmpty(novaPessoa.Nome))
                erros.Add("O nome é obrigatório");

            if (string.IsNullOrEmpty(novaPessoa.Telefone))
                erros.Add("O telefone é obrigatório");

            if (erros.Count > 0)
                return BadRequest(new { erros = erros });

            _db.Add(novaPessoa);
            _db.SaveChanges();
            return Ok(novaPessoa.Id);
        }

        [HttpPut("{id}")]
        public IActionResult Editar(string id, PessoaRegistrarViewModel pessoaEditarViewModel)
        {
            var erros = new List<string>();

            if (pessoaEditarViewModel.Telefone.Length > 11)
                erros.Add("O numero de telefone possui mais de 11 caracteries");
            if (pessoaEditarViewModel.Nome.Length > 100)
                erros.Add("O nome possui mais de 100 caracteries");

            if (string.IsNullOrEmpty(pessoaEditarViewModel.Nome))
                erros.Add("O nome é obrigatório");

            if (string.IsNullOrEmpty(pessoaEditarViewModel.Telefone))
                erros.Add("O telefone é obrigatório");

            if (erros.Count > 0)
                return BadRequest(new { erros = erros });

            var guidId = new Guid(id);
            var pessoaEditarDb = _db.Pessoas.Where(pessoa => pessoa.Id == guidId).FirstOrDefault();

            if (pessoaEditarDb == null)
            {
                erros.Add("O ID solicitado não foi encontrado no banco de dados");
                return BadRequest(new { erros = erros });
            }

            pessoaEditarDb.EditarPessoa(pessoaEditarViewModel.Nome, pessoaEditarViewModel.Telefone);
            _db.Update(pessoaEditarDb);
            _db.SaveChanges();
            return Ok();
        }

    }
}