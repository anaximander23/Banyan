using Lamar;
using MediatR;

namespace Banyan.ServiceRegistries
{
    internal class MediatorRegistry : ServiceRegistry
    {
        public MediatorRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();

                scan.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<>));
                scan.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                scan.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
            });

            For<IMediator>().Use<Mediator>().Transient();

            For<ServiceFactory>().Use(ctx => ctx.GetInstance);
        }
    }
}