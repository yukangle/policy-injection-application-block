﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reflection;
using EnterpriseLibrary.Common.Configuration;
using EnterpriseLibrary.PolicyInjection.Configuration;
using EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors;
using Unity.Interception.PolicyInjection.MatchingRules;
using Unity.Interception.PolicyInjection.Pipeline;
using Unity.Interception.PolicyInjection.Policies;

namespace EnterpriseLibrary.PolicyInjection.Tests
{
    [TestClass]
    public class RuleCreationFixture
    {
        private IUnityContainer container;

        [TestInitialize]
        public void SetUp()
        {
            container = new UnityContainer().AddNewExtension<Interception>();
        }

        [TestMethod]
        public void CanCreatePolicyWithAssemblyMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new AssemblyMatchingRuleData("RuleName", "assemblyName"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            RuleDrivenPolicy policy = container.Resolve<InjectionPolicy>("Policy") as RuleDrivenPolicy;
            List<IMatchingRule> rules = GetRules(policy);

            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, rules.Count);
            Assert.AreEqual(typeof(AssemblyMatchingRule), rules[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithMemberNameMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new MemberNameMatchingRuleData("RuleName", "memberName"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            RuleDrivenPolicy policy = container.Resolve<InjectionPolicy>("Policy") as RuleDrivenPolicy;
            List<IMatchingRule> rules = GetRules(policy);

            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, rules.Count);
            Assert.AreEqual(typeof(MemberNameMatchingRule), rules[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithMethodSignatureMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new MethodSignatureMatchingRuleData("RuleName", "memberName"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            RuleDrivenPolicy policy = container.Resolve<InjectionPolicy>("Policy") as RuleDrivenPolicy;
            List<IMatchingRule> rules = GetRules(policy);

            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, rules.Count);
            Assert.AreEqual(typeof(MethodSignatureMatchingRule), rules[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithNamespaceMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new NamespaceMatchingRuleData("RuleName", "namespaceName"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            RuleDrivenPolicy policy = container.Resolve<InjectionPolicy>("Policy") as RuleDrivenPolicy;
            List<IMatchingRule> rules = GetRules(policy);

            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, rules.Count);
            Assert.AreEqual(typeof(NamespaceMatchingRule), rules[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithReturnTypeMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new ReturnTypeMatchingRuleData("RuleName", "returnType"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            RuleDrivenPolicy policy = container.Resolve<InjectionPolicy>("Policy") as RuleDrivenPolicy;
            List<IMatchingRule> rules = GetRules(policy);

            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, rules.Count);
            Assert.AreEqual(typeof(ReturnTypeMatchingRule), rules[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithTagMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new TagAttributeMatchingRuleData("RuleName", "tag"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            RuleDrivenPolicy policy = container.Resolve<InjectionPolicy>("Policy") as RuleDrivenPolicy;
            List<IMatchingRule> rules = GetRules(policy);

            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, rules.Count);
            Assert.AreEqual(typeof(TagAttributeMatchingRule), rules[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithTypeMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new TypeMatchingRuleData("RuleName", "returnType"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            RuleDrivenPolicy policy = container.Resolve<InjectionPolicy>("Policy") as RuleDrivenPolicy;
            List<IMatchingRule> rules = GetRules(policy);

            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, rules.Count);
            Assert.AreEqual(typeof(TypeMatchingRule), rules[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithCustomMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new CustomMatchingRuleData("alwaysTrue", typeof(AlwaysMatchingRule)));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            RuleDrivenPolicy policy = container.Resolve<InjectionPolicy>("Policy") as RuleDrivenPolicy;
            List<IMatchingRule> rules = GetRules(policy);

            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, rules.Count);
            Assert.AreEqual(typeof(AlwaysMatchingRule), rules[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithPropertyMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new PropertyMatchingRuleData("alwaysTrue"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            RuleDrivenPolicy policy = container.Resolve<InjectionPolicy>("Policy") as RuleDrivenPolicy;
            List<IMatchingRule> rules = GetRules(policy);

            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, rules.Count);
            Assert.AreEqual(typeof(PropertyMatchingRule), rules[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithCustomAttributeMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules
                .Add(new CustomAttributeMatchingRuleData("alwaysTrue", typeof(TagAttribute), true));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            RuleDrivenPolicy policy = container.Resolve<InjectionPolicy>("Policy") as RuleDrivenPolicy;
            List<IMatchingRule> rules = GetRules(policy);

            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, rules.Count);
            Assert.AreEqual(typeof(CustomAttributeMatchingRule), rules[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithCustomCallHandler()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new CustomMatchingRuleData("alwaysTrue", typeof(AlwaysMatchingRule)));
            settings.Policies.Get(0).Handlers.Add(new CustomCallHandlerData("callCountHandler", typeof(CallCountHandler)));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            InjectionPolicy policy = container.Resolve<InjectionPolicy>("Policy");
            List<ICallHandler> handlers
                = new List<ICallHandler>(policy.GetHandlersFor(MakeMethodImplementationInfo(MethodBase.GetCurrentMethod()), container));

            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, handlers.Count);
            Assert.AreEqual(typeof(CallCountHandler), handlers[0].GetType());
        }

        [TestMethod]
        public void ConfiguresCustomCallHandlerAsSingleton()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new CustomMatchingRuleData("alwaysTrue", typeof(AlwaysMatchingRule)));
            settings.Policies.Get(0).Handlers.Add(new CustomCallHandlerData("callCountHandler", typeof(CallCountHandler)));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            InjectionPolicy policy = container.Resolve<InjectionPolicy>("Policy");
            List<ICallHandler> handlers1
                = new List<ICallHandler>(policy.GetHandlersFor(MakeMethodImplementationInfo(MethodBase.GetCurrentMethod()), container));
            List<ICallHandler> handlers2
                = new List<ICallHandler>(policy.GetHandlersFor(MakeMethodImplementationInfo(MethodBase.GetCurrentMethod()), container));

            CollectionAssert.AreEquivalent(handlers1, handlers2);
        }

        public MethodImplementationInfo MakeMethodImplementationInfo(MethodBase method)
        {
            return new MethodImplementationInfo(null, (MethodInfo)method);
        }

        public static List<IMatchingRule> GetRules(RuleDrivenPolicy policy)
        {
            return (List<IMatchingRule>)typeof(RuleDrivenPolicy)
                .GetField(
                    "_ruleSet",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .GetValue(policy);
        }
    }
}
