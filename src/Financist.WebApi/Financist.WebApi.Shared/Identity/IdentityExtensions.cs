using System.Security.Claims;
using System.Security.Principal;

namespace Financist.WebApi.Shared.Identity;

public static class IdentityExtensions
{
    public static Guid? GetSubjectId(this IIdentity identity)
    {
        var nameIdentifier = (identity as ClaimsIdentity)?.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?
            .Value;
        
        if (nameIdentifier is null)
            return null;

        return Guid.Parse(nameIdentifier);
    }
}
