using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using SampleWebApiService.Business.CalendarEvents;
using SampleWebApiService.DataAccess;
using SampleWebApiService.DataAccess.Repositories;

namespace SampleWebApiService.Infrastructure
{
    public class ApiCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceSqlLiteDbContext>().As<ServiceDbContext>();
            builder.RegisterType<CalendarEventRepository>().AsImplementedInterfaces();
            builder.RegisterType<CalendarEventsFacade>().AsSelf();
        }
    }
}
