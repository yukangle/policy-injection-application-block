﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Interception.PolicyInjection.MatchingRules;
using Unity.Lifetime;

namespace EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class AssemblyNameMatchingRuleDataFixture : MatchingRuleDataFixtureBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanSerializeTypeMatchingRule()
        {
            AssemblyMatchingRuleData asmMatchingRule = new AssemblyMatchingRuleData("RuleName", "mscorlib");

            AssemblyMatchingRuleData deserializedRule = SerializeAndDeserializeMatchingRule(asmMatchingRule) as AssemblyMatchingRuleData;

            Assert.IsNotNull(deserializedRule);
            Assert.AreEqual(asmMatchingRule.Name, deserializedRule.Name);
            Assert.AreEqual(asmMatchingRule.Match, deserializedRule.Match);
        }

        [TestMethod]
        public void MatchingRuleHasTransientLifetime()
        {
            AssemblyMatchingRuleData asmMatchingRule = new AssemblyMatchingRuleData("RuleName", "mscorlib");

            using (var container = new UnityContainer())
            {
                asmMatchingRule.ConfigureContainer(container, "-test");
                var registration = container.Registrations.Single(r => r.Name == "RuleName-test");
                Assert.AreSame(typeof(IMatchingRule), registration.RegisteredType);
                Assert.AreSame(typeof(AssemblyMatchingRule), registration.MappedToType);
                Assert.AreSame(typeof(TransientLifetimeManager), registration.LifetimeManager.GetType());
            }
        }
    }
}
