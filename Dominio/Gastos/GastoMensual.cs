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
    public class GastoMensual : Gasto
    {
        public GastoMensual() { }
        public GastoMensual(SubCategoria subCategoria, MetodoDePago metodoDePago, Moneda moneda, DateTime fechaDeGasto,
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
                errores.Add(new Error("GastoMensual", "El monto debe ser mayor a 0."));
            if (SubCategoria == null)
                errores.Add(new Error("GastoMensual", "El gasto debe estar asignado a una categoria."));
            if (Moneda == null)
                errores.Add(new Error("GastoMensual", "El gasto debe tener asignada una moneda."));
            if (MetodoDePago == null)
                errores.Add(new Error("GastoMensual", "El gasto debe tener asignado un metodo de pago."));
            if (FechaDeGasto > DateTime.Now || FechaDeGasto < new DateTime(2000, 1, 1))
                errores.Add(new Error("GastoMensual", "La fecha del gasto es invalida."));
            if (errores.Any())
                return Resultado<Gasto>.Failure(errores);
            return Resultado<Gasto>.Success(this);
        }

        public override Gasto ActualizarGasto(Gasto datosNuevos)
        {
            if (datosNuevos is GastoMensual mensual)
            {
                SubCategoria = mensual.SubCategoria;
                MetodoDePago = mensual.MetodoDePago;
                Moneda = mensual.Moneda;
                FechaDeGasto = mensual.FechaDeGasto;
                Descripcion = mensual.Descripcion;
                Etiqueta = mensual.Etiqueta;
                Lugar = mensual.Lugar;
                Monto = mensual.Monto;
                Estado = mensual.Estado;
                Grupo = mensual.Grupo;
                GrupoId = mensual.GrupoId;
                UsuarioCreador = mensual.UsuarioCreador;
                UsuarioCreadorId = mensual.UsuarioCreadorId;
            }
            return this;
        }
    }
}
