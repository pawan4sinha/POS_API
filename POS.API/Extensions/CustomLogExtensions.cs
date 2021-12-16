using Microsoft.AspNetCore.Http;
using POS.Business.Interfaces;
using POS.Data.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace POS.API.Extensions
{
    public class CustomLogExtensions
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public CustomLogExtensions(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        private Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new Logging
            {
                StatusCode=context.Response.StatusCode,
                Message="Internal server error."
            }.ToString());
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context);
            }
        }
    }
}
