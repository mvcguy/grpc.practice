using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class FakeTokenSource : ITokenSource
    {
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;
        private readonly SigningCredentials credentials;

        public FakeTokenSource(JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            this.jwtSecurityTokenHandler = jwtSecurityTokenHandler;
            credentials = new SigningCredentials(FakeKeyStore.Key, SecurityAlgorithms.RsaSha512);
        }
        public Task<string> GetTokenAsync(string email)
        {
            return Task.Run(() =>
            {
                var claims = new[] { new Claim(ClaimTypes.Email, email) };
                var token = new JwtSecurityToken(issuer: "http://localhost/",
                    audience: "grpc-auth-demo-console",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(90),
                    signingCredentials: credentials);
                return jwtSecurityTokenHandler.WriteToken(token);
            });
        }
    }
}

