using ActualLab.CommandR;
using ActualLab.Fusion;
using ActualLab.Fusion.Server;
using ActualLab.Rpc;
using System.Security.Claims;

namespace Server;

public static class FusionServices
{
    public static IServiceCollection AddFusionServices(this IServiceCollection services)
    {
        // Fusion services
        var fusion = services.AddFusion(RpcServiceMode.Server, true);
        fusion.AddCustomServices();

        var fusionServer = fusion.AddWebServer();

        /*fusionServer.ConfigureAuthEndpoint(_ => new()
        {
            DefaultSignInScheme = "oidc",
            DefaultSignOutScheme = "oidc",
            SignInPropertiesBuilder = (_, properties) =>
            {
                properties.IsPersistent = true;
            }
        });
        fusionServer.ConfigureServerAuthHelper(_ => new()
        {
            NameClaimKeys = Array.Empty<string>(),
        });*/

        //fusion.AddSandboxedKeyValueStore<FusionDbContext>();
        fusion.AddOperationReprocessor();

        //fusion.AddBlazor().AddAuthentication().AddPresenceReporter();

        /*fusion.AddDbAuthService<FusionDbContext, string>();
        fusion.AddDbKeyValueStore<FusionDbContext>();*/

        return services;
    }
}
/*public class FusionAuthMiddleWare
{
    private readonly RequestDelegate _next;

    public FusionAuthMiddleWare(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ServerAuthHelper serverAuthHelper, IAuth auth, ICommander commander, UserContext userContext)
    {
        userContext.UserClaims = context.User.Claims;
        userContext.Session = serverAuthHelper.Session;
        if (context is { Request.Path.Value: { } } &&
        (context.Request.Path.Value.Contains("api") || context.Request.Path.Value.Contains("rpc")) &&
        context.User?.Identity != null &&
        context.User.Identity.IsAuthenticated)
        {
            var user = await auth.GetUser(serverAuthHelper.Session);
            if (user != null && user.Claims.First(x => x.Key.Equals(ClaimTypes.NameIdentifier)).Value != context.User.Claims.First(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value)
            {
                await commander.Call(new Auth_SignOut(serverAuthHelper.Session));
            }
            var dups = context.User.Claims.ToList();

            foreach (var dupGroup in dups.GroupBy(x => x.Type).Where(x => x.Count() > 1).ToList())
            {
                int i = 0;
                foreach (var item in dupGroup)
                {
                    if (i == 0)
                    {
                        i++;
                        continue;
                    }
                    (context.User.Identity as ClaimsIdentity)?.RemoveClaim(item);
                    i++;
                }
            }
            await serverAuthHelper.UpdateAuthState(context);
        }

        await _next.Invoke(context!);
    }
}*/
