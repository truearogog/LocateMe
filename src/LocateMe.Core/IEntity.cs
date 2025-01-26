namespace LocateMe.Core;

public interface IEntity
{
    Guid Id { get; set; }
    DateTime Created { get; set; }
    DateTime Updated { get; set; }
    string CreatedBy { get; set; }
    string UpdatedBy { get; set; }
}
