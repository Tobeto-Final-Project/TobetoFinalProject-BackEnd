﻿using Application.Features.Auth.Commands.EnableEmailAuthenticator;
using Application.Features.Auth.Commands.EnableOtpAuthenticator;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.RevokeToken;
using Application.Features.Auth.Commands.StudentLogin;
using Application.Features.Auth.Commands.VerifyEmailAuthenticator;
using Application.Features.Auth.Commands.VerifyOtpAuthenticator;
using Core.Application.Dtos;
using Core.Security.Entities;
using Core.Security.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using WebAPI.Controllers.Dtos;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{
    private readonly WebApiConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        const string configurationSection = "WebAPIConfiguration";
        _configuration =
            configuration.GetSection(configurationSection).Get<WebApiConfiguration>()
            ?? throw new NullReferenceException($"\"{configurationSection}\" section cannot found in configuration.");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
    {
        LoginCommand loginCommand = new() { UserForLoginDto = userForLoginDto, IpAddress = getIpAddress() };
        LoggedResponse result = await Mediator.Send(loginCommand);
        
        if (result.RefreshToken is not null)
            setRefreshTokenToCookie(result.RefreshToken);
       
       

        return Ok(result.ToHttpResponse());
    }
    [HttpPost("StudentLogin")]
    public async Task<IActionResult> StudentLogin([FromBody] UserForLoginDto userForLoginDto)
    {
        StudentLoginCommand loginCommand = new() { UserForLoginDto = userForLoginDto, IpAddress = getIpAddress() };
        StudentLoggedResponse result = await Mediator.Send(loginCommand);
        if (result.RefreshToken is not null)
            setRefreshTokenToCookie(result.RefreshToken);



        string encodedRefreshToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(result.RefreshToken.Token));
        result.RefreshToken.Token = encodedRefreshToken;
        return Ok(result.ToHttpResponse());
    }
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
    {
        RegisterCommand registerCommand = new() { UserForRegisterDto = userForRegisterDto, IpAddress = getIpAddress() };
        RegisteredResponse result = await Mediator.Send(registerCommand);
        setRefreshTokenToCookie(result.RefreshToken);
        return Created(uri: "", result.AccessToken);
    }

    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        RefreshTokenCommand refreshTokenCommand = new() { RefreshToken = getRefreshTokenFromCookies(), IpAddress = getIpAddress() };
        RefreshedTokensResponse result = await Mediator.Send(refreshTokenCommand);
        setRefreshTokenToCookie(result.RefreshToken);
        return Created(uri: "", result.AccessToken);
    }
    [HttpGet("RefreshForValue")]
    public async Task<IActionResult> RefreshTokenForValue(string refreshToken)
    {
        string decodedRefreshToken = Encoding.UTF8.GetString(Convert.FromBase64String(refreshToken));
        RefreshTokenCommand refreshTokenCommand = new() { RefreshToken = decodedRefreshToken, IpAddress = getIpAddress() };
        RefreshedTokensResponse result = await Mediator.Send(refreshTokenCommand);
        string encodedRefreshToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(result.RefreshToken.Token));
        
        GetRefreshTokenWithValueItemDto refreshTokenWithValueItemDto = new() { AccessToken = result.AccessToken, RefreshTokenValue = encodedRefreshToken };
        return Created(uri: "", refreshTokenWithValueItemDto);
    }

    [HttpPut("RevokeToken")]
    public async Task<IActionResult> RevokeToken([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string? refreshToken)
    {
        RevokeTokenCommand revokeTokenCommand = new() { Token = refreshToken ?? getRefreshTokenFromCookies(), IpAddress = getIpAddress() };
        RevokedTokenResponse result = await Mediator.Send(revokeTokenCommand);
        return Ok(result);
    }

    [HttpGet("EnableEmailAuthenticator")]
    public async Task<IActionResult> EnableEmailAuthenticator()
    {
        EnableEmailAuthenticatorCommand enableEmailAuthenticatorCommand =
            new() { UserId = getUserIdFromRequest(), VerifyEmailUrlPrefix = $"{_configuration.ApiDomain}/Auth/VerifyEmailAuthenticator" };
        await Mediator.Send(enableEmailAuthenticatorCommand);

        return Ok();
    }

    [HttpGet("EnableOtpAuthenticator")]
    public async Task<IActionResult> EnableOtpAuthenticator()
    {
        EnableOtpAuthenticatorCommand enableOtpAuthenticatorCommand = new() { UserId = getUserIdFromRequest() };
        EnabledOtpAuthenticatorResponse result = await Mediator.Send(enableOtpAuthenticatorCommand);

        return Ok(result);
    }

    [HttpGet("VerifyEmailAuthenticator")]
    public async Task<IActionResult> VerifyEmailAuthenticator([FromQuery] VerifyEmailAuthenticatorCommand verifyEmailAuthenticatorCommand)
    {
        await Mediator.Send(verifyEmailAuthenticatorCommand);
        return Ok();
    }

    [HttpPost("VerifyOtpAuthenticator")]
    public async Task<IActionResult> VerifyOtpAuthenticator([FromBody] string authenticatorCode)
    {
        VerifyOtpAuthenticatorCommand verifyEmailAuthenticatorCommand =
            new() { UserId = getUserIdFromRequest(), ActivationCode = authenticatorCode };

        await Mediator.Send(verifyEmailAuthenticatorCommand);
        return Ok();
    }
  
    private string getRefreshTokenFromCookies() =>
        Request.Cookies["refreshToken"] ?? throw new ArgumentException("Refresh token is not found in request cookies.");

    private void setRefreshTokenToCookie(RefreshToken refreshToken)
    {
        CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7) };
        Response.Cookies.Append(key: "refreshToken", refreshToken.Token, cookieOptions);
    }
}
