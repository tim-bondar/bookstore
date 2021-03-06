﻿using System.Linq;
using Core;
using Features.Books.Commands;
using FluentValidation;

namespace Features.Books.Validators
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            // All these rules were implemented without requirements, just common sense.
            RuleFor(c => c.Book.Title).NotNull();
            RuleFor(c => c.Book.Title).NotEmpty()
                .ChildRules(x =>
                {
                    x.RuleFor(c => c).Length(1, Constants.MaxTitleSize);
                });

            RuleFor(c => c.Book.Author).NotNull();
            RuleFor(c => c.Book.Author).NotEmpty()
                .ChildRules(x =>
                {
                    x.RuleFor(c => c).Length(1, Constants.MaxAuthorSize);
                });

            RuleFor(c => c.Book.Description).Length(0, Constants.MaxDescriptionSize);
            RuleFor(c => c.Book.Price).GreaterThan(0);
            RuleFor(c => c.Book.Image).NotNull()
                .ChildRules(x =>
                {
                    x.RuleFor(c => c.Length).GreaterThan(0);
                    x.RuleFor(c => c.Length).LessThan(Constants.MaxImageSize); // 10MB max image size for this app
                    x.RuleFor(c => c.ContentType).Custom((s, context) =>
                    {
                        if (!Constants.AllowedImageContentTypes.Contains(s))
                        {
                            context.AddFailure("'Image' should have JPG or PNG content-type.");
                        }
                    });
                });
        }
    }
}
