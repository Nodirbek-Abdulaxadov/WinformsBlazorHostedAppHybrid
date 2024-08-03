using ActualLab.Fusion;
using Services;
using Shared;

namespace Server;

public static class FusionServerExtension
{
    public static FusionBuilder AddCustomServices(this FusionBuilder fusion)
    {
        fusion.AddService<ITodoService, TodoService>();
        return fusion;
    }
}