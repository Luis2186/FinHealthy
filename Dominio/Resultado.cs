using Dominio.Abstracciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dominio
{
    public struct Resultado<T>
    {
        public readonly T? Valor;
        public static implicit operator Resultado<T>(T valor) => new Resultado<T>(valor);
        public bool EsCorrecto { get; }
        public bool TieneErrores => !EsCorrecto;
        public readonly List<Error>  Errores { get; }


        private Resultado(T value)
        {
            EsCorrecto = true;
            Valor = value;
            Errores = new List<Error>();
        }

        private Resultado(List<Error> errores)
        {
            EsCorrecto = false;
            Errores = errores ?? new List<Error>();
        }

        public static Resultado<T> Success(T value) => new Resultado<T>(value);

        public static Resultado<T> Failure(List<Error> errores) => new Resultado<T>(errores);

        public static Resultado<T> Failure(Error error) => new Resultado<T>(new List<Error> { error });


    }
}
