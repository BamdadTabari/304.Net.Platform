namespace Core.Base.Auth;
public class LockoutConfig
{
    public const string SectionName = "Lockout";

    public int FailedLoginLimit { get; set; } = 4;
    public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(1);
}