﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using EnterpriseLibrary.Common.Configuration;
using EnterpriseLibrary.PolicyInjection.Configuration;
using EnterpriseLibrary.PolicyInjection.Properties;
using Unity.Interception.PolicyInjection.MatchingRules;

namespace EnterpriseLibrary.PolicyInjection.MatchingRules
{
    /// <summary>
    /// Placeholder for <see cref="ReturnTypeMatchingRule"/>.
    /// </summary>
    [ConfigurationElementType(typeof(ReturnTypeMatchingRuleData))]
    public class ReturnTypeMatchingRule : IMatchingRule
    {
        /// <summary>
        /// Tests to see if this rule applies to the given member.
        /// </summary>
        /// <remarks>
        /// This type is available to support the configuration subsystem. Use 
        /// <see cref="ReturnTypeMatchingRule"/> instead.
        /// </remarks>
        public bool Matches(System.Reflection.MethodBase member)
        {
            throw new System.InvalidOperationException(Resources.PlaceholderRule);
        }
    }
}
