﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Identity.Web.InstanceDiscovery
{
    /// <summary>
    /// Model child class to hold alias information parsed from the Azure AD issuer endpoint.
    /// </summary>
    internal class Metadata
    {
        /// <summary>
        /// Preferred alias.
        /// </summary>
        [JsonPropertyName(PropertyName = "preferred_network")]
        public string? PreferredNetwork { get; set; }

        /// <summary>
        /// Preferred alias to cache tokens emitted by one of the aliases (to avoid
        /// SSO islands).
        /// </summary>
        [JsonPropertyName(PropertyName = "preferred_cache")]
        public string? PreferredCache { get; set; }

        /// <summary>
        /// Aliases of issuer URLs which are equivalent.
        /// </summary>
        [JsonPropertyName(PropertyName = "aliases")]
        public List<string>? Aliases { get; set; }
    }
}
