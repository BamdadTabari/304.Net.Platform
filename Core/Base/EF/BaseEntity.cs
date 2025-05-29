using System.ComponentModel.DataAnnotations;

namespace Core.Base.EF;
public class BaseEntity : IBaseEntity
{
    public long id { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string? name { get; set; }

    [MaxLength(1000)]
    public string slug { get; set; }

    public BaseEntity()
    {
        created_at = DateTime.UtcNow;
        updated_at = DateTime.UtcNow;
    }
}
