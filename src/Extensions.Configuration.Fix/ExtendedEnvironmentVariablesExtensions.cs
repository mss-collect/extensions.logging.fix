// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Extension methods for registering <see cref="EnvironmentVariablesConfigurationProvider"/> with <see cref="IConfigurationBuilder"/>.
    /// </summary>
    public static class ExtendedEnvironmentVariablesExtensions
    {
        public static IWebHostBuilder ConfigureConfigurationKeyNormalization(this IWebHostBuilder hostBuilder, Func<string, string> normalizeKeyDelegate)
        {
            if (hostBuilder == null) throw new ArgumentNullException(nameof(hostBuilder));
            if (normalizeKeyDelegate == null) throw new ArgumentNullException(nameof(normalizeKeyDelegate));
            hostBuilder.ConfigureAppConfiguration(app =>
            {
                if (app.Sources.SingleOrDefault(s => s is EnvironmentVariablesConfigurationSource) is EnvironmentVariablesConfigurationSource finding)
                {
                    int oldIndex = app.Sources.IndexOf(finding);
                    //replace on the same position in the list (ordering is important)
                    var newSource = new ExtendedEnvironmentVariablesConfigurationSource
                    {
                        NormalizeKeyCallback = normalizeKeyDelegate
                    };
                    app.Sources[oldIndex] = newSource;
                }
            });
            return hostBuilder;
        }
        public static IWebHostBuilder ConfigureConfigurationFix(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureConfigurationKeyNormalization(key =>
            {
                int logLevelIndex = key.IndexOf("LogLevel:", StringComparison.InvariantCultureIgnoreCase);
                if (logLevelIndex > -1)
                {
                    return key.Replace("_", ".");
                }
                return key;
            }
            );
        }
    }
}
