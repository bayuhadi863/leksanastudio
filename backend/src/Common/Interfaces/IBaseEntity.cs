namespace LeksanaStudio.Common.Interfaces
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset? UpdatedDate { get; set; }
        DateTimeOffset? DeletedDate { get; set; }
        string CreatedBy { get; set; }
        string? UpdatedBy { get; set; }
        string? DeletedBy { get; set; }
    }
}
