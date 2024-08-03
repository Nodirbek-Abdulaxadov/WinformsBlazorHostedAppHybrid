﻿namespace Shared;

[DataContract, MemoryPackable]
public sealed partial record TableOptions
{
    [property: DataMember] public int Page { get; set; } = 1;

    [property: DataMember] public int PageSize { get; set; } = 15;

    [property: DataMember] public string? SortLabel { get; set; }

    [property: DataMember] public int SortDirection { get; set; } = 1;

    [property: DataMember] public string? Search { get; set; }

    [property: DataMember] public DateOnly? From { get; set; }

    [property: DataMember] public DateOnly? To { get; set; }

    [property: DataMember] public Session? Session { get; set; }
}