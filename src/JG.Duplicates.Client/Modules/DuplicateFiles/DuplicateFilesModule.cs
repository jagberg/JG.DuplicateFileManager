using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.Duplicates.Client.Modules
{
    public class DuplicateFilesModule : IModule
    {
        /// <summary>
        /// The event aggregator.
        /// </summary>
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// The region manager.
        /// </summary>
        private readonly IRegionViewRegistry regionViewRegistry;

        /// <summary>
        /// The unity container.
        /// </summary>
        private readonly IUnityContainer unityContainer;

        /// <summary>
        /// The view of the HomeView.
        /// </summary>
        private readonly DuplicateFilesView view;

        public DuplicateFilesModule(IUnityContainer unityContainer, IRegionViewRegistry regionViewRegistry, IEventAggregator eventAggregator)
        {
            this.unityContainer = unityContainer;
            this.regionViewRegistry = regionViewRegistry;
            this.eventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            var viewModel = this.unityContainer.Resolve<IDuplicateFileViewModel>();

            //viewModel.SelectedFolder = new DelegateCommand(() => this.eventAggregator.GetEvent<DockAllPanels>().Publish(null));

            this.regionViewRegistry.RegisterViewWithRegion(RegionNames.DuplicateFilesRegion, typeof(DuplicateFilesView));
        }
    }
}
