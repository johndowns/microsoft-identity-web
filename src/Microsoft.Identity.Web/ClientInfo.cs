﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Identity.Web
{
    internal class ClientInfo
    {
        [JsonPropertyName("uid", IsRequired = false)]
        public string? UniqueObjectIdentifier { get; set; }

        [JsonPropertyName("utid", IsRequired=false)]
        public string? UniqueTenantIdentifier { get; set; }

        public static ClientInfo CreateFromJson(string clientInfo)
        {
            if (string.IsNullOrEmpty(clientInfo))
            {
                throw new ArgumentNullException(nameof(clientInfo), $"client info returned from the server is null");
            }

            return DeserializeFromJson(Base64UrlHelpers.DecodeToBytes(clientInfo));
        }

        internal static ClientInfo DeserializeFromJson(byte[] jsonByteArray)
        {
            if (jsonByteArray == null || jsonByteArray.Length == 0)
            {
                return default;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<ClientInfo>(jsonByteArray, options);
        }
    }
}
