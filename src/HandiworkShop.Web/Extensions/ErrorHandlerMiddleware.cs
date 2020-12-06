﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HandiworkShop.Web.Extensions
{
    /// <summary>
    /// Global error handler.
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next">Request delegate.</param>
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Error handler.
        /// </summary>
        /// <param name="context">Http context.</param>
        public async Task Invoke(HttpContext context)
        {
            context = context ?? throw new ArgumentNullException(nameof(context));

            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = error switch
                {
                    KeyNotFoundException _ => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                await response.WriteAsync($"Error: {response.StatusCode} - {error.Message}");
            }
        }
    }
}
