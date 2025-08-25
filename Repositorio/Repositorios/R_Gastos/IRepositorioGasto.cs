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
        Task<Resultado<List<Gasto>>> ObtenerGastosFijosPorGrupoYUsuarioIncluyendoTodo(int grupoId, string usuarioId, CancellationToken cancellationToken);
        Task<Resultado<List<Gasto>>> ObtenerGastosCompartidosPorGrupoYUsuarioIncluyendoTodo(int grupoId, string usuarioId, CancellationToken cancellationToken);
        Task<Resultado<List<Gasto>>> ObtenerGastosEnCuotasPorGrupoYUsuarioIncluyendoTodo(int grupoId, string usuarioId, CancellationToken cancellationToken);
    }
}
