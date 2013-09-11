﻿using NUnit.Framework;

namespace DryIoc.UnitTests.Playground
{
    [TestFixture]
    public class DecoratorConditionPlayTests
    {
        [Test]
        public void Register_decorator_with_condition()
        {
            var container = new Container();
            container.Register<IHandler, FastHandler>(named: "fast");
            container.Register<IHandler, SlowHandler>(named: "slow");
            container.RegisterDecorator(
                new Decorator(
                    new ReflectionFactory(typeof(LoggingHandlerDecorator), setup: Factory.With(skipCache: true)),
                    request => Equals(request.ServiceKey, "slow")),
                typeof(IHandler));

            Assert.That(container.Resolve<IHandler>("fast"), Is.InstanceOf<FastHandler>());
            Assert.That(container.Resolve<IHandler>("slow"), Is.InstanceOf<LoggingHandlerDecorator>());
        }
    }

    public interface IHandler
    {
    }

    class FastHandler : IHandler
    {

    }

    class SlowHandler : IHandler
    {

    }

    class LoggingHandlerDecorator : IHandler
    {
        public IHandler Handler { get; set; }

        public LoggingHandlerDecorator(IHandler handler)
        {
            Handler = handler;
        }
    }
}
