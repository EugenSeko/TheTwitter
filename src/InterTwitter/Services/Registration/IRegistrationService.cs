﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InterTwitter.Enums;
using InterTwitter.Helpers.ProcessHelpers;
using InterTwitter.Models;

namespace InterTwitter.Services.Registration
{
    public interface IRegistrationService
    {
        Task<AOResult<int>> UserAddAsync(UserModel user);
        Task<AOResult<ECheckEnter>> CheckTheCorrectEmailAsync(string email);
        ECheckEnter CheckTheCorrectPassword(string password, string confirmPassword);
        ECheckEnter CheckCorrectEmail(string email);
        ECheckEnter CheckCorrectName(string name);
        List<UserModel> GetUsers();
    }
}
