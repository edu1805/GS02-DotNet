using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WellWork.Application;
using WellWork.Application.DTOs;

namespace WellWork.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
[SwaggerTag("Controller responsável por gerenciar usuários.")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    /// <summary>
    /// Cria um novo usuário no sistema.
    /// </summary>
    /// <param name="dto">Dados para criação do usuário.</param>
    /// <returns>O usuário criado.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Cria um novo usuário.")]
    [SwaggerResponse(201, "Usuário criado com sucesso.", typeof(UserResponseDto))]
    [SwaggerResponse(400, "Dados inválidos fornecidos.")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> Create(UserCreateDto dto)
    {
        var user = await _userService.CreateUserAsync(dto.Username, dto.PasswordHash, "ROLE_USER");
        var result = _mapper.Map<UserResponseDto>(user);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Busca um usuário pelo seu ID.
    /// </summary>
    /// <param name="id">ID do usuário.</param>
    /// <returns>O usuário encontrado ou 404 caso não exista.</returns>
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Busca um usuário pelo ID.")]
    [SwaggerResponse(200, "Usuário encontrado.", typeof(UserResponseDto))]
    [SwaggerResponse(404, "Usuário não encontrado.")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return user == null ? NotFound() : Ok(_mapper.Map<UserResponseDto>(user));
    }

    /// <summary>
    /// Busca um usuário pelo username.
    /// </summary>
    /// <param name="username">Nome de usuário.</param>
    /// <returns>O usuário correspondente ou 404 caso não exista.</returns>
    [HttpGet("by-username/{username}")]
    [SwaggerOperation(Summary = "Busca um usuário pelo nome de usuário.")]
    [SwaggerResponse(200, "Usuário encontrado.", typeof(UserResponseDto))]
    [SwaggerResponse(404, "Usuário não encontrado.")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var user = await _userService.GetByUsernameAsync(username);
        return user == null ? NotFound() : Ok(_mapper.Map<UserResponseDto>(user));
    }

    /// <summary>
    /// Atualiza a senha de um usuário.
    /// </summary>
    /// <param name="id">ID do usuário a ser atualizado.</param>
    /// <param name="dto">Nova senha.</param>
    /// <returns>Status 204 caso atualizado com sucesso.</returns>
    [HttpPut("{id:guid}/password")]
    [SwaggerOperation(Summary = "Atualiza a senha de um usuário.")]
    [SwaggerResponse(204, "Senha atualizada com sucesso.")]
    [SwaggerResponse(404, "Usuário não encontrado.")]
    [SwaggerResponse(400, "Dados inválidos.")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> UpdatePassword(Guid id, UserUpdatePasswordDto dto)
    {
        await _userService.UpdatePasswordAsync(id, dto.NewPassword);
        return NoContent();
    }

    /// <summary>
    /// Lista usuários de forma paginada.
    /// </summary>
    /// <param name="page">Número da página.</param>
    /// <param name="pageSize">Quantidade por página.</param>
    /// <returns>Lista paginada de usuários.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Lista usuários de forma paginada.")]
    [SwaggerResponse(200, "Lista retornada com sucesso.")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var (items, total) = await _userService.GetPagedAsync(page, pageSize);

        return Ok(new
        {
            TotalCount = total,
            Items = items.Select(_mapper.Map<UserResponseDto>)
        });
    }

    /// <summary>
    /// Remove um usuário pelo ID.
    /// </summary>
    /// <param name="id">ID do usuário a ser removido.</param>
    /// <returns>Status 204 se removido com sucesso.</returns>
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Remove um usuário pelo ID.")]
    [SwaggerResponse(204, "Usuário removido com sucesso.")]
    [SwaggerResponse(404, "Usuário não encontrado.")]
    [SwaggerResponse(500, "Erro interno do servidor")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}