﻿using Dominio;
using Dominio.Errores;
using Dominio.Familias;
using Dominio.Gastos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Repositorios.Monedas
{
    public class RepositorioMoneda : IRepositorioMoneda
    {
        private readonly ApplicationDbContext _context;

        public RepositorioMoneda(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Resultado<Moneda>> ActualizarAsync(Moneda model)
        {
            try
            {
                var moneda = await ObtenerPorIdAsync(model.Codigo);

                if (moneda.TieneErrores) return Resultado<Moneda>.Failure(moneda.Errores);

                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                _context.Monedas.Update(model);
                var resultadoActualizado = await _context.SaveChangesAsync() == 1;

                if (!resultadoActualizado) return Resultado<Moneda>.Failure(ErroresCrud.ErrorDeActualizacion("Moneda"));

                return Resultado<Moneda>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<Moneda>.Failure(ErroresCrud.ErrorDeExcepcion("UPDATE", ex.Message));
            }
        }

        public async Task<Resultado<Moneda>> CrearAsync(Moneda model)
        {
            try
            {
                var resultadoValidacion = DataAnnotationsValidator.Validar(model);

                if (resultadoValidacion.TieneErrores) return resultadoValidacion;

                await _context.Monedas.AddAsync(model);
                var resultadoCreado = await _context.SaveChangesAsync() == 1;

                if (!resultadoCreado) return Resultado<Moneda>.Failure(ErroresCrud.ErrorDeCreacion("Moneda"));

                return Resultado<Moneda>.Success(model);
            }
            catch (Exception ex)
            {
                return Resultado<Moneda>.Failure(ErroresCrud.ErrorDeExcepcion("CREATE", ex.Message));
            };
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var moneda = await ObtenerPorIdAsync(id);

                if (moneda.TieneErrores) return Resultado<bool>.Failure(moneda.Errores);

                _context.Monedas.Remove(moneda.Valor);
                var resultadoEliminado = await _context.SaveChangesAsync() == 1;

                if (!resultadoEliminado) return Resultado<bool>.Failure(ErroresCrud.ErrorDeEliminacion("Moneda"));

                return Resultado<bool>.Success(resultadoEliminado);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Failure(ErroresCrud.ErrorDeExcepcion("REMOVE", ex.Message));
            }
        }

        public async Task<Resultado<Moneda>> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Resultado<Moneda>> ObtenerPorIdAsync(string codigo)
        {
            try
            {
                var moneda = _context.Monedas
                    .FirstOrDefault(f => f.Codigo == codigo);

                if (moneda == null) return Resultado<Moneda>.Failure(ErroresCrud.ErrorBuscarPorId("Moneda"));

                return Resultado<Moneda>.Success(moneda);
            }
            catch (Exception ex)
            {
                return Resultado<Moneda>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_BY_ID", ex.Message));
            }
        }

        public async Task<Resultado<IEnumerable<Moneda>>> ObtenerTodosAsync()
        {
            try
            {
                var familias = await _context.Monedas
                    .ToListAsync();

                return Resultado<IEnumerable<Moneda>>.Success(familias);
            }
            catch (Exception ex)
            {
                return Resultado<IEnumerable<Moneda>>.Failure(ErroresCrud.ErrorDeExcepcion("FIND_ALL", ex.Message));
            }
        }
    }
}