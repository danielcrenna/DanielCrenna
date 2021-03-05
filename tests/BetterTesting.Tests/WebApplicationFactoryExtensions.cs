// Copyright (c) Daniel Crenna. All rights reserved.
// 
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, you can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace BetterTesting.Tests
{
    internal static class WebApplicationFactoryExtensions
    {
        #region Test Logging

        /// <summary>
        /// Redirect all application logging to the test console.
        /// </summary>
        /// <typeparam name="TStartup">The web application under test</typeparam>
        /// <param name="factory">The chained factory to enable test logging on</param>
        /// <param name="output">The current instance of the Xunit test output helper</param>
        /// <returns></returns>
        public static WebApplicationFactory<TStartup> WithTestLogging<TStartup>(this WebApplicationFactory<TStartup> factory, ITestOutputHelper output) where TStartup : class
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(logging =>
                {
                    logging.ClearProviders(); // remove other logging providers, such as remote loggers or unnecessary event logs
                    logging.Services.AddSingleton<ILoggerProvider>(r => new XunitLoggerProvider(output));
                });
            });
        }

        private sealed class XunitLogger : ILogger
        {
            private const string ScopeDelimiter = "=> ";
            private const string Spacer = "      ";

            private const string Trace = "trce";
            private const string Debug = "dbug";
            private const string Info = "info";
            private const string Warn = "warn";
            private const string Error = "fail";
            private const string Critical = "crit";
            private readonly string _categoryName;

            private readonly ITestOutputHelper _output;
            private readonly IExternalScopeProvider _scopes;

            public XunitLogger(ITestOutputHelper output, IExternalScopeProvider scopes, string categoryName)
            {
                _output = output;
                _scopes = scopes;
                _categoryName = categoryName;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel != LogLevel.None;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return _scopes.Push(state);
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                Func<TState, Exception, string> formatter)
            {
                var sb = new StringBuilder();

                switch (logLevel)
                {
                    case LogLevel.Trace:
                        sb.Append(Trace);
                        break;
                    case LogLevel.Debug:
                        sb.Append(Debug);
                        break;
                    case LogLevel.Information:
                        sb.Append(Info);
                        break;
                    case LogLevel.Warning:
                        sb.Append(Warn);
                        break;
                    case LogLevel.Error:
                        sb.Append(Error);
                        break;
                    case LogLevel.Critical:
                        sb.Append(Critical);
                        break;
                    case LogLevel.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                }

                sb.Append(": ").Append(_categoryName).Append('[').Append(eventId).Append(']').AppendLine();

                if (TryAppendScopes(sb))
                    sb.AppendLine();

                sb.Append(Spacer);
                sb.Append(formatter(state, exception));

                if (exception != null)
                {
                    sb.AppendLine();
                    sb.Append(Spacer);
                    sb.Append(exception);
                }

                var message = sb.ToString();
                _output.WriteLine(message);
            }

            private bool TryAppendScopes(StringBuilder sb)
            {
                var scopes = false;
                _scopes.ForEachScope((callback, state) =>
                {
                    if (!scopes)
                    {
                        state.Append(Spacer);
                        scopes = true;
                    }

                    state.Append(ScopeDelimiter);
                    state.Append(callback);
                }, sb);
                return scopes;
            }
        }

        private sealed class XunitLoggerProvider : ILoggerProvider, ISupportExternalScope
        {
            private readonly ITestOutputHelper _output;
            private IExternalScopeProvider _scopes;

            public XunitLoggerProvider(ITestOutputHelper output)
            {
                _output = output;
            }

            public ILogger CreateLogger(string categoryName)
            {
                return new XunitLogger(_output, _scopes, categoryName);
            }

            public void Dispose()
            {
            }

            public void SetScopeProvider(IExternalScopeProvider scopes)
            {
                _scopes = scopes;
            }
        }

        #endregion
    }
}