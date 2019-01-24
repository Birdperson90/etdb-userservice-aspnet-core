﻿using System.Collections.Generic;
using IdentityServer4.Models;

namespace Etdb.UserService.Authentication.Configs
{
    public class IdentityResourceConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}