using Dominio.Abstracciones;
using Dominio.Documentos;
using Dominio.Usuarios;
using Dominio.Grupos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dominio.Gastos
{
    public abstract class Gasto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La Subcategoria del gasto es requerida")]
        public SubCategoria SubCategoria { get; protected set; }
        [Required(ErrorMessage = "El Metodo de Pago del gasto es requerido")]
        public MetodoDePago MetodoDePago { get; protected set; }
        public Documento? DocumentoAsociado { get; protected set; }
        [Required(ErrorMessage = "La Moneda del gasto es requerida")]
        public Moneda Moneda { get; protected set; }
        [Required(ErrorMessage = "La fecha del gasto es requerida")]
        public DateTime FechaDeGasto { get; protected set; }
        public string Descripcion { get; protected set; }
        public string Lugar { get; protected set; }
        public string Etiqueta { get; protected set; }
        public bool Estado { get; protected set; }
        public decimal Monto { get; protected set; }
        public int GrupoId { get; protected set; }
        public Grupo Grupo { get; protected set; }
        public string UsuarioCreadorId { get; protected set; }
        public Usuario UsuarioCreador { get; protected set; }
        public abstract Resultado<Gasto> EsValido(List<Usuario> usuariosParaCompartirGasto = null);
        public abstract Gasto ActualizarGasto(Gasto datosNuevos);
    }
}
