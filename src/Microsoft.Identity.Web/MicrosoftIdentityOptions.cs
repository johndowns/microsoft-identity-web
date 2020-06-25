﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Microsoft.Identity.Web
{
    /// <summary>
    /// Options for configuring authentication using Azure Active Directory. It has both AAD and B2C configuration attributes.
    /// </summary>
    public class MicrosoftIdentityOptions : OpenIdConnectOptions
    {
        /// <summary>
        /// Gets or sets the Azure Active Directory instance, e.g. "https://login.microsoftonline.com".
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// Gets or sets the tenant Id.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the domain of the Azure Active Directory tenant, e.g. contoso.onmicrosoft.com.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets TokenAcquisition as a Singleton. There are scenarios, like using the Graph SDK,
        /// which require TokenAcquisition to be a Singleton.
        /// </summary>
        public bool SingletonTokenAcquisition { get; set; } = false;

        /// <summary>
        /// Gets or sets the edit profile user flow name for B2C, e.g. b2c_1_edit_profile.
        /// </summary>
        public string EditProfilePolicyId { get; set; }

        /// <summary>
        /// Gets or sets the sign up or sign in user flow name for B2C, e.g. b2c_1_susi.
        /// </summary>
        public string SignUpSignInPolicyId { get; set; }

        /// <summary>
        /// Gets or sets the reset password user flow name for B2C, e.g. B2C_1_password_reset.
        /// </summary>
        public string ResetPasswordPolicyId { get; set; }

        /// <summary>
        /// Gets the default user flow (which is signUpsignIn).
        /// </summary>
        public string DefaultUserFlow => SignUpSignInPolicyId;

        /// <summary>
        /// Is considered B2C if the attribute SignUpSignInPolicyId is defined.
        /// </summary>
        internal bool IsB2C
        {
            get => !string.IsNullOrWhiteSpace(DefaultUserFlow);
        }

        /// <summary>
        /// Description of the certificates used to prove the identity of the Web app or Web API.
        /// For the moment only the first certificate is considered.
        /// </summary>
        /// <example> An example in the appsetting.json:
        /// <code>
        /// "ClientCertificates": [
        ///   {
        ///     "SourceType": "StoreWithDistinguishedName",
        ///      "CertificateStorePath": "CurrentUser/My",
        ///      "CertificateDistinguishedName": "CN=WebAppCallingWebApiCert"
        ///     }
        ///    ]
        ///   </code>
        ///   See also https://aka.ms/ms-id-web-certificates.
        ///   </example>
        public IEnumerable<CertificateDescription> ClientCertificates { get; set; }

        /// <summary>
        /// Description of the certificates used to decrypt an encrypted token in a Web API.
        /// For the moment only the first certificate is considered.
        /// </summary>
        /// <example> An example in the appsetting.json:
        /// <code>
        /// "TokenDecryptionCertificates": [
        ///   {
        ///     "SourceType": "StoreWithDistinguishedName",
        ///      "CertificateStorePath": "CurrentUser/My",
        ///      "CertificateDistinguishedName": "CN=WebAppCallingWebApiCert"
        ///     }
        ///    ]
        ///   </code>
        ///   See also https://aka.ms/ms-id-web-certificates.
        ///   </example>
        public IEnumerable<CertificateDescription> TokenDecryptionCertificates { get; set; }

        /// <summary>
        /// Specifies if the x5c claim (public key of the certificate) should be sent to the STS.
        /// Sending the x5c enables application developers to achieve easy certificate rollover in Azure AD:
        /// this method will send the public certificate to Azure AD along with the token request,
        /// so that Azure AD can use it to validate the subject name based on a trusted issuer policy.
        /// This saves the application admin from the need to explicitly manage the certificate rollover
        /// (either via portal or PowerShell/CLI operation). For details see https://aka.ms/msal-net-sni.
        /// </summary>
        /// The default is <c>false.</c>
        public bool SendX5C { get; set; } = false;
    }
}
