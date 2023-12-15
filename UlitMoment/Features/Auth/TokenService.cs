﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UlitMoment.Configuration;

namespace UlitMoment.Features.Auth;

public class TokenService
{
    private readonly JWTSettings _settings;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    private readonly SymmetricSecurityKey _refreshSecurityKey;
    private readonly SigningCredentials _accessCredentials;
    private readonly SigningCredentials _refreshCredentials;

    public TokenService(IOptions<ApplicationSettings> settings)
    {
        _settings = settings.Value.JWT;

        var accessKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.AccessSecret));
        _accessCredentials = new SigningCredentials(
            accessKey,
            SecurityAlgorithms.HmacSha512Signature
        );

        var refreshKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.RefreshSecret));
        _refreshSecurityKey = refreshKey;
        _refreshCredentials = new SigningCredentials(
            refreshKey,
            SecurityAlgorithms.HmacSha512Signature
        );
    }

    public string CreateAccessToken(string userId)
    {
        var claims = new List<Claim> { new("UserId", userId) };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.TokenValidityInMinutes),
            signingCredentials: _accessCredentials,
            issuer: _settings.ValidIssuer,
            audience: _settings.ValidAudience
        );

        return _tokenHandler.WriteToken(token);
    }

    public string CreateRefreshToken(string userId)
    {
        var claims = new List<Claim> { new("UserId", userId) };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_settings.RefreshTokenValidityInDays),
            signingCredentials: _refreshCredentials,
            issuer: _settings.ValidIssuer,
            audience: _settings.ValidAudience
        );

        return _tokenHandler.WriteToken(token);
    }

    public (bool validationSuccess, ClaimsPrincipal? claims) ValidateRefreshToken(
        string refreshToken
    )
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _settings.ValidIssuer,
            ValidateAudience = true,
            ValidAudience = _settings.ValidAudience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _refreshSecurityKey,
        };

        try
        {
            return (true, _tokenHandler.ValidateToken(refreshToken, validationParameters, out _));
        }
        catch (Exception)
        {
            return (false, null);
        }
    }
}