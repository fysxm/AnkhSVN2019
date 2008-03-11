﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Ankh.VSPackage;
using AnkhSvn_UnitTestProject.Helpers;
using Ankh.Commands;
using Ankh;
using AnkhSvn_UnitTestProject.Mocks;
using AnkhSvn.Ids;
using Ankh.Selection;
using Microsoft.VisualStudio.Shell.Interop;
using Ankh.UI.Services;
using Ankh.UI;

namespace AnkhSvn_UnitTestProject.CommandRouting
{
    static class CommandTester
    {
        public static void TestExecution(AnkhCommand commandEnum)
        {
            AnkhRuntime runtime = new AnkhRuntime(ServiceProviderHelper.serviceProvider);
            runtime.AddModule(new AnkhModule(runtime));
            runtime.Start();

            bool executed = runtime.CommandMapper.Execute(commandEnum, new CommandEventArgs(commandEnum, runtime.Context));
            if (!executed)
                Assert.Inconclusive("Command disabled");
            Assert.IsTrue(executed);
        }
    }
    [TestClass]
    public class CommandRoutingTest
    {
        [TestMethod]
        public void AddItem()
        {
            MockRepository mocks = new MockRepository();

            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            {
                // TODO: set-up fake selection
                CommandTester.TestExecution(AnkhCommand.AddItem);
            }
        }

