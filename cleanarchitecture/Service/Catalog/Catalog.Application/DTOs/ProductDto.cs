using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.DTOs
{
    // ---- record (positional) ----
    // "record" is a reference type with built-in value-based equality, ToString, and deconstruction.
    // Positional syntax (parameters in parentheses) generates read-only properties automatically.
    // Choose this for: simple, immutable data carriers like API responses / read models
    //   where you don't need attributes or custom logic on individual properties.
    //
    // ---- class ----
    // "class" is the standard reference type with mutable properties by default.
    // Choose this for: entities with identity, mutable state, or complex behavior/methods.
    //
    // ---- record class (nominal) ----
    // "record class" is the same as "record" (both are reference types) but written with
    //   a class body { } instead of positional parameters.
    // Choose this when you want record benefits (value equality, immutability via init)
    //   AND need property-level attributes like [Required], [Range], data annotations, etc.
    //   Positional records don't support attributes on individual properties easily.

    public record ProductDto
    (
        // Using positional record here because ProductDto is a pure read-only data carrier
        // for API responses — no validation attributes needed, just immutable snapshot of data.

        string Id,
        string Name,
        string Summary,
        string ImageFile,
        BrandDto Brand,
        TypeDto Type,
        decimal Price,
        DateTimeOffset CreatedDate
    );

    public record BrandDto(
        string Id,
        string Name
    );

    public record TypeDto
    (
        string Id,
        string Name
    );

    //Automapper has gone commercial, so we need to write our own AutoMapper DTOs

    // Using "record class" here instead of positional "record" because:
    // 1. We need [Required], [Range] and other DataAnnotation attributes on each property.
    // 2. Positional records don't support per-property attributes cleanly.
    // 3. "record class" gives us value equality + immutability (via init) + attribute support.
    // 4. "init" setters allow the object to be set only during creation (e.g., model binding)
    //     and become read-only afterward.  Example:
    //     var dto = new CreateProductDto { Name = "Phone" };  // allowed
    //     dto.Name = "Tablet";                                // compile error — init-only
    public record class CreateProductDto
    {
        [Required]
        public string Name { get; init; }

        [Required]
        public string Summary { get; init; }

        [Required]
        public string Description { get; init; }

        [Required]
        public string ImageFile { get; init; }

        [Required]
        public string BrandId { get; init; }

        [Required]
        public string TypeId { get; init; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public string Price { get; init; }
    }


    public record class UpdateProductDto
    {
        [Required]
        public string Name { get; init; }

        [Required]
        public string Summary { get; init; }

        [Required]
        public string Description { get; init; }

        [Required]
        public string ImageFile { get; init; }

        [Required]
        public string BrandId { get; init; }

        [Required]
        public string TypeId { get; init; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public string Price { get; init; }
    }
}