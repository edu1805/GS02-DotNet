using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WellWork.Application;
using WellWork.Application.DTOs;
using WellWork.Domain.Enums;

namespace WellWork.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[SwaggerTag("Controller responsável por gerenciar CheckIns.")]
[ApiVersion("1.0")]
public class CheckInsController : ControllerBase
{
    private readonly ICheckInService _checkInService;
    private readonly IGeneratedMessageService _messageService;
    private readonly IMapper _mapper;

    public CheckInsController(
        ICheckInService checkInService,
        IGeneratedMessageService messageService,
        IMapper mapper)
    {
        _checkInService = checkInService;
        _messageService = messageService;
        _mapper = mapper;
    }

    // --------------------------------------------------------------------
    // POST /checkins
    // --------------------------------------------------------------------
    /// <summary>
    /// Cria um novo Check-In para um usuário.
    /// </summary>
    /// <remarks>
    /// ## 🔢 Valores possíveis
    ///
    /// ### 🎭 Mood (Humor)
    /// * 0 = Feliz  
    /// * 1 = Neutro  
    /// * 2 = Cansado  
    /// * 3 = Estressado  
    /// * 4 = Ansioso  
    /// * 5 = Triste  
    ///
    /// ### ⚡ EnergyLevel (Energia)
    /// * 0 = Baixa  
    /// * 1 = Média  
    /// * 2 = Alta  
    ///
    /// ## 📥 Exemplo de requisição (JSON)
    /// ```json
    /// {
    ///   "userId": "c2b1f9f1-28e3-4a79-a3c7-8d8c99ef1234",
    ///   "mood": 0,
    ///   "energy": 1,
    ///   "notes": "Dia tranquilo, porém um pouco cansado."
    /// }
    /// ```
    /// </remarks>
    [HttpPost]
    [SwaggerResponse(201, "CheckIn criado com sucesso", typeof(CheckInResponseDto))]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> Create(CheckInCreateDto dto)
    {
        var checkIn = await _checkInService.CreateCheckInAsync(
            dto.UserId,
            dto.Mood,
            dto.Energy,
            dto.Notes
        );

        return CreatedAtAction(nameof(GetById), new { id = checkIn.Id },
            _mapper.Map<CheckInResponseDto>(checkIn));
    }
    
    /// <summary>
    /// Retorna um CheckIn específico pelo ID.
    /// </summary>
    /// <param name="id">ID do CheckIn desejado.</param>
    /// <returns>O CheckIn encontrado ou 404 se não existir.</returns>
    [HttpGet("{id:guid}")]
    [SwaggerResponse(200, "CheckIn localizado", typeof(CheckInResponseDto))]
    [SwaggerResponse(404, "CheckIn não encontrado")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var checkIn = await _checkInService.GetByIdAsync(id);
        return checkIn == null ? NotFound() : Ok(_mapper.Map<CheckInResponseDto>(checkIn));
    }
    
    /// <summary>
    /// Retorna todos os CheckIns de um usuário, com paginação opcional.
    /// </summary>
    /// <param name="userId">ID do usuário.</param>
    /// <param name="page">Número da página.</param>
    /// <param name="pageSize">Quantidade de itens por página.</param>
    /// <returns>Lista paginada de CheckIns.</returns>
    [HttpGet("user/{userId:guid}")]
    [SwaggerResponse(200, "Lista de CheckIns retornada com sucesso")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> GetByUserId(Guid userId, int page = 1, int pageSize = 10)
    {
        var (items, total) = await _checkInService.GetByUserIdAsync(userId, page, pageSize);

        return Ok(new
        {
            TotalCount = total,
            Items = items.Select(_mapper.Map<CheckInResponseDto>)
        });
    }

    /// <summary>
    /// Atualiza o humor registrado em um CheckIn.
    /// </summary>
    /// <param name="id">ID do CheckIn.</param>
    /// <param name="mood">Novo humor.</param>
    /// <returns>204 se atualizado com sucesso.</returns>
    [HttpPut("{id:guid}/mood")]
    [SwaggerResponse(204, "Humor atualizado com sucesso")]
    [SwaggerResponse(404, "CheckIn não encontrado")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> UpdateMood(Guid id, [FromBody] Mood mood)
    {
        await _checkInService.UpdateMoodAsync(id, mood);
        return NoContent();
    }

    /// <summary>
    /// Atualiza o nível de energia registrado em um CheckIn.
    /// </summary>
    /// <param name="id">ID do CheckIn.</param>
    /// <param name="energy">Novo nível de energia.</param>
    /// <returns>204 se atualizado com sucesso.</returns>
    [HttpPut("{id:guid}/energy")]
    [SwaggerResponse(204, "Nível de energia atualizado com sucesso")]
    [SwaggerResponse(404, "CheckIn não encontrado")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> UpdateEnergy(Guid id, [FromBody] EnergyLevel energy)
    {
        await _checkInService.UpdateEnergyAsync(id, energy);
        return NoContent();
    }

    /// <summary>
    /// Atualiza as anotações de um CheckIn.
    /// </summary>
    /// <param name="id">ID do CheckIn.</param>
    /// <param name="dto">Objeto contendo as novas anotações.</param>
    /// <returns>204 se atualizado com sucesso.</returns>
    [HttpPut("{id:guid}/notes")]
    [SwaggerResponse(204, "Anotações atualizadas com sucesso")]
    [SwaggerResponse(404, "CheckIn não encontrado")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> UpdateNotes(Guid id, CheckInUpdateNotesDto dto)
    {
        await _checkInService.UpdateNotesAsync(id, dto.Notes);
        return NoContent();
    }

    /// <summary>
    /// Gera uma mensagem personalizada com IA baseada nos dados do CheckIn.
    /// </summary>
    /// <param name="id">ID do CheckIn.</param>
    /// <returns>Mensagem gerada com IA.</returns>
    [HttpPost("{id:guid}/generate-message")]
    [SwaggerResponse(200, "Mensagem gerada com sucesso", typeof(GeneratedMessageResponseDto))]
    [SwaggerResponse(404, "CheckIn não encontrado")]
    [SwaggerResponse(500, "Erro ao gerar mensagem com IA")]
    public async Task<IActionResult> GenerateMessage(Guid id)
    {
        var result = await _checkInService.GenerateLLMMessageAsync(id);
        return Ok(_mapper.Map<GeneratedMessageResponseDto>(result));
    }

    /// <summary>
    /// Remove um CheckIn pelo ID.
    /// </summary>
    /// <param name="id">ID do CheckIn.</param>
    /// <returns>204 se removido com sucesso.</returns>
    [HttpDelete("{id:guid}")]
    [SwaggerResponse(204, "CheckIn removido com sucesso")]
    [SwaggerResponse(404, "CheckIn não encontrado")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _checkInService.DeleteAsync(id);
        return NoContent();
    }
}