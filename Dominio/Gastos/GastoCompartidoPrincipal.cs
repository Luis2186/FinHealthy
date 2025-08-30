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
    public class GastoCompartidoPrincipal : Gasto
    {
        public List<GastoCompartido> CompartidoCon { get; set; } = new();
        public bool EsCompartido => true;

        public GastoCompartidoPrincipal() { }
        public GastoCompartidoPrincipal(SubCategoria subCategoria, MetodoDePago metodoDePago, Moneda moneda, DateTime fechaDeGasto,
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

        public Resultado<GastoCompartidoPrincipal> GenerarGastosCompartidos(List<Usuario> participantes, List<decimal>? porcentajes = null)
        {
            var errores = ValidarGeneracion(participantes, porcentajes);
            if (errores.Any())
                return Resultado<GastoCompartidoPrincipal>.Failure(errores);
            CompartidoCon.Clear();
            RepartirMontos(participantes, porcentajes);
            return Resultado<GastoCompartidoPrincipal>.Success(this);
        }

        private List<Error> ValidarGeneracion(List<Usuario> participantes, List<decimal>? porcentajes)
        {
            var errores = new List<Error>();
            if (participantes == null || participantes.Count < 1)
                errores.Add(new Error("GastoCompartidoPrincipal", "Debe seleccionar al menos un miembro para compartir el gasto (incluyendo al creador)."));
            var usarPorcentajes = porcentajes != null && porcentajes.Count == participantes.Count;
            if (usarPorcentajes)
            {
                if (Math.Abs(porcentajes.Sum() - 100m) > 0.01m)
                    errores.Add(new Error("GastoCompartidoPrincipal", "La suma de los porcentajes debe ser 100%"));
            }
            if (!usarPorcentajes && porcentajes != null && porcentajes.Count > 0)
                errores.Add(new Error("GastoCompartidoPrincipal", "La cantidad de porcentajes debe coincidir con la cantidad de usuarios o estar vacío para partes iguales."));
            if (Monto <= 0)
                errores.Add(new Error("GastoCompartidoPrincipal", "El monto debe ser mayor a 0."));
            return errores;
        }

        private void RepartirMontos(List<Usuario> participantes, List<decimal>? porcentajes)
        {
            var usarPorcentajes = porcentajes != null && porcentajes.Count == participantes.Count;
            decimal montoRestante = Monto;
            for (int i = 0; i < participantes.Count; i++)
            {
                var porcentaje = usarPorcentajes ? porcentajes[i] : Math.Round(100m / participantes.Count, 2);
                var montoAsignado = Math.Round(Monto * porcentaje / 100m, 2);
                if (i == participantes.Count - 1) montoAsignado = montoRestante; // Ajuste para redondeo
                montoRestante -= montoAsignado;
                CompartidoCon.Add(new GastoCompartido(this, participantes[i])
                {
                    Porcentaje = porcentaje,
                    MontoAsignado = montoAsignado
                });
            }
        }

        /// <summary>
        /// Valida el gasto compartido y los usuarios involucrados.
        /// </summary>
        public override Resultado<Gasto> EsValido(List<Usuario> participantesGasto = null)
        {
            var errores = ValidarNegocio(participantesGasto);
            if (errores.Any())
                return Resultado<Gasto>.Failure(errores);
            return Resultado<Gasto>.Success(this);
        }

        private List<Error> ValidarNegocio(List<Usuario> participantesGasto)
        {
            var errores = new List<Error>();
            if (Monto <= 0)
                errores.Add(new Error("GastoCompartidoPrincipal", "El monto debe ser mayor a 0."));
            var participantes = participantesGasto ?? new List<Usuario>();
            if (UsuarioCreador != null && !participantes.Any(u => u.Id == UsuarioCreador.Id))
                participantes = new List<Usuario>(participantes) { UsuarioCreador };
            if (!participantes.Any())
                errores.Add(new Error("GastoCompartidoPrincipal", "Debe haber al menos un usuario para compartir el gasto."));
            if (CompartidoCon.Count != participantes.Count)
                errores.Add(new Error("GastoCompartidoPrincipal", "La cantidad de usuarios compartidos no coincide."));
            var sumaPorcentajes = CompartidoCon.Sum(x => x.Porcentaje);
            if (Math.Abs(sumaPorcentajes - 100m) > 0.01m)
                errores.Add(new Error("GastoCompartidoPrincipal", "La suma de los porcentajes debe ser 100%"));
            var idsGrupo = Grupo?.MiembrosGrupoGasto?.Select(u => u.Id).ToHashSet() ?? new HashSet<string>();
            foreach (var gc in CompartidoCon)
            {
                if (!idsGrupo.Contains(gc.MiembroId))
                    errores.Add(new Error("GastoCompartidoPrincipal", $"El usuario {gc.MiembroId} no pertenece al grupo."));
            }
            return errores;
        }

        public override Gasto ActualizarGasto(Gasto datosNuevos)
        {
            if (datosNuevos is GastoCompartidoPrincipal compartido)
            {
                SubCategoria = compartido.SubCategoria;
                MetodoDePago = compartido.MetodoDePago;
                Moneda = compartido.Moneda;
                FechaDeGasto = compartido.FechaDeGasto;
                Descripcion = compartido.Descripcion;
                Etiqueta = compartido.Etiqueta;
                Lugar = compartido.Lugar;
                Monto = compartido.Monto;
                Estado = compartido.Estado;
                Grupo = compartido.Grupo;
                GrupoId = compartido.GrupoId;
                UsuarioCreador = compartido.UsuarioCreador;
                UsuarioCreadorId = compartido.UsuarioCreadorId;
                CompartidoCon = compartido.CompartidoCon;
            }
            return this;
        }
    }
}
