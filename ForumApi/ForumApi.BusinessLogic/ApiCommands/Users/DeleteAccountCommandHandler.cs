using Core.Events;
using Core.Models;
using Core.Models.Errors;
using ForumApi.BusinessLogic.DTOs;
using ForumApi.Persistence;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.BusinessLogic.ApiCommands.Users;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Result<UserEntityDto>>
{
    private readonly DatabaseContext _context;
    private readonly IRequestClient<DeleteUserEvent> _deleteUserClient;

    public DeleteAccountCommandHandler(DatabaseContext context, IRequestClient<DeleteUserEvent> deleteUserClient)
    {
        _context = context;
        _deleteUserClient = deleteUserClient;
    }

    public async Task<Result<UserEntityDto>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var deleteUserEvent = new DeleteUserEvent(
            UserId: request.RequesterId);

        var deleteUserEventResult = await _deleteUserClient.GetResponse<Result<bool>>(deleteUserEvent, cancellationToken);

        if (deleteUserEventResult.Message.IsSuccess == false)
        {
            return new Result<UserEntityDto>(new DbEntityNotFoundError("User not found"));
        }

        var requester = await _context.Users
            .FirstAsync(u => u.Id == request.RequesterId, cancellationToken);

        _context.Users.Remove(requester);
        await _context.SaveChangesAsync(cancellationToken);

        var userDto = new UserEntityDto
        {
            Id = requester.Id,
            Nickname = requester.Nickname
        };

        return new Result<UserEntityDto>(userDto);
    }
}