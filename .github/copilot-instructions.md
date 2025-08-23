# GitHub Copilot — Instrucciones del Repositorio (.NET 9)

> **Objetivo**: que Copilot genere y refactorice código **limpio, seguro, testeable y eficiente** para **.NET 9** (TFM `net9.0`) con C# moderno y buenas prácticas de ingeniería.

> **Ubicación sugerida**: `.github/copilot-instructions.md` en la raíz del repo.

---

## 0) Contexto del proyecto
- **Runtime/TFM**: `net9.0`.
- **Lenguaje**: C# (usar características modernas cuando apliquen: `required`, `file-scoped types`, `pattern matching`, `records`, `async streams`).
- **Arquitectura base**: *Clean Architecture* con capas `Domain`, `Application`, `Infrastructure`, `Web`.
- **Estilo**: consistente, minimalista, con comentarios solo cuando agreguen valor.
- **Pruebas**: xUnit + FluentAssertions. Integración con `WebApplicationFactory` para APIs.
- **CI**: `dotnet build -warnaserror`, `dotnet format`, `dotnet test` con cobertura mínima 80%.

> Si faltan carpetas o paquetes, **proponer** estructura y commits necesarios explicando por qué.

---

## 1) Estándares de código
- **Nullable**: habilitado (`<Nullable>enable</Nullable>`). Evitar el operador de supresión `!` salvo justificado.
- **Analizadores**: habilitar .NET Analyzers y tratar *warnings* como errores. Integrar StyleCop/IDE analyzers si aplica.
- **Nomenclatura**: `PascalCase` para tipos/miembros públicos; `camelCase` para locales y parámetros; `_camelCase` para campos privados.
- **Inmutabilidad**: preferir `record`/`init` en modelos inmutables y *value objects*.
- **XML docs**: en APIs públicas; evitar comentarios obvios; incluir ejemplos cuando aclaren el contrato.
- **Formateo**: respetar `.editorconfig`; ancho 120; UTF-8; fin de línea LF; archivos terminan con nueva línea.

---

## 2) Arquitectura y patrones
- **SOLID** y separación de responsabilidades. Evitar *God classes* y *anemic domain models*.
- **DI**: usar `Microsoft.Extensions.DependencyInjection`. Inyectar interfaces, no concretas.
- **Configuración**: `IOptions<T>` (validar con `ValidateDataAnnotations` o validadores dedicados). Cargar desde `appsettings`/variables de entorno.
- **Validación**: FluentValidation en los límites del sistema (DTOs/requests), no en el dominio.
- **Mediación/CQRS**: si aplica, usar MediatR con *pipeline behaviors* de logging/validación.
- **Errores**: exponer *Problem Details* (RFC 9457). No filtrar excepciones internas al cliente.

---

## 3) ASP.NET Core (APIs y web)
- **Minimal APIs** para servicios pequeños; **Controllers** cuando hay filtros, versionado o convenciones.
- **Versionado**: por encabezado o ruta `/v{n}`; documentar en OpenAPI.
- **OpenAPI**: con Swashbuckle. Describir esquemas, respuestas y códigos de error.
- **Logging**: `ILogger<T>` y *scopes*; niveles adecuados (Trace→Critical); no loggear datos sensibles.
- **Seguridad**:
  - HTTPS, HSTS y cabeceras seguras.
  - Autenticación con JWT o Identity; autorización por políticas/roles.
  - Validar/sanitizar entradas y salidas; *input normalization*.
  - **Secretos**: nunca en código; usar User Secrets/Key Vault/Variables de entorno.
- **Rendimiento**:
  - `async/await` de punta a punta; soportar `CancellationToken`.
  - Evitar asignaciones innecesarias; `IAsyncEnumerable<>` cuando corresponda.
  - Caching con `IMemoryCache`/`IDistributedCache` y `OutputCache` si aplica.

---

## 4) Persistencia (EF Core)
- **DbContext** con límites claros; **no** usar *lazy loading* por defecto.
- **Mapping**: configurar claves, índices, conversiones, *owned types* y restricciones explícitas.
- **Transacciones**: `DbContext`/`IDbContextTransaction` o *Unit of Work* si corresponde.
- **Consultas**: expresivas, sin N+1; proyectar a DTOs; `AsNoTracking` para lectura.
- **Migraciones**: versionadas y revisadas; scripts reproducibles; *seeding* idempotente.

---

## 5) Contratos, DTOs y mapeo
- **DTOs** separados de entidades de dominio. No exponer entidades en la API.
- **Mapeo**: AutoMapper (perfiles por capa) o mapeo manual cuando sea sencillo/estrictamente performante.
- **Paginación y orden**: usar parámetros estándar (`page`, `pageSize`, `sort`) y respuestas con metadatos.

---

