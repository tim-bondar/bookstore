using System;
using System.Net;
using System.Threading.Tasks;
using API.Middleware;
using Core.Exceptions;
using FakeItEasy;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests.OtherTests
{
    public class ExceptionHandlerMiddlewareTests
    {
        private readonly ExceptionHandlerMiddleware _middleware;
        private readonly RequestDelegate _next;
        private readonly IActionResultExecutor<ObjectResult> _executor;

        public ExceptionHandlerMiddlewareTests()
        {
            _next = A.Fake<RequestDelegate>();
            _executor = A.Fake<IActionResultExecutor<ObjectResult>>();
            _middleware = new ExceptionHandlerMiddleware(_next, _executor, A.Fake<ILogger<ExceptionHandlerMiddleware>>());
        }

        [Fact]
        public async Task Invoke_should_execute_NotFound_result_if_catches_NotFoundException()
        {
            // Arrange
            A.CallTo(() => _next(A<HttpContext>.Ignored)).Throws(new NotFoundException("Test"));
            var fakeContext = A.Fake<HttpContext>();

            // Act
            await _middleware.Invoke(fakeContext);

            // Assert
            A.CallTo(() => _executor.ExecuteAsync(
                    A<ActionContext>.Ignored,
                    A<ObjectResult>.That
                        .Matches(x => x.StatusCode == (int)HttpStatusCode.NotFound)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Invoke_should_execute_BadRequest_result_if_catches_ValidationException()
        {
            // Arrange
            A.CallTo(() => _next(A<HttpContext>.Ignored)).Throws(new ValidationException("Test"));
            var fakeContext = A.Fake<HttpContext>();

            // Act
            await _middleware.Invoke(fakeContext);

            // Assert
            A.CallTo(() => _executor.ExecuteAsync(
                    A<ActionContext>.Ignored,
                    A<ObjectResult>.That
                        .Matches(x => x.StatusCode == (int)HttpStatusCode.BadRequest)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Invoke_should_execute_InternalServerError_result_if_catches_any_other_exceptions()
        {
            // Arrange
            A.CallTo(() => _next(A<HttpContext>.Ignored)).Throws(new InvalidOperationException("Test"));
            var fakeContext = A.Fake<HttpContext>();

            // Act
            await _middleware.Invoke(fakeContext);

            // Assert
            A.CallTo(() => _executor.ExecuteAsync(
                    A<ActionContext>.Ignored,
                    A<ObjectResult>.That
                        .Matches(x => x.StatusCode == (int)HttpStatusCode.InternalServerError)))
                .MustHaveHappenedOnceExactly();
        }
    }
}
