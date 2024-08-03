namespace Services;

public class AppDbContext(MongoDbCoreOptions options) 
    : MongoDbContext(options)
{
    public Collection<Todo> Todos { get; set; } = null!;

    protected override void OnInitialized()
    {
        if (!Todos.Any())
        {
            Todos.AddRange([
                new Todo() { Name = "Salom"},
                new Todo() { Name = "asahd"}
                ]);
        }

        base.OnInitialized();
    }
}