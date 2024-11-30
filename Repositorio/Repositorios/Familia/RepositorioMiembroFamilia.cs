using Dominio;
using Dominio.Abstracciones;
using Dominio.Errores;
using Dominio.Familia;
using Dominio.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Familia
{
    public class RepositorioMiembroFamilia : IRepositorioMiembroFamilia
    {
        private readonly ApplicationDbContext _context;
        public RepositorioMiembroFamilia(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Resultado<MiembroFamilia>> ActualizarAsync(MiembroFamilia model)
        {
            try
            {
                var buscarMiembro = await ObtenerPorIdAsync(model.Id);

                if (buscarMiembro.TieneErrores) return Resultado<MiembroFamilia>.Failure(buscarMiembro.Errores);

                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;
                
                _context.MiembrosFamiliares.Update(model);
                var resultadoActualizado = await _context.SaveChangesAsync() == 1;

                if (!resultadoActualizado) return Resultado<MiembroFamilia>.Failure(Errores.ErrorDeActualizacion("Miembro Familiar"));
                
                return Resultado<MiembroFamilia>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<MiembroFamilia>.Failure(Errores.ErrorDeExcepcion("UPDATE",ex.Message));
            }
        }

        public async Task<Resultado<MiembroFamilia>> CrearAsync(MiembroFamilia model)
        {
            try
            {
                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                await _context.MiembrosFamiliares.AddAsync(model);
                var resultadoCreado = await _context.SaveChangesAsync() == 1;

                if (!resultadoCreado) return Resultado<MiembroFamilia>.Failure(Errores.ErrorDeCreacion("Miembro Familiar"));

                return Resultado<MiembroFamilia>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<MiembroFamilia>.Failure(Errores.ErrorDeExcepcion("CREATE", ex.Message));
            };
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var buscarMiembro = await ObtenerPorIdAsync(id);

                if (buscarMiembro.TieneErrores) return Resultado<bool>.Failure(buscarMiembro.Errores);

                _context.MiembrosFamiliares.Remove(buscarMiembro.Valor);
                var resultadoEliminado = await _context.SaveChangesAsync() == 1;

                if (!resultadoEliminado) return Resultado<bool>.Failure(Errores.ErrorDeEliminacion("Miembro Familiar"));

                return Resultado<bool>.Success(resultadoEliminado);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(Errores.ErrorDeExcepcion("REMOVE", ex.Message));
            }
        }

        public async Task<Resultado<MiembroFamilia>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var miembro = _context.MiembrosFamiliares.Find(id);

                if (miembro == null) return Resultado<MiembroFamilia>.Failure(Errores.ErrorBuscarPorId("Miembro Familiar"));

                return Resultado<MiembroFamilia>.Success(miembro);
            }
            catch (Exception ex)
            {
                return Resultado<MiembroFamilia>.Failure(Errores.ErrorDeExcepcion("FIND_BY_ID", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<MiembroFamilia>>> ObtenerTodosAsync()
        {
            try
            {
                var miembros = await _context.MiembrosFamiliares.ToListAsync();

                if (miembros == null || !miembros.Any())
                    return Resultado<IEnumerable<MiembroFamilia>>.Failure(Errores.ErrorBuscarTodos("Miembros Familiares"));

                return Resultado<IEnumerable<MiembroFamilia>>.Success(miembros);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<MiembroFamilia>>.Failure(Errores.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }
    }
}
