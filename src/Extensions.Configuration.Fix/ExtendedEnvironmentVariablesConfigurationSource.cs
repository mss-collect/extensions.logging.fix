// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration.EnvironmentVariables
{
    /// <summary>
    /// Represents environment variables as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class ExtendedEnvironmentVariablesConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// A prefix used to filter environment variables.
        /// </summary>
        public string? Prefix { get; set; }

        public Func<string, string>? NormalizeKeyCallback { get; set; }

        /// <summary>
        /// Builds the <see cref="ExtendedEnvironmentVariablesConfigurationSource"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="ExtendedEnvironmentVariablesConfigurationSource"/></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ExtendedEnvironmentVariablesConfigurationProvider(this.Prefix)
            {
                NormalizeKeyCallback = NormalizeKeyCallback
            };
        }
    }
}
