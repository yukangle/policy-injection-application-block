﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Interception.PolicyInjection.MatchingRules;
using Unity.Lifetime;

namespace EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class TagAttributeMatchingRuleFixture : MatchingRuleDataFixtureBase
    {
        [TestMethod]
        public void CanSerializeTypeMatchingRule()
        {
            TagAttributeMatchingRuleData tagAttributeMatchingRule = new TagAttributeMatchingRuleData("RuleName", "Tag");
            tagAttributeMatchingRule.IgnoreCase = true;

            TagAttributeMatchingRuleData deserializedRule = SerializeAndDeserializeMatchingRule(tagAttributeMatchingRule) as TagAttributeMatchingRuleData;

            Assert.IsNotNull(deserializedRule);
            Assert.AreEqual(tagAttributeMatchingRule.Name, deserializedRule.Name);
            Assert.AreEqual(tagAttributeMatchingRule.IgnoreCase, deserializedRule.IgnoreCase);
            Assert.AreEqual(tagAttributeMatchingRule.Match, deserializedRule.Match);
        }

        [TestMethod]
        public void MatchingRuleHasTransientLifetime()
        {
            TagAttributeMatchingRuleData ruleData = new TagAttributeMatchingRuleData("RuleName", "TAg");

            using (var container = new UnityContainer())
            {
                ruleData.ConfigureContainer(container, "-test");
                var registration = container.Registrations.Single(r => r.Name == "RuleName-test");
                Assert.AreSame(typeof(IMatchingRule), registration.RegisteredType);
                Assert.AreSame(typeof(TagAttributeMatchingRule), registration.MappedToType);
                Assert.AreSame(typeof(TransientLifetimeManager), registration.LifetimeManager.GetType());
            }
        }
    }
}