        [TestMethod]
        public void AddRepositoryRoot()
        {
            MockRepository mocks = new MockRepository();

            IUIShell uiShell = mocks.DynamicMock<IUIShell>();

            RepositoryRootInfo reposInfo = new RepositoryRootInfo("http://fakeurl/path", 100);

            using (mocks.Record())
            {
                Expect.Call(uiShell.ShowAddRepositoryRootDialog()).Return(reposInfo).Repeat.Any();
            }

            IContext context = AnkhContextMock.GetInstance(mocks, uiShell);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.AddRepositoryRoot);
            }
        }

        [TestMethod]
        public void AddSolutionToRepositoryCommand()
        {
            MockRepository mocks = new MockRepository();
            
            IContext context = AnkhContextMock.GetInstance(mocks);
            using (mocks.Record())
            {
                // TODO: Selection context
            }

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.AddSolutionToRepository);
            }
        }

        [TestMethod]
        public void AddWorkingCopyExplorerRootCommand()
        {
            MockRepository mocks = new MockRepository();

            IUIShell uiShell = mocks.DynamicMock<IUIShell>();

            using (mocks.Record())
            {
                Expect.Call(uiShell.ShowAddWorkingCopyExplorerRootDialog()).Return("C:\\something").Repeat.Any();
            }

            IContext context = AnkhContextMock.GetInstance(mocks, uiShell);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.AddWorkingCopyExplorerRoot);
            }
        }

        [TestMethod]
        public void BlameCommand()
        {
            MockRepository mocks = new MockRepository();

            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                
                CommandTester.TestExecution(AnkhCommand.Blame);
            }
        }

        [TestMethod, Ignore] // Command shows dialog directly
        public void CheckoutCommand()
        {
            MockRepository mocks = new MockRepository();

            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                // TODO: set-up fake selection
                CommandTester.TestExecution(AnkhCommand.Checkout);
            }
        }

        public void CheckoutFolderCommand()
        {
        }

        public void CheckoutSolutionCommand()
        {
        }

        [TestMethod]
        public void CleanupCommand()
        {
            MockRepository mocks = new MockRepository();

            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);
            
            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.Cleanup);
            }
        }

        [TestMethod]
        public void CommitItemCommand()
        {
            MockRepository mocks = new MockRepository();

            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using(mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.CommitItem);
            }
        }

        [TestMethod]
        public void CopyReposExplorerUrl()
        {
            MockRepository mocks = new MockRepository();

            IContext context = AnkhContextMock.GetInstance(mocks);
            using(mocks.Playback())

            using(ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.CopyReposExplorerUrl);
            }
        }

        [TestMethod, Ignore] // shows MessageBox
        public void CreatePatch()
        {
            MockRepository mocks = new MockRepository();

            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IUIShell uiShell = AnkhUIShellMock.GetInstance(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks, uiShell);
            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.CreatePatch);
            }
        }

        [TestMethod]
        public void DiffExternalLocalItem()
        {
            MockRepository mocks = new MockRepository();

            IContext context = AnkhContextMock.GetInstance(mocks);
            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.DiffExternalLocalItem);
            }
        }

        [TestMethod]
        public void DiffLocalItem()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IUIShell uiShell = AnkhUIShellMock.GetInstance(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks, uiShell);
            
            Assert.Inconclusive("Diff not verified");

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.DiffLocalItem);
            }
        }

        [TestMethod, Ignore] // Shows dialog
        public void ExportCommand()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IUIShell uiShell = AnkhUIShellMock.GetInstance(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks, uiShell);


            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.Export);
            }
        }

        [TestMethod] 
        public void ExportFolderCommand()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IUIShell uiShell = AnkhUIShellMock.GetInstance(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks, uiShell);


            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.ExportFolder);
            }
        }

        [TestMethod]
        public void LockCommand()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.Lock);
            }
        }

        [TestMethod]
        public void LogCommand()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.Log);
            }
        }

        [TestMethod]
        public void MakeDirectoryCommand()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.NewDirectory);
            }
        }

        [TestMethod]
        public void Refresh()
        {
            MockRepository mocks = new MockRepository();

            IContext context = AnkhContextMock.GetInstance(mocks);
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            {
                // TODO: set-up fake selection
                CommandTester.TestExecution(AnkhCommand.Refresh);
            }
        }

        [TestMethod]
        public void RelocateCommand()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.Relocate);
            }
        }

        [TestMethod]
        public void RemoveReposRoot()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.RemoveRepositoryRoot);
            }
        }

        [TestMethod]
        public void RemoveWorkingCopyRoot()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.RemoveWorkingCopyExplorerRoot);
            }
        }

        [TestMethod]
        public void ResolveConflictCommand()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.ResolveConflict);
            }
        }

        [TestMethod]
        public void ResolveConflictExternalCommand()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.ResolveConflictExternal);
            }
        }

        [TestMethod]
        public void ReverseMergeCommand()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.RevertToRevision);
            }
        }

        [TestMethod]
        public void RevertItemCommand()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.RevertItem);
            }
        }

        [TestMethod]
        public void SaveToFileCommand()
        {
            MockRepository mocks = new MockRepository();
            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.SaveToFile);
            }
        }

        [TestMethod]
        public void SendErrorReportCommand()
        {
            MockRepository mocks = new MockRepository();

            IErrorHandler errorHandler = mocks.DynamicMock<IErrorHandler>();
            using (mocks.Record())
            {
                errorHandler.SendReport();
                LastCall.Repeat.Once();
            }

            IContext context = AnkhContextMock.GetInstance(mocks);
            mocks.BackToRecord(context, BackToRecordOptions.None);
            Expect.Call(context.ErrorHandler).Return(errorHandler).Repeat.AtLeastOnce();
            mocks.Replay(context);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.SendFeedback);
            }
        }

        [TestMethod]
        public void ShowCommitDialog()
        {
            
            MockRepository mocks = new MockRepository();
            IAnkhPackage package = mocks.CreateMock<IAnkhPackage>();
            using (mocks.Record())
            {
                package.ShowToolWindow(AnkhToolWindow.PendingChanges);
                LastCall.Repeat.Once();
            }
            
            IContext context = AnkhContextMock.GetInstance(mocks);
            mocks.BackToRecord(context);
            Expect.Call(context.Package).Return(package).Repeat.AtLeastOnce();
            mocks.Replay(context);
            
            
            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.ShowCommitDialog);
            }
        }

        [TestMethod]
        public void ShowReposExplorer()
        {
            MockRepository mocks = new MockRepository();
            IAnkhPackage package = mocks.CreateMock<IAnkhPackage>();
            using (mocks.Record())
            {
                package.ShowToolWindow(AnkhToolWindow.RepositoryExplorer);
                LastCall.Repeat.Once();
            }

            IContext context = AnkhContextMock.GetInstance(mocks);
            mocks.BackToRecord(context);
            Expect.Call(context.Package).Return(package).Repeat.AtLeastOnce();
            mocks.Replay(context);


            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.ShowRepositoryExplorer);
            }
        }

        [TestMethod]
        public void ShowWorkingCopyExplorer()
        {
            MockRepository mocks = new MockRepository();
            IAnkhPackage package = mocks.CreateMock<IAnkhPackage>();
            using (mocks.Record())
            {
                package.ShowToolWindow(AnkhToolWindow.WorkingCopyExplorer);
                LastCall.Repeat.Once();
            }

            IContext context = AnkhContextMock.GetInstance(mocks);
            mocks.BackToRecord(context);
            Expect.Call(context.Package).Return(package).Repeat.AtLeastOnce();
            mocks.Replay(context);


            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.ShowWorkingCopyExplorer);
            }
        }

        [TestMethod]
        public void SwitchItemCommand()
        {
            MockRepository mocks = new MockRepository();

            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.SwitchItem);
            }
        }

        [TestMethod]
        public void UnlockCommand()
        {
            MockRepository mocks = new MockRepository();

            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.Unlock);
            }
        }

        [TestMethod]
        public void UpdateItemCommand()
        {
            MockRepository mocks = new MockRepository();

            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.UpdateItem);
            }
        }

        [TestMethod]
        public void ViewInVSNetCommand()
        {
            MockRepository mocks = new MockRepository();

            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.ViewInVsNet);
            }
        }

        [TestMethod]
        public void ViewInWindowsCommand()
        {
            MockRepository mocks = new MockRepository();

            ISelectionContext selC = SelectionContextMock.EmptyContext(mocks);
            IContext context = AnkhContextMock.GetInstance(mocks);

            using (mocks.Playback())
            using (ServiceProviderHelper.AddService(typeof(ISelectionContext), selC))
            using (ServiceProviderHelper.AddService(typeof(IContext), context))
            {
                CommandTester.TestExecution(AnkhCommand.ViewInWindows);
            }
        }
    }
}
