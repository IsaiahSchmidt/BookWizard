using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BW.Models.Token;

namespace BW.Services.Token
{
    public interface ITokenService
    {
        Task<TokenResponse?> GetTokenAsync(TokenRequest model);
    }
}