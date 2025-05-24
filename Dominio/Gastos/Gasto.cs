using Dominio.Abstracciones;
using Dominio.Documentos;
using Dominio.Gastos.IGastos;
using Dominio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public class Gasto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La Subcategoria del gasto es requerida")]
        public SubCategoria SubCategoria { get; private set; }
        [Required(ErrorMessage = "El Metodo de Pago del gasto es requerido")]
        public MetodoDePago MetodoDePago { get; private set; }
        public Documento? DocumentoAsociado { get; private set; }
        [Required(ErrorMessage = "La Moneda del gasto es requerida")]
        public Moneda Moneda { get; private set; }
        [JsonIgnore] [NotMapped]
        private IGastoStrategy GastoStrategy { get; set; }
        [Required(ErrorMessage = "La fecha del gasto es requerida")]
        public DateTime FechaDeGasto { get; private set; }
        public string Descripcion { get; private set; }
        public string Lugar { get; private set; }
        public string Etiqueta { get; private set; }
        public bool Estado { get; private set; }
        [Required(ErrorMessage = "El monto del gasto es requerido")]
        public bool EsFinanciado { get; private set; }
        public int CantidadDeCuotas { get; private set; } 
        public List<Cuota> Cuotas { get; private set; }  
        public bool EsCompartido { get; private set; }
        public List<GastoCompartido> CompartidoCon { get; private set; }
        public decimal Monto { get; private set; }

        public Gasto(){}
        public Gasto(SubCategoria categoria, MetodoDePago metodoDePago, Moneda moneda, DateTime fecha,
                     string descripcion, string etiqueta,string lugar,bool esFinanciado,bool esCompartido, decimal monto, 
                     int cantidadCuotas)
        {
            SubCategoria = categoria;
            MetodoDePago = metodoDePago;
            Moneda = moneda;
            FechaDeGasto = fecha;
            Descripcion = descripcion;
            Etiqueta = etiqueta;
            EsFinanciado = esFinanciado;
            EsCompartido = esCompartido;
            Lugar = lugar;
            Monto = monto;
            CompartidoCon = new List<GastoCompartido>();
            Cuotas = new List<Cuota>();
            CantidadDeCuotas = cantidadCuotas;
            Estado = true;
        }

        public Resultado<Gasto> EsValido(List<Usuario> usuariosParaCompartirGasto = null)
        {
            if (Monto <= 0) return Resultado<Gasto>.Failure(new Error("Gasto.EsValido", "El gasto debe ser mayor a 0 "));
            if (SubCategoria == null) return Resultado<Gasto>.Failure(new Error("Gasto.EsValido", "El gasto debe estar asignado a una categoria"));
            if (Moneda == null) return Resultado<Gasto>.Failure(new Error("Gasto.EsValido", "El gasto debe tener asignada una moneda"));
            if (MetodoDePago == null) return Resultado<Gasto>.Failure(new Error("Gasto.EsValido", "El gasto debe tener asignado un metodo de pago"));
            if (FechaDeGasto > DateTime.Now || FechaDeGasto < new DateTime(2000, 1, 1)) return Resultado<Gasto>.Failure(new Error("Gasto.EsValido", "La fecha del gasto es invalida"));

            if (EsFinanciado && CantidadDeCuotas <= 2) return Resultado<Gasto>.Failure(new Error("Gasto.EsValido", "El gasto financiado debe tener 2 o mas cuotas"));
            if (EsCompartido && (usuariosParaCompartirGasto == null || usuariosParaCompartirGasto.Count == 0)) return Resultado<Gasto>.Failure(new Error("Gasto.EsValido", "Los Usuarios con los que intenta compartir el gasto no son validos o no existen"));

            return this;

        }

        public Resultado<Gasto> IngresarGastoCompartido(List<Usuario> usuarios)
        {
            var esValidoResultado = EsValido(usuarios);
            if (esValidoResultado.TieneErrores) return Resultado<Gasto>.Failure(esValidoResultado.Errores);

            GastoStrategy = new GastoCompartidoStrategy();
            return GastoStrategy.CalcularGasto(this, usuarios);
            
        }

        public Resultado<Gasto> IngresarGastoPersonal()
        {
            var esValidoResultado = EsValido();
            if(esValidoResultado.TieneErrores) return Resultado <Gasto>.Failure(esValidoResultado.Errores);

            GastoStrategy = new GastoPersonalStrategy();
            return GastoStrategy.CalcularGasto(this);
        }

        public void AdjuntarDocumento(Documento documentoAdjunto)
        {
            DocumentoAsociado = documentoAdjunto;
        }

        // Lógica para generar las cuotas
        public Resultado<Gasto> GenerarCuotas(int cantidadCuotas)
        {
            if (!EsFinanciado || cantidadCuotas <= 1)
                return Resultado<Gasto>.Failure(new Error("GenerarCuotas","No se puede generar cuotas si el gasto no es financiado o la cantidad es inválida."));

            Cuotas.Clear();
            decimal montoPorCuota = Monto / cantidadCuotas;

            for (int i = 0; i < cantidadCuotas; i++)
            {
                Cuotas.Add(new Cuota
                {
                    GastoId = this.Id,
                    Gasto = this,
                    Monto = montoPorCuota,
                    FechaPago = FechaDeGasto.AddMonths(i + 1), // Las cuotas se asignan mes a mes
                    Pagado = false
                });
            }

            return this;
        }

        public Resultado<Gasto> CompartirGastoConUsuarios(List<Usuario> usuarios)
        {
            if (usuarios == null || usuarios.Count == 0)
                return Resultado<Gasto>.Failure(new Error("Debe haber al menos un usuario para compartir el gasto."));

            var cantidadUsuarios = usuarios.Count;
            var porcentajePorUsuario = Math.Round(100m / cantidadUsuarios, 2); // Redondeamos a 2 decimales
            var montoPorUsuario = Math.Round(this.Monto / cantidadUsuarios, 2);

            var gastosCompartidos = new List<GastoCompartido>();

            foreach (var usuario in usuarios)
            {
                var gastoCompartido = new GastoCompartido(this, usuario)
                {
                    Porcentaje = porcentajePorUsuario,
                    MontoAsignado = montoPorUsuario
                };

                gastosCompartidos.Add(gastoCompartido);
            }

            CompartidoCon = gastosCompartidos;
            
            return this;
        }

    }
}
