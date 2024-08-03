namespace Services;

public static class TodoMapper
{
    public static List<TodoView> MapToViewList(this List<Todo> todos)
        => todos.Select(MapToView).ToList();

    public static TodoView MapToView(this Todo todo)
        => new()
        {
            Id = todo.Id,
            Name = todo.Name
        };

    public static void From(this Todo todo, TodoView view)
    {
        todo.Id = todo.Id;
        todo.Name = todo.Name;
    }
}