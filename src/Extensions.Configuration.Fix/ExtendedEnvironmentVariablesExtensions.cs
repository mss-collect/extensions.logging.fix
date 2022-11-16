// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

#pragma warning disable RCS1224 // Make method an extension method.

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Extension methods for registering <see cref="EnvironmentVariablesConfigurationProvider"/> with <see cref="IConfigurationBuilder"/>.
    /// </summary>
    public static class ExtendedEnvironmentVariablesExtensions
    {
        public static string NormalizeKey(string key)
        {
            int logLevelIndex = key.IndexOf("LogLevel:", StringComparison.InvariantCultureIgnoreCase);
            if (logLevelIndex > -1)
            {
                return key.Replace("_", ".");
            }
            return key;
        }

        public static IWebHostBuilder ConfigureConfigurationKeyNormalization(this IWebHostBuilder hostBuilder, Func<string, string> normalizeKeyDelegate)
        {
            if (hostBuilder == null) throw new ArgumentNullException(nameof(hostBuilder));
            if (normalizeKeyDelegate == null) throw new ArgumentNullException(nameof(normalizeKeyDelegate));
            hostBuilder.ConfigureAppConfiguration(configurationBuilder => ConfigureConfigurationFix(configurationBuilder, normalizeKeyDelegate));
            return hostBuilder;
        }

        public static IConfigurationBuilder ConfigureConfigurationFix(this IConfigurationBuilder configurationBuilder)
        {
            return ConfigureConfigurationFix(configurationBuilder, NormalizeKey);
        }

        public static IConfigurationBuilder ConfigureConfigurationFix(this IConfigurationBuilder configurationBuilder, Func<string, string> normalizeKeyDelegate)
        {
            var sources = configurationBuilder.Sources.OfType<EnvironmentVariablesConfigurationSource>().ToArray();
            foreach (EnvironmentVariablesConfigurationSource finding in sources)
            {
                int oldIndex = configurationBuilder.Sources.IndexOf(finding);
                //replace on the same position in the list (ordering is important)
                var newSource = new ExtendedEnvironmentVariablesConfigurationSource
                {
                    NormalizeKeyCallback = normalizeKeyDelegate,
                    Prefix = finding.Prefix
                };
                configurationBuilder.Sources[oldIndex] = newSource;
            }
            return configurationBuilder;
        }

        public static IWebHostBuilder ConfigureConfigurationFix(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureConfigurationKeyNormalization(NormalizeKey);
        }
    }
}
