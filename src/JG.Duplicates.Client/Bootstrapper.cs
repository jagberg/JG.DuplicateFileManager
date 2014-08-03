//using Autofac;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
//using Prism.AutofacExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using JG.Duplicates.Client.Modules;

namespace JG.Duplicates.Client
{
    internal class Bootstrapper : UnityBootstrapper
    {
        protected override System.Windows.DependencyObject CreateShell()
        {
            //var builder = new ContainerBuilder();
            //builder.RegisterSource(new ResolveAnythingSource());
            //var container = builder.Build();
            

            //IRegionManager regionManager = base.Container.Resolve<IRegionManager>();
            //Cinch.ViewModelBase.ServiceProvider.Add(typeof(IViewManager),
            //    new ViewManager(regionManager));


            return this.Container.Resolve<Shell>();
            //return new Shell();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
        }

        /// <summary>
        /// Populates the module's collection with the modules.
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(Modules.DuplicateFilesModule));
            moduleCatalog.AddModule(typeof(Modules.FileCompareModule));
        }

        /// <summary>
        /// Configures the <see cref="T:Microsoft.Practices.Unity.IUnityContainer"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.RegisterDuplicateFileViewModel();
            this.RegisterFileCompareViewModel();
        }

        private void RegisterDuplicateFileViewModel()
        {
            var windowViewModelMappingsLifetimeManager = new ContainerControlledLifetimeManager();

            try
            {
                this.Container.RegisterType<IDuplicateFileViewModel, DuplicateFileViewModel>(windowViewModelMappingsLifetimeManager);
            }
            catch
            {
                windowViewModelMappingsLifetimeManager.Dispose();
                throw;
            }

            var menuViewModel = this.Container.Resolve<IDuplicateFileViewModel>();
            this.Container.RegisterInstance(menuViewModel);
        }



        private void RegisterFileCompareViewModel()
        {
            var windowViewModelMappingsLifetimeManager = new ContainerControlledLifetimeManager();

            try
            {
                this.Container.RegisterType<IFileCompareViewModel, FileCompareViewModel>(windowViewModelMappingsLifetimeManager);
            }
            catch
            {
                windowViewModelMappingsLifetimeManager.Dispose();
                throw;
            }

            var menuViewModel = this.Container.Resolve<IFileCompareViewModel>();
            this.Container.RegisterInstance(menuViewModel);
        }


    }
}
