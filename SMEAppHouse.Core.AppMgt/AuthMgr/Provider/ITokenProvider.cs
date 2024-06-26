﻿using Microsoft.IdentityModel.Tokens;
using System;

namespace SMEAppHouse.Core.AppMgt.AuthMgr.Provider
{
    public interface ITokenProvider
    {
        string CreateToken(string username, DateTime expiry);

        // TokenValidationParameters is from Microsoft.IdentityModel.Tokens
        TokenValidationParameters GetValidationParameters();
    }
}