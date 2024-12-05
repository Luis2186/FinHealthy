using Quartz;

namespace FinHealthAPI.ProcesosSegundoPlano;

    public static class InyeccionDeDependencia
    {
        public static void AgregarInfrastructura(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                var jobKey = JobKey.Create(nameof(Trabajo_ActualizacionDeMoneda));
                options.UseMicrosoftDependencyInjectionJobFactory();

                options.AddJob<Trabajo_ActualizacionDeMoneda>(jobKey)
                .AddTrigger(trigger => 
                trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInHours(24).RepeatForever())
                    );
                
            });

            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
  
        }
    }

