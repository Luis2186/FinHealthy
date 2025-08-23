using FluentValidation;

namespace Servicio.DTOS.GastosDTO
{
    public class CrearGastoDTOValidator : AbstractValidator<CrearGastoDTO>
    {
        public CrearGastoDTOValidator()
        {
            RuleFor(x => x.SubCategoriaId)
                .GreaterThan(0).WithMessage("La subcategoría es requerida");
            RuleFor(x => x.MetodoDePagoId)
                .GreaterThan(0).WithMessage("El método de pago es requerido");
            RuleFor(x => x.MonedaId)
                .NotEmpty().WithMessage("La moneda es requerida");
            RuleFor(x => x.FechaDeGasto)
                .NotEmpty().WithMessage("La fecha del gasto es requerida")
                .Must(f => f <= DateTime.Now && f >= new DateTime(2000, 1, 1))
                .WithMessage("La fecha del gasto es inválida");
            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La descripción es requerida")
                .MaximumLength(200);
            RuleFor(x => x.Lugar)
                .NotEmpty().WithMessage("El lugar es requerido")
                .MaximumLength(100);
            RuleFor(x => x.Etiqueta)
                .MaximumLength(50);
            RuleFor(x => x.Monto)
                .GreaterThan(0).WithMessage("El monto del gasto debe ser mayor a 0");
            RuleFor(x => x.EsFinanciado)
                .NotNull();
            RuleFor(x => x.CantidadDeCuotas)
                .GreaterThanOrEqualTo(1).When(x => x.EsFinanciado)
                .WithMessage("El gasto financiado debe tener al menos 1 cuota");
            RuleFor(x => x.EsCompartido)
                .NotNull();
        }
    }
}
