using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace _01SampleAuth.Helpers
{
    public class EmailDomainHandler: AuthorizationHandler<EmailDomainRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailDomainRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Email))
                return Task.CompletedTask;
            var emailAddress = context.User.FindFirst(c => c.Type == ClaimTypes.Email).Value;
            if (emailAddress.EndsWith(requirement.EmailDomain))
                context.Succeed(requirement);
            return Task.CompletedTask;
        }

    }
}
