namespace Shared;

[DataContract, MemoryPackable]
public partial record CreateTodoCommand([property: DataMember] Session Session, [property: DataMember] TodoView Entity) : ISessionCommand<TodoView>;

[DataContract, MemoryPackable]
public partial record UpdateTodoCommand([property: DataMember] Session Session, [property: DataMember] TodoView Entity) : ISessionCommand<TodoView>;

[DataContract, MemoryPackable]
public partial record DeleteTodoCommand([property: DataMember] Session Session, [property: DataMember] string Id) : ISessionCommand<TodoView>;