using Quartz;
using Repositorio.Repositorios.Monedas;
using Repositorio.Repositorios.R_Familias;
using Servicio.ServiciosExternos;

namespace FinHealthAPI.ProcesosSegundoPlano
{
    [DisallowConcurrentExecution]
    public class Trabajo_ActualizacionDeMoneda : IJob
    {
        private readonly IServicioMonedas _servicioMonedas;
           
        public Trabajo_ActualizacionDeMoneda(IServicioMonedas servicioMonedas)
        {
            _servicioMonedas = servicioMonedas;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _servicioMonedas.ActualizarMonedasDesdeServicio();
        }
    }
}
