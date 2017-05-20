using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Configuration;
using System.Web.Mvc;
using PrintStore.Domain.Infrastructure.Abstract;
using PrintStore.Domain.Infrastructure.Concrete;
using PrintStore.Infrastructure.Abstract;
using PrintStore.Infrastructure.Concrete;

namespace PrintStore.Infrastructure.Util
{
    /// <summary>
    /// Class for resolving dependencies
    /// </summary>
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        /// <summary>
        /// Registers dependencies and resolves them
        /// </summary>
        private void AddBindings()
        {
            kernel.Bind<ILoggingLayer>().To<EFLoggingLayer>();
            kernel.Bind<IBusinessLogicLayer>().To<EFBusinessLogicLayer>();
            kernel.Bind<IUserLayer>().To<IdentityUserLayer>();
        }
    }
}