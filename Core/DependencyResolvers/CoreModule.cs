using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AutoMapper;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.CrossCuttingConcerns.Caching.Redis;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Core.DependencyResolvers
{
    public class CoreModule:ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddMemoryCache();
            //services.AddStackExchangeRedisCache(option => { option.Configuration = "localhost:6379"; });
            //services.AddDistributedRedisCache(options => { options.Configuration = "localhost:44321"; });
            services.AddSingleton<ICacheManager, MemoryCache>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<Stopwatch>();
            
        }
    }
}
