using AAH_AutoSim.TestCase.ViewModels;
using AAH_AutoSim.TestCase.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAH_AutoSim.TestCase
{
    public class TestCaseModule : IModule
    {
        public TestCaseModule()
        {

        }

        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.Register<Server.Dialogs.IMessageDialogService, Server.Dialogs.MessageDialogService>();
            containerRegistry.RegisterForNavigation<TestCaseView>();
            containerRegistry.RegisterForNavigation<RunTestCaseView>();
        }
    }
}
