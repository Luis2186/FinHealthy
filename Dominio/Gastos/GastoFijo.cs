using Dominio.Abstracciones;
using Dominio.Documentos;
using Dominio.Usuarios;
using Dominio.Grupos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace Dominio.Gastos
{
    public class GastoFijo : Gasto
    {
        public string? Periodicidad { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public GastoFijo() { }
        public GastoFijo(SubCategoria subCategoria, MetodoDePago metodoDePago, Moneda moneda, DateTime fechaDeGasto,
            string descripcion, string etiqueta, string lugar, decimal monto, Grupo grupo, Usuario usuarioCreador)
        {
            SubCategoria = subCategoria;
            MetodoDePago = metodoDePago;
            Moneda = moneda;
            FechaDeGasto = fechaDeGasto;
            Descripcion = descripcion;
            Etiqueta = etiqueta;
            Lugar = lugar;
            Monto = monto;
            Estado = true;
            Grupo = grupo;
            GrupoId = grupo?.Id ?? 0;
            UsuarioCreador = usuarioCreador;
            UsuarioCreadorId = usuarioCreador?.Id;
        }

        public override Resultado<Gasto> EsValido(List<Usuario> usuariosParaCompartirGasto = null)
        {
            var errores = new List<Error>();
            if (Monto <= 0)
                errores.Add(new Error("GastoFijo", "El monto debe ser mayor a 0."));
            if (FechaDeGasto < DateTime.Now.AddYears(-10) || FechaDeGasto > DateTime.Now.AddYears(1))
                errores.Add(new Error("GastoFijo", "La fecha de gasto es inválida."));
            if (string.IsNullOrWhiteSpace(Periodicidad))
                errores.Add(new Error("GastoFijo", "La periodicidad es obligatoria."));
            if (FechaInicio is not null && FechaFin is not null && FechaFin < FechaInicio)
                errores.Add(new Error("GastoFijo", "La fecha de fin no puede ser anterior a la fecha de inicio."));
            if (errores.Any())
                return Resultado<Gasto>.Failure(errores);
            return Resultado<Gasto>.Success(this);
        }

        public override Gasto ActualizarGasto(Gasto datosNuevos)
        {
            if (datosNuevos is GastoFijo fijo)
            {
                Periodicidad = fijo.Periodicidad;
                FechaInicio = fijo.FechaInicio;
                FechaFin = fijo.FechaFin;
                SubCategoria = fijo.SubCategoria;
                MetodoDePago = fijo.MetodoDePago;
                Moneda = fijo.Moneda;
                FechaDeGasto = fijo.FechaDeGasto;
                Descripcion = fijo.Descripcion;
                Etiqueta = fijo.Etiqueta;
                Lugar = fijo.Lugar;
                Monto = fijo.Monto;
                Estado = fijo.Estado;
                Grupo = fijo.Grupo;
                GrupoId = fijo.GrupoId;
                UsuarioCreador = fijo.UsuarioCreador;
                UsuarioCreadorId = fijo.UsuarioCreadorId;
            }
            return this;
        }
    }
}
