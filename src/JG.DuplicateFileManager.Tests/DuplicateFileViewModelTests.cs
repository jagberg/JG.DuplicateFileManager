using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JG.Duplicates.Client;
using Moq;
using Microsoft.Practices.Prism.PubSubEvents;

namespace JG.DuplicateFileManager.Tests
{
    [TestClass]
    public class DuplicateFileViewModelTests
    {
        [TestMethod]
        public void LoadRootComparison_RootDirectory_TreeStructureIsValid()
        {
            //IEventAggregator eventAgg = new Mock<IEventAggregator>();

            DuplicateFileViewModel vm = new DuplicateFileViewModel(null);

            vm.LoadRootComparison();
        }
    }
}
