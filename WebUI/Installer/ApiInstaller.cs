using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebUI.Installer
{
    public class ApiInstaller:IInstaller
    {
        public void InstallerService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddCors(options =>
                options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("domain"))
            );
        }
    }
}
