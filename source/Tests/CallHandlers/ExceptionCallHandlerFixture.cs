﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using EnterpriseLibrary.Common.Configuration;
using EnterpriseLibrary.ExceptionHandling;
using EnterpriseLibrary.ExceptionHandling.Configuration;
using EnterpriseLibrary.ExceptionHandling.PolicyInjection;
using EnterpriseLibrary.PolicyInjection.Configuration;
using EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Injection;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors;
using Unity.Interception.Interceptors.InstanceInterceptors.TransparentProxyInterception;
using Unity.Interception.PolicyInjection.MatchingRules;
using Unity.Interception.PolicyInjection.Pipeline;
using Unity.Interception.PolicyInjection.Policies;

namespace EnterpriseLibrary.PolicyInjection.CallHandlers.Tests
{
    [TestClass]
    public class ExceptionCallHandlerFixture
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ShouldThrowCorrectException()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<TargetType>(new TransparentProxyInterceptor());
            AddNoopPolicy(factory, new TypeMatchingRule("TargetType"));

            TargetType target = factory.Resolve<TargetType>();

            target.WillThrowException();
        }

        [TestMethod]
        public void ShouldBeCreatable()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<TargetType>(new TransparentProxyInterceptor());
            AddExceptionHandlingConfiguration();
            AddExceptionPolicy(factory, "Swallow Exceptions", new TypeMatchingRule("TargetType"));

            TargetType target = factory.Resolve<TargetType>();
            target.WillThrowException();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldTranslateException()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<TargetType>(new TransparentProxyInterceptor());
            AddExceptionHandlingConfiguration();
            AddExceptionPolicy(factory, "Translate Exceptions", new TypeMatchingRule("TargetType"));

            TargetType target = factory.Resolve<TargetType>();
            target.WillThrowException();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ShouldRethrowFromNoOpPolicy()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<TargetType>(new TransparentProxyInterceptor());
            AddExceptionHandlingConfiguration();
            AddExceptionPolicy(factory, "No-Op Policy", new TypeMatchingRule("TargetType"));

            TargetType target = factory.Resolve<TargetType>();
            target.ThrowFromFunctionWithReturnValue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldThrowWhenSwallowingExceptionFromNonVoidMethod()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<TargetType>(new TransparentProxyInterceptor());
            AddExceptionHandlingConfiguration();
            AddExceptionPolicy(factory, "Swallow Exceptions", new TypeMatchingRule("TargetType"));

            TargetType target = factory.Resolve<TargetType>();
            target.ThrowFromFunctionWithReturnValue();
            Assert.Fail("An exception should have been thrown");
        }

        [TestMethod]
        public void ShouldBeAbleToSwallowExceptionFromPropertySet()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<TargetType>(new TransparentProxyInterceptor());
            AddExceptionHandlingConfiguration();
            AddExceptionPolicy(factory, "Swallow Exceptions", new TypeMatchingRule("TargetType"));

            TargetType target = factory.Resolve<TargetType>();
            target.MyProperty = 5;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldThrowWhenSwallowingExceptionFromPropertyGet()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<TargetType>(new TransparentProxyInterceptor());
            AddExceptionHandlingConfiguration();
            AddExceptionPolicy(factory, "Swallow Exceptions", new TypeMatchingRule("TargetType"));

            TargetType target = factory.Resolve<TargetType>();
            int foo = target.MyProperty;
        }

        [TestMethod]
        [Ignore]    // TODO the configurator will not work as currently designed
        public void CanCreateExceptionHandlerFromConfiguration()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            PolicyData policyData = new PolicyData("policy");
            policyData.Handlers.Add(new ExceptionCallHandlerData("exceptionhandler", "Swallow Exceptions"));
            policyData.MatchingRules.Add(new CustomMatchingRuleData("matchesEverything", typeof(AlwaysMatchingRule)));
            settings.Policies.Add(policyData);

            ExceptionHandlingSettings ehabSettings = new ExceptionHandlingSettings();
            ExceptionPolicyData swallowExceptions = new ExceptionPolicyData("Swallow Exceptions");
            swallowExceptions.ExceptionTypes.Add(new ExceptionTypeData("Exception", typeof(Exception), PostHandlingAction.None));
            ehabSettings.ExceptionPolicies.Add(swallowExceptions);
            DictionaryConfigurationSource dictConfigurationSource = new DictionaryConfigurationSource();
            dictConfigurationSource.Add(PolicyInjectionSettings.SectionName, settings);
            dictConfigurationSource.Add(ExceptionHandlingSettings.SectionName, ehabSettings);

            using (PolicyInjector injector = new PolicyInjector(dictConfigurationSource))
            {
                TargetType target = injector.Create<TargetType>();
                target.WillThrowException();
            }
        }

        [TestMethod]
        public void TestCallHandlerCustomFactory()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            PolicyData policyData = new PolicyData("policy");
            ExceptionCallHandlerData data = new ExceptionCallHandlerData("exceptionhandler", "Swallow Exceptions");
            data.Order = 5;
            policyData.Handlers.Add(data);
            policyData.MatchingRules.Add(new CustomMatchingRuleData("matchesEverything", typeof(AlwaysMatchingRule)));
            settings.Policies.Add(policyData);

            ExceptionPolicyData swallowExceptions = new ExceptionPolicyData("Swallow Exceptions");
            swallowExceptions.ExceptionTypes.Add(new ExceptionTypeData("Exception", typeof(Exception), PostHandlingAction.None));

            DictionaryConfigurationSource dictConfigurationSource = new DictionaryConfigurationSource();

            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
            settings.ConfigureContainer(container);
            container.RegisterInstance("Swallow Exceptions", swallowExceptions.BuildExceptionPolicy());


            InjectionPolicy policy = container.Resolve<InjectionPolicy>("policy");

            ICallHandler handler
                = (policy.GetHandlersFor(new MethodImplementationInfo(null, (MethodInfo)MethodBase.GetCurrentMethod()), container)).ElementAt(0);

            Assert.IsNotNull(handler);
            Assert.AreEqual(handler.Order, data.Order);
        }

        void AddNoopPolicy(IUnityContainer factory,
                           params IMatchingRule[] rules)
        {
            var policy = factory.Configure<Interception>().AddPolicy("Noop");
            foreach (var rule in rules)
            {
                policy.AddMatchingRule(rule);
            }
            policy.AddCallHandler(new NoopCallHandler());
        }

        void AddExceptionPolicy(IUnityContainer factory,
                                string exceptionPolicyName,
                                params IMatchingRule[] rules)
        {
            var policy = factory.Configure<Interception>().AddPolicy("Noop");
            foreach (var rule in rules)
            {
                policy.AddMatchingRule(rule);
            }
            policy.AddCallHandler(typeof(ExceptionCallHandler), new InjectionConstructor(exceptionPolicyName));
        }

        private static void AddExceptionHandlingConfiguration()
        {
            ExceptionPolicy.SetExceptionManager(new ExceptionPolicyFactory().CreateManager(), false);
        }
    }

    class TargetType : MarshalByRefObject
    {
        public int MyProperty
        {
            get { throw new NotImplementedException("Exception from property getter"); }
            set { throw new NotImplementedException("Exception from property setter"); }
        }

        public int ThrowFromFunctionWithReturnValue()
        {
            throw new NotImplementedException("This is not implemented either");
        }

        public void WillThrowException()
        {
            throw new NotImplementedException("This is not implemented");
        }
    }

    class NoopCallHandler : ICallHandler
    {
        int order;

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public IMethodReturn Invoke(IMethodInvocation input,
                                    GetNextHandlerDelegate getNext)
        {
            return getNext()(input, getNext);
        }
    }
}
