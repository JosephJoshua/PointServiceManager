﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PSMDataManager.Startup))]

namespace PSMDataManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
