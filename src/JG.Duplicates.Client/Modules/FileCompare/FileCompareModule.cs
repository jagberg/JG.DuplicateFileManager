using JG.Duplicates.Client.Modules;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.Duplicates.Client.Modules
{
    public class FileCompareModule : IModule
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
        private readonly FileCompareView view;

        public FileCompareModule(IUnityContainer unityContainer, IRegionViewRegistry regionViewRegistry, IEventAggregator eventAggregator)
        {
            this.unityContainer = unityContainer;
            this.regionViewRegistry = regionViewRegistry;
            this.eventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            var viewModel = this.unityContainer.Resolve<IFileCompareViewModel>();

            //viewModel.SelectedFolder = new DelegateCommand(() => this.eventAggregator.GetEvent<DockAllPanels>().Publish(null));

            this.regionViewRegistry.RegisterViewWithRegion(RegionNames.FileCompareRegion, typeof(FileCompareView));
        }
    }
}
