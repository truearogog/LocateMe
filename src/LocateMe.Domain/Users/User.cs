using LocateMe.Core;
using Microsoft.AspNetCore.Identity;

namespace LocateMe.Domain.Users;

public sealed class User : IdentityUser<Guid>, IEntity
{
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
}
