using Dominio;
using Dominio.Errores;
using Dominio.Familia;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Familia
{
    public class RepositorioGrupoFamilia : IRepositorioGrupoFamilia
    {
        private readonly ApplicationDbContext _context;
        public RepositorioGrupoFamilia(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Resultado<GrupoFamiliar>> ActualizarAsync(GrupoFamiliar model)
        {
            try
            {
                var buscarMiembro = await ObtenerPorIdAsync(model.Id);

                if (buscarMiembro.TieneErrores) return Resultado<GrupoFamiliar>.Failure(buscarMiembro.Errores);

                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                _context.GruposFamiliares.Update(model);
                var resultadoActualizado = await _context.SaveChangesAsync() == 1;

                if (!resultadoActualizado) return Resultado<GrupoFamiliar>.Failure(Errores.ErrorDeActualizacion("Grupo Familiar"));

                return Resultado<GrupoFamiliar>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<GrupoFamiliar>.Failure(Errores.ErrorDeExcepcion("UPDATE", ex.Message));
            }
        }

        public async Task<Resultado<GrupoFamiliar>> CrearAsync(GrupoFamiliar model)
        {
            try
            {
                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                await _context.GruposFamiliares.AddAsync(model);
                var resultadoCreado = await _context.SaveChangesAsync() == 1;

                if (!resultadoCreado) return Resultado<GrupoFamiliar>.Failure(Errores.ErrorDeCreacion("Grupo Familiar"));

                return Resultado<GrupoFamiliar>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<GrupoFamiliar>.Failure(Errores.ErrorDeExcepcion("CREATE", ex.Message));
            };
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var grupo = await ObtenerPorIdAsync(id);

                if (grupo.TieneErrores) return Resultado<bool>.Failure(grupo.Errores);

                _context.GruposFamiliares.Remove(grupo.Valor);
                var resultadoEliminado = await _context.SaveChangesAsync() == 1;

                if (!resultadoEliminado) return Resultado<bool>.Failure(Errores.ErrorDeEliminacion("Grupo Familiar"));

                return Resultado<bool>.Success(resultadoEliminado);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(Errores.ErrorDeExcepcion("REMOVE", ex.Message));
            }
        }

        public async Task<Resultado<GrupoFamiliar>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var grupo = _context.GruposFamiliares.Find(id);

                if (grupo == null) return Resultado<GrupoFamiliar>.Failure(Errores.ErrorBuscarPorId("Grupo Familiar"));

                return Resultado<GrupoFamiliar>.Success(grupo);
            }
            catch (Exception ex)
            {
                return Resultado<GrupoFamiliar>.Failure(Errores.ErrorDeExcepcion("FIND_BY_ID", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<GrupoFamiliar>>> ObtenerTodosAsync()
        {
            try
            {
                var familias = await _context.GruposFamiliares.ToListAsync();

                if (familias == null || !familias.Any())
                    return Resultado<IEnumerable<GrupoFamiliar>>.Failure(Errores.ErrorBuscarTodos("Grupos Familiares"));

                return Resultado<IEnumerable<GrupoFamiliar>>.Success(familias);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<GrupoFamiliar>>.Failure(Errores.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }
    }
}
