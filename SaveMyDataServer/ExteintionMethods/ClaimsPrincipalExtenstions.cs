using System.Security.Claims;

namespace SaveMyDataServer.ExteintionMethods
{
    /// <summary>
    /// Some extenstion methods for the <see cref="ClaimsPrincipal"/>
    /// </summary>
    public static class ClaimsPrincipalExtenstions
    {
        public static Claim GetClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            //Get the claims list
            var claims = claimsPrincipal.Identity as ClaimsIdentity;
            //Get the claim from the list
            return claims.FindFirst(claimType);
        }
    }
}
