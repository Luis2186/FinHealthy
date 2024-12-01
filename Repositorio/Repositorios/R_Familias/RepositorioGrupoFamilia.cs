﻿using Dominio;
using Dominio.Errores;
using Dominio.Familias;
using Microsoft.EntityFrameworkCore;
using Repositorio.Repositorios.R_Familias;

namespace Repositorio.Repositorios.R_Familia
{
    public class RepositorioGrupoFamilia : IRepositorioGrupoFamilia
    {
        private readonly ApplicationDbContext _context;
        public RepositorioGrupoFamilia(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Resultado<Familia>> ActualizarAsync(Familia model)
        {
            try
            {
                var grupo = await ObtenerPorIdAsync(model.Id);

                if (grupo.TieneErrores) return Resultado<Familia>.Failure(grupo.Errores);

                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                _context.Familias.Update(model);
                var resultadoActualizado = await _context.SaveChangesAsync() == 1;

                if (!resultadoActualizado) return Resultado<Familia>.Failure(ErroresCrud.ErrorDeActualizacion("Grupo Familiar"));

                return Resultado<Familia>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<Familia>.Failure(ErroresCrud.ErrorDeExcepcion("UPDATE", ex.Message));
            }
        }

        public async Task<Resultado<Familia>> CrearAsync(Familia model)
        {
            try
            {
                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                await _context.Familias.AddAsync(model);
                var resultadoCreado = await _context.SaveChangesAsync() == 1;

                if (!resultadoCreado) return Resultado<Familia>.Failure(ErroresCrud.ErrorDeCreacion("Grupo Familiar"));

                return Resultado<Familia>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<Familia>.Failure(ErroresCrud.ErrorDeExcepcion("CREATE", ex.Message));
            };
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var grupo = await ObtenerPorIdAsync(id);

                if (grupo.TieneErrores) return Resultado<bool>.Failure(grupo.Errores);

                _context.Familias.Remove(grupo.Valor);
                var resultadoEliminado = await _context.SaveChangesAsync() == 1;

                if (!resultadoEliminado) return Resultado<bool>.Failure(ErroresCrud.ErrorDeEliminacion("Grupo Familiar"));

                return Resultado<bool>.Success(resultadoEliminado);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("REMOVE", ex.Message));
            }
        }

        public async Task<Resultado<Familia>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var grupo = _context.Familias.Find(id);

                if (grupo == null) return Resultado<Familia>.Failure(ErroresCrud.ErrorBuscarPorId("Grupo Familiar"));

                return Resultado<Familia>.Success(grupo);
            }
            catch (Exception ex)
            {
                return Resultado<Familia>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_BY_ID", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<Familia>>> ObtenerTodosAsync()
        {
            try
            {
                var familias = await _context.Familias.ToListAsync();

                if (familias == null || !familias.Any())
                    return Resultado<IEnumerable<Familia>>.Failure(ErroresCrud.ErrorBuscarTodos("Grupos Familiares"));

                return Resultado<IEnumerable<Familia>>.Success(familias);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<Familia>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }
    }
}