## 6) Calidad, pruebas y TDD
- **Unit tests**: patrón AAA, un concepto por prueba, nombres descriptivos.
- **Integración**: dobles mínimos; `WebApplicationFactory` para API; Testcontainers/Respawn si aplica.
- **Contratos**: pruebas de contrato/OpenAPI en endpoints críticos.
- **Cobertura**: mínima 80%; priorizar reglas de dominio y caminos críticos.
- **Herramientas**: xUnit, FluentAssertions, NSubstitute/Moq.

---

## 7) Utilidades y estilo
- **Guard clauses**: `ArgumentNullException.ThrowIfNull(...)`, validaciones tempranas.
- **Extensiones**: métodos de extensión por tema; evitar *helpers* genéricos sin contexto.
- **Documentación**: README por proyecto, ejemplos de uso y comandos de build/run/test.
- **Internals**: preferir `internal` para restringir superficie pública; `InternalsVisibleTo` solo para pruebas.

---

## 8) Reglas de generación para Copilot
Cuando **escribas o refactorices código**, sigue estas reglas:

1. **Contextualiza** en 1–3 frases qué vas a generar y por qué (DI, patrones, validación, manejo de errores).
2. **Incluye pruebas** (unidad o integración) cuando el cambio lo amerite.
3. **Cancelación**: agrega `CancellationToken` de punta a punta en operaciones I/O.
4. **Logging**: registra eventos clave con `ILogger<T>`; evita verbosidad en *hot paths*.
5. **Errores**: usa excepciones específicas; en API devuélvelas como *Problem Details*.
6. **Seguridad por defecto**: no exponer detalles internos; usar consultas parametrizadas; nunca escribir secretos.
7. **Documentación**: XML docs en endpoints públicos y clases complejas; ejemplos de request/response.
8. **Performance awareness**: evita bloqueos; mide antes de micro-optimizar; `ConfigureAwait(false)` en librerías.
9. **Accesibilidad**: en UI, cumplir WCAG básicas (nombres accesibles, roles/labels).
10. **Opciones**: si propones alternativas, ordénalas por **claridad → mantenibilidad → performance**, con trade‑offs.

---

## 9) Plantillas breves

**Minimal API con validación y Problem Details**
```csharp
app.MapPost("/items", async (CreateItemDto dto, IValidator<CreateItemDto> v, IMediator m, CancellationToken ct) =>
{
    var val = await v.ValidateAsync(dto, ct);
    if (!val.IsValid) return Results.ValidationProblem(val.ToDictionary());

    var id = await m.Send(new CreateItem(dto.Name, dto.Price), ct);
    return Results.Created($"/items/{id}", new { id });
})
.Produces(StatusCodes.Status201Created)
.ProducesValidationProblem()
.WithOpenApi();
```

**Command handler con logging y cancelación**
```csharp
public sealed class CreateItemHandler(ILogger<CreateItemHandler> log, IItemRepo repo)
    : IRequestHandler<CreateItem, Guid>
{
    public async Task<Guid> Handle(CreateItem request, CancellationToken ct)
    {
        log.LogInformation("Creating item {Name}", request.Name);
        var entity = Item.Create(request.Name, request.Price);
        await repo.AddAsync(entity, ct);
        return entity.Id;
    }
}
```

**xUnit + FluentAssertions**
```csharp
public class MoneyTests
{
    [Fact]
    public void Sum_Should_Return_New_Instance_With_Total()
    {
        var a = new Money(10, "USD");
        var b = new Money(15, "USD");

        var total = a + b;

        total.Amount.Should().Be(25);
        total.Currency.Should().Be("USD");
    }
}
```

**Configuración fuerte tipada con validación**
```csharp
public sealed class SmtpOptions
{
    public required string Host { get; init; }
    public required int Port { get; init; }
}

builder.Services.AddOptions<SmtpOptions>()
    .Bind(builder.Configuration.GetSection("Smtp"))
    .ValidateDataAnnotations()
    .Validate(o => o.Port > 0, "Port must be positive");
```

---

## 10) Checklist para PR (usar como guía de revisión)
- [ ] Compila con `-warnaserror` y pasa `dotnet format`.
- [ ] Pruebas verdes y cobertura ≥ 80%.
- [ ] Manejo de errores consistente y Problem Details en API.
- [ ] Logs útiles, sin datos sensibles.
- [ ] Validación de entrada y contratos OpenAPI actualizados.
- [ ] Documentación breve de la decisión técnica (ADR si aplica).

---

## 11) Indicaciones a Copilot Chat (prompt base)
> **Rol**: Eres un asistente técnico senior para un repositorio .NET 9. Debes proponer código y refactors profesionales, seguros y testeables, alineados a estas instrucciones. Antes de mostrar código, explica brevemente tus decisiones. Incluye pruebas cuando agregues lógica. Considera performance, cancelación, logging y seguridad por defecto. Si faltan datos, sugiere supuestos razonables y señala los riesgos.
