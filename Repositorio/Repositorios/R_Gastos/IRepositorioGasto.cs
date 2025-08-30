using Dominio.Documentos;
using Dominio;
using Dominio.Gastos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Dominio.Abstracciones;

namespace Repositorio.Repositorios.R_Gastos
{
    public interface IRepositorioGasto : IRepositorioCRUD<Gasto>
    {
        Task<Resultado<List<Gasto>>> ObtenerGastosPorGrupoYUsuario(int grupoId, string usuarioId, CancellationToken cancellationToken, bool esFijo = false);
        Task<Resultado<List<Gasto>>> ObtenerGastosCompartidosPorGrupoYUsuario(int grupoId, string usuarioId, CancellationToken cancellationToken, bool esFijo = false);
        Task<Resultado<List<Gasto>>> ObtenerGastosEnCuotasPorGrupoYUsuario(int grupoId, string usuarioId, CancellationToken cancellationToken);
    }
}
