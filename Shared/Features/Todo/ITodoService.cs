namespace Shared;

public interface ITodoService : IComputeService
{
    [ComputeMethod]
    Task<TableResponse<TodoView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    [ComputeMethod]
    Task<TodoView> Get(string Id, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateTodoCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateTodoCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteTodoCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}