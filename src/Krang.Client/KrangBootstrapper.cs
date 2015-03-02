using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using Caliburn.Micro;
using Krang.Client.Services;
using Krang.Client.ViewModels;
using Ninject;

namespace Krang.Client
{
    public class KrangBootstrapper : BootstrapperBase, IDisposable
    {
        private readonly IKernel _kernel;
        private Mutex _mutex;
        private bool _disposed;

        public KrangBootstrapper()
        {
            _kernel = new StandardKernel();
            Initialize();
        }

        ~KrangBootstrapper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (_mutex != null)
            {
                _mutex.Close();
                _mutex = null;
            }

            _disposed = true;
        }

        protected override void BuildUp(object instance)
        {
            _kernel.Inject(instance);
        }

        protected override void Configure()
        {
            // Caliburn components
            _kernel.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
            _kernel.Bind<IWindowManager>().To<WindowManager>().InSingletonScope();

            // Services
            _kernel.Bind<IHadoukenService>().To<HadoukenService>().InSingletonScope();
        }

        protected override object GetInstance(Type service, string key)
        {
            if (service == null) throw new ArgumentNullException("service");
            return _kernel.Get(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            if (service == null) throw new ArgumentNullException("service");
            return _kernel.GetAll(service);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            if (!_kernel.IsDisposed)
            {
                _kernel.Dispose();
            }

            base.OnExit(sender, e);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            // Make sure another instance of Krang isn't already running.
            _mutex = new Mutex(false, "Hadouken-Krang-Application");

            if (!_mutex.WaitOne(0, false))
            {
                Application.Current.Shutdown();
                return;
            }

            base.OnStartup(sender, e);

            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
