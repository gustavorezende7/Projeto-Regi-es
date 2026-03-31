using Microsoft.AspNetCore.Mvc;
using RegioesApi.DTOs;
using RegioesApi.Models;
using RegioesApi.Repositories;

namespace RegioesApi.Controllers
{
    [ApiController]
    [Route("regioes")]
    public class RegioesController : ControllerBase
    {
        private readonly IRegiaoRepository _repo;

        public RegioesController(IRegiaoRepository repo)
        {
            _repo = repo;
        }

        // GET /regioes?incluirInativas=true
        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] bool incluirInativas = false)
        {
            var regioes = await _repo.ListarAsync(incluirInativas);
            return Ok(regioes.Select(r => new RegiaoReadDto
            {
                Id = r.Id,
                UF = r.UF,
                Nome = r.Nome,
                Ativo = r.Ativo,
                CreatedAt = r.CreatedAt
            }));
        }

        // GET /regioes/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var regiao = await _repo.BuscarPorIdAsync(id);
            if (regiao == null) return NotFound("Região não encontrada.");
            return Ok(new RegiaoReadDto
            {
                Id = regiao.Id,
                UF = regiao.UF,
                Nome = regiao.Nome,
                Ativo = regiao.Ativo,
                CreatedAt = regiao.CreatedAt
            });
        }

        // POST /regioes
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] RegiaoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _repo.ExisteComMesmoNomeAsync(dto.UF, dto.Nome))
                return Conflict("Já existe uma região com o mesmo nome e UF.");

            var nova = new Regiao { UF = dto.UF.ToUpper().Trim(), Nome = dto.Nome.Trim(), Ativo = true };
            await _repo.CriarAsync(nova);
            return CreatedAtAction(nameof(BuscarPorId), new { id = nova.Id }, new {
                id = nova.Id,
                uf = nova.UF,
                nome = nova.Nome,
                ativo = nova.Ativo,
                createdAt = nova.CreatedAt
            });
        }

        // PUT /regioes/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] RegiaoUpdateDto dto)
        {
            var regiao = await _repo.BuscarPorIdAsync(id);
            if (regiao == null) return NotFound("Região não encontrada.");

            if (await _repo.ExisteComMesmoNomeAsync(dto.UF, dto.Nome, id))
                return Conflict("Outra região já possui este nome e UF.");

            regiao.UF = dto.UF.ToUpper().Trim();
            regiao.Nome = dto.Nome.Trim();
            regiao.Ativo = dto.Ativo;

            await _repo.AtualizarAsync(regiao);
            return NoContent();
        }

        // PATCH /regioes/{id}?ativo=false
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> AlterarStatus(int id, [FromQuery] bool ativo)
        {
            var regiao = await _repo.BuscarPorIdAsync(id);
            if (regiao == null) return NotFound("Região não encontrada.");

            regiao.Ativo = ativo;
            await _repo.AtualizarAsync(regiao);
            return NoContent();
        }
    }
}
