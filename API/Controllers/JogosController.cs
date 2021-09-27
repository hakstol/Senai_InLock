using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using senai.inlock.webApi_.Domains;
using senai.inlock.webApi_.Interfaces;
using senai.inlock.webApi_.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.inlock.webApi_.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    
    public class JogosController : ControllerBase
    {
        private IJogoRepository _jogoRepository { get; set; }

        public JogosController()
        {
            _jogoRepository = new JogoRepository();
        }

        /// <summary>
        /// Listar todos os jogos
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "1, 2")]
 
        [HttpGet]
        public IActionResult Get()
        {
            List<JogoDomain> listaJogos = _jogoRepository.ListarTodos();
            return Ok(listaJogos);
        }

        /// <summary>
        /// Localizar jogo por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            JogoDomain jogoBuscado = _jogoRepository.BuscarPorId(id);

            if (jogoBuscado == null)
            {
                return NotFound("Nenhum jogo encontrado!");
            }

            return Ok(jogoBuscado);
        }
        /// <summary>
        /// Cadastrar novo jogo
        /// </summary>
        /// <param name="novoJogo"></param>
        /// <returns></returns>
        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult Post(JogoDomain novoJogo)
        {
            _jogoRepository.Cadastrar(novoJogo);

            return StatusCode(201);
        }
        /// <summary>
        /// Atualizar jogo pela URL
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jogoAtualizado"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult PutIdUrl(int id, JogoDomain jogoAtualizado)
        {
            JogoDomain jogoBuscado = _jogoRepository.BuscarPorId(id);

            if (jogoBuscado == null)
            {
                return NotFound(
                        new
                        {
                            mensagem = "Jogo não encontrado!",
                            erro = true
                        }
                    );
            }

            try
            {
                _jogoRepository.AtualizarIdUrl(id, jogoAtualizado);

                return NoContent();
            }
            catch (Exception erro)
            {
                return BadRequest(erro);
            }
        }

        /// <summary>
        /// Atualizar jogo pelo body
        /// </summary>
        /// <param name="jogoAtualizado"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult PutIdBody(JogoDomain jogoAtualizado)
        {
            if (jogoAtualizado.nomeJogo == null ||jogoAtualizado.idJogo <= 0)
            {
                return BadRequest(
                    new
                    {
                        mensagemErro = "O nome ou o ID do jogo não foi informado!"
                    });
            }

            JogoDomain jogoBuscado = _jogoRepository.BuscarPorId(jogoAtualizado.idJogo);

            if (jogoBuscado != null)
            {
                try
                {
                    _jogoRepository.AtualizarIdCorpo(jogoAtualizado);

                    return NoContent();
                }
                catch (Exception codErro)
                {
                    return BadRequest(codErro);
                }
            }

            return NotFound(
                    new
                    {
                        mensagem = "Jogo não encontrado!",
                        errorStatus = true
                    }
                );
        }

        /// <summary>
        /// Deletar jogo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("excluir/{id}")]
        public IActionResult Delete(int id)
        {
            _jogoRepository.Deletar(id);

            return NoContent();
        }
    }
}
