using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SentimentAnalysisTool.Api.Helpers;
using SentimentAnalysisTool.Services.Services.Implementations;
using SentimentAnalysisTool.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SentimentAnalysisTool.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ICorpusRecordService, CorpusRecordService>();
            services.AddTransient<ICorpusTypeService, CorpusTypeService>();
            services.AddTransient<ICorpusWordsService, CorpusWordsService>();
            services.AddTransient<ISlangRecordsService, SlangRecordsService>();
            services.AddTransient<IWordFrequencyService, WordFrequencyService>();
            services.AddTransient<IRecordService, RecordService>();
            services.AddTransient<IFileHelper, FileHelper>();
            services.AddTransient<IServiceWrapper, ServiceWrapper>();
            services.AddTransient<IAbbreviationsService, AbbreviationsService>();
            services.AddTransient<HttpClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
