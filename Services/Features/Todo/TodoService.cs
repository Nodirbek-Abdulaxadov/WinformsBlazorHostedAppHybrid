namespace Services;

public class TodoService(AppDbContext dbContext) : ITodoService
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<TodoView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        var todo = dbContext.Todos.ToList();

        var result = SortSearchPaginate(todo, options);

        var count = todo.Count();
        return new TableResponse<TodoView>() { Items = result.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<TodoView> Get(string Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        var todo = await dbContext.Todos.FirstOrDefaultAsync(x => x.Id == Id);

        return todo == null ? throw new NotFoundException("Todo Not Found") : todo.MapToView();
    }

    #endregion
    #region Mutations
    public async virtual Task Create(CreateTodoCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        Todo todo = new();
        todo.From(command.Entity);

        if (dbContext.Todos.Any(x => x.Name == todo.Name))
        {
            throw new BadRequestException($"Todo already exists!");
        }

        await dbContext.Todos.AddAsync(todo);
    }


    public async virtual Task Delete(DeleteTodoCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        var todo = await dbContext.Todos.FirstOrDefaultAsync(x => x.Id == command.Id);
        if (todo == null) throw new NotFoundException("Todo Not Found");
        await dbContext.Todos.DeleteAsync(todo.Id);
    }


    public async virtual Task Update(UpdateTodoCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        var todo = await dbContext.Todos.FirstOrDefaultAsync(x => x.Id == command.Entity.Id);
        if (todo == null) throw new NotFoundException("Todo Not Found");

        todo.From(command.Entity);

        if (dbContext.Todos.Any(x => x.Name == todo.Name && x.Id != todo.Id))
        {
            throw new BadRequestException($"Todo already exists!");
        }

        await dbContext.Todos.UpdateAsync(todo);
    }
    #endregion

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    private List<Todo> SortSearchPaginate(List<Todo> todo, TableOptions options)
    {
        var list = (options.Search is not null ?
                   todo.Where(s => s.Name.Contains(options.Search)) :
                   todo).OrderByDescending(x => x.CreatedAt);

        return
        (
            options.SortLabel switch
            {
                "Name" => list.OrderBy(o => o.Name),
                "Id" => list.OrderBy(o => o.Id),
                _ => list.OrderBy(o => o.Id),
            }
        ).Skip((options.Page > 0 ? options.Page - 1 : 0) * options.PageSize)
         .Take(options.PageSize)
         .ToList();
    }
    #endregion
}