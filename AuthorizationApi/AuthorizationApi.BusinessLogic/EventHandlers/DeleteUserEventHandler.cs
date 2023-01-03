using AuthorizationApi.Persistence;
using Core.Events;
using Core.Models;
using Core.Models.Errors;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationApi.BusinessLogic.EventHandlers;

public class DeleteUserEventHandler : IConsumer<DeleteUserEvent>
{
    private readonly DatabaseContext _databaseContext;

    public DeleteUserEventHandler(DatabaseContext context)
    {
        _databaseContext = context;
    }

    public async Task Consume(ConsumeContext<DeleteUserEvent> context)
    {
        var user = await _databaseContext.Users
            .FirstOrDefaultAsync(u => u.Id == context.Message.UserId);

        if (user == null)
        {
            await context.RespondAsync(new Result<bool>(new DbEntityNotFoundError("User not found")));
            return;
        }

        _databaseContext.Users.Remove(user);
        await _databaseContext.SaveChangesAsync();

        await context.RespondAsync(new Result<bool>(true));
    }
}