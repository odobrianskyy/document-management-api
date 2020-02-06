using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace DocumentManagement.API.Application.Validation
{
    public class FormFileValidator : AbstractValidator<IFormFile>
    {
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public FormFileValidator()
        {
            RuleFor(x => x.FileName)
                .NotNull()
                .NotEmpty()
                .Must(v => v.Trim().EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                    .WithMessage("Only pdf files allowed");

            RuleFor(x => x.Length)
                .LessThanOrEqualTo(MaxFileSize);
        }
    }
}
