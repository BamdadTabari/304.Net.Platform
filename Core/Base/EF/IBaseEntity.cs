namespace Core.Base.EF;
public interface IBaseEntity
{
    string? name { get; }
    string slug { get; }
    DateTime created_at { get; }
    DateTime updated_at { get; }
}