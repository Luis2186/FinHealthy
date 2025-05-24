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
        [Required(ErrorMessage = "La categoria del gasto es requerida")]
        public SubCategoria SubCategoria { get; set; }
        [Required(ErrorMessage = "El metodo de pago del gasto es requerido")]
        public MetodoDePago MetodoDePago { get; set; }
        public Documento? DocumentoAsociado { get; set; }
        [Required(ErrorMessage = "La moneda del gasto es requerida")]
        public Moneda Moneda { get; set; }
        [JsonIgnore] [NotMapped]
        private IGastoStrategy GastoStrategy { get; set; }
        [Required(ErrorMessage = "La fecha del gasto es requerida")]
        public DateTime FechaDeGasto { get; set; }
        public string Descripcion { get; set; }
        public string Lugar { get; set; }
        public string Etiqueta { get; set; }
        public bool Estado { get; set; }
        [Required(ErrorMessage = "El monto del gasto es requerido")]
        public bool EsFinanciado { get; set; }
        public int CantidadDeCuotas { get; set; }
        public List<Cuota> Cuotas { get; set; }  
        public bool EsCompartido { get; set; }
        public List<GastoCompartido> CompartidoCon { get; set; }
        public decimal Monto { get; set; }

        public Gasto(){}
        public Gasto(SubCategoria categoria, MetodoDePago metodoDePago, Moneda moneda, DateTime fecha,
                     string descripcion, string etiqueta,string lugar,bool esFinanciado, decimal monto)
        {
            SubCategoria = categoria;
            MetodoDePago = metodoDePago;
            Moneda = moneda;
            FechaDeGasto = fecha;
            Descripcion = descripcion;
            Etiqueta = etiqueta;
            EsFinanciado = esFinanciado;
            Lugar = lugar;
            Monto = monto;
            CompartidoCon = new List<GastoCompartido>();
            Cuotas = new List<Cuota>();
        }

        public Resultado<Gasto> IngresarGastoCompartido(List<Usuario> usuarios)
        {
            EsCompartido = true;
            GastoStrategy = new GastoCompartidoStrategy();
            return GastoStrategy.CalcularGasto(this, usuarios);
            
        }

        public Resultado<Gasto> IngresarGastoPersonal()
        {
            EsCompartido = false;
            GastoStrategy = new GastoPersonalStrategy();
            return GastoStrategy.CalcularGasto(this);
        }

        public void AdjuntarDocumento(Documento documentoAdjunto)
        {
            DocumentoAsociado = documentoAdjunto;
        }

        // Lógica para generar las cuotas
        public void GenerarCuotas(int cantidadCuotas)
        {
            if (!EsFinanciado || cantidadCuotas <= 1)
                throw new InvalidOperationException("No se puede generar cuotas si el gasto no es financiado o la cantidad es inválida.");


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

            return this;
        }

    }
}
