using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Base.Auth;
public class SecurityTokenConfig
{
	public const string SectionName = "SecurityToken";

	public string Key { get; set; } = string.Empty;

	public string Issuer { get; set; } = string.Empty;
	public string Audience { get; set; } = string.Empty;

	public string AccessTokenSecretKey { get; set; } = string.Empty;
	public TimeSpan AccessTokenLifetime { get; set; } = TimeSpan.FromMinutes(15);

	public string RefreshTokenSecretKey { get; set; } = string.Empty;
	public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(15);

	public TimeSpan AdminRefreshTokenLifetime { get; set; } = TimeSpan.FromDays(1);
}