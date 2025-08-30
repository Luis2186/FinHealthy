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
    public class GastoEnCuotas : Gasto
    {
        public bool EsFinanciado { get; private set; }
        public int CantidadDeCuotas { get; private set; }
        public List<Cuota> Cuotas { get; private set; } = new();

        public GastoEnCuotas() { }
        public GastoEnCuotas(SubCategoria categoria, MetodoDePago metodoDePago, Moneda moneda, DateTime fecha,
                         string descripcion, string etiqueta, string lugar, decimal monto, int cantidadCuotas,
                         Grupo grupo, Usuario usuarioCreador)
        {
            SubCategoria = categoria;
            MetodoDePago = metodoDePago;
            Moneda = moneda;
            FechaDeGasto = fecha;
            Descripcion = descripcion;
            Etiqueta = etiqueta;
            Lugar = lugar;
            Monto = monto;
            Estado = true;
            Grupo = grupo;
            GrupoId = grupo?.Id ?? 0;
            UsuarioCreador = usuarioCreador;
            UsuarioCreadorId = usuarioCreador?.Id;
            EsFinanciado = true;
            CantidadDeCuotas = cantidadCuotas;
        }

        public override Resultado<Gasto> EsValido(List<Usuario> usuariosParaCompartirGasto = null)
        {
            var errores = ValidarNegocio();
            if (errores.Any())
                return Resultado<Gasto>.Failure(errores);
            return Resultado<Gasto>.Success(this);
        }

        private List<Error> ValidarNegocio()
        {
            var errores = new List<Error>();
            if (Monto <= 0)
                errores.Add(new Error("GastoEnCuotas", "El monto debe ser mayor a 0."));
            if (SubCategoria == null)
                errores.Add(new Error("GastoEnCuotas", "El gasto debe estar asignado a una categoria."));
            if (Moneda == null)
                errores.Add(new Error("GastoEnCuotas", "El gasto debe tener asignada una moneda."));
            if (MetodoDePago == null)
                errores.Add(new Error("GastoEnCuotas", "El gasto debe tener asignado un metodo de pago."));
            if (FechaDeGasto > DateTime.Now || FechaDeGasto < new DateTime(2000, 1, 1))
                errores.Add(new Error("GastoEnCuotas", "La fecha del gasto es invalida."));
            if (!EsFinanciado || CantidadDeCuotas <= 1)
                errores.Add(new Error("GastoEnCuotas", "No se puede generar cuotas si el gasto no es financiado o la cantidad es inválida."));
            if (Cuotas.Count != CantidadDeCuotas)
                errores.Add(new Error("GastoEnCuotas", "La cantidad de cuotas no coincide."));
            return errores;
        }

        public Resultado<GastoEnCuotas> GenerarCuotas(int cantidadCuotas)
        {
            var errores = ValidarGeneracion(cantidadCuotas);
            if (errores.Any())
                return Resultado<GastoEnCuotas>.Failure(errores);
            Cuotas.Clear();
            RepartirCuotas(cantidadCuotas);
            return Resultado<GastoEnCuotas>.Success(this);
        }

        private List<Error> ValidarGeneracion(int cantidadCuotas)
        {
            var errores = new List<Error>();
            if (!EsFinanciado || cantidadCuotas <= 1)
                errores.Add(new Error("GenerarCuotas","No se puede generar cuotas si el gasto no es financiado o la cantidad es inválida."));
            if (Monto <= 0)
                errores.Add(new Error("GenerarCuotas","El monto debe ser mayor a 0."));
            return errores;
        }

        private void RepartirCuotas(int cantidadCuotas)
        {
            decimal montoPorCuota = Math.Round(Monto / cantidadCuotas, 2);
            decimal montoRestante = Monto;
            for (int i = 0; i < cantidadCuotas; i++)
            {
                var monto = (i == cantidadCuotas - 1) ? montoRestante : montoPorCuota;
                montoRestante -= monto;
                Cuotas.Add(new Cuota
                {
                    GastoId = this.Id,
                    Gasto = this,
                    Monto = monto,
                    FechaPago = FechaDeGasto.AddMonths(i + 1),
                    Pagado = false
                });
            }
        }

        public override Gasto ActualizarGasto(Gasto datosNuevos)
        {
            if (datosNuevos is GastoEnCuotas cuotas)
            {
                SubCategoria = cuotas.SubCategoria;
                MetodoDePago = cuotas.MetodoDePago;
                Moneda = cuotas.Moneda;
                FechaDeGasto = cuotas.FechaDeGasto;
                Descripcion = cuotas.Descripcion;
                Etiqueta = cuotas.Etiqueta;
                Lugar = cuotas.Lugar;
                Monto = cuotas.Monto;
                Estado = cuotas.Estado;
                Grupo = cuotas.Grupo;
                GrupoId = cuotas.GrupoId;
                UsuarioCreador = cuotas.UsuarioCreador;
                UsuarioCreadorId = cuotas.UsuarioCreadorId;
                EsFinanciado = cuotas.EsFinanciado;
                CantidadDeCuotas = cuotas.CantidadDeCuotas;
                Cuotas = cuotas.Cuotas;
            }
            return this;
        }
    }
}
