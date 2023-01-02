using Core.Models;
using ForumApi.BusinessLogic.DTOs;
using ForumApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.BusinessLogic.ApiQueries.Users;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserEntityDto>>
{
    private readonly DatabaseContext _context;

    public GetUserQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<UserEntityDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        var userDto = new UserEntityDto
        {
            Id = user.Id,
            Nickname = user.Nickname
        };

        return new Result<UserEntityDto>(userDto);
    }
}