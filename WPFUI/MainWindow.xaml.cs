using AyoControlLibrary;
using LibEasySave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFUI.Themes;

namespace WPFUI
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        IModelViewJob _modelView;

        public MainWindow(IModelViewJob modelView)
        {
            InitializeComponent();
            _modelView = modelView;

            _modelView.OnAdding -= ModelView_OnAdding;
            _modelView.OnAdding += ModelView_OnAdding;

            _modelView.OnRemoving -= ModelView_OnRemoving;
            _modelView.OnRemoving += ModelView_OnRemoving;

            _modelView.OnEditing -= ModelView_OnEditing;
            _modelView.OnEditing += ModelView_OnEditing;

            _modelView.OnRunAll -= ModelView_OnRunAll;
            _modelView.OnRunAll += ModelView_OnRunAll;

            EditJobUC.JobNameChanged -= EditJobUC_JobNameChanged;
            EditJobUC.JobNameChanged += EditJobUC_JobNameChanged;
        }

        private void EditJobUC_JobNameChanged(object sender, TextChangedEventArgs e)
        {
            Control c = ScrollPanel[_modelView.EditingJobName];
            if (!(c is JobChoiceUC))
                return;

            (c as JobChoiceUC).Name = _modelView.Model.Jobs[_modelView.EditingJobName].Name;


        }


        #region event from UI

        private void btnQuit_OnClick(object sender, EventArgs e)=>  Application.Current.Shutdown();


        private void btnMax_OnClick(object sender, EventArgs e) => 
            WindowState = (WindowState == WindowState.Normal)? WindowState.Maximized : WindowState.Normal;

        private void btnMin_OnClick(object sender, EventArgs e) => WindowState = WindowState.Minimized;

        private void Header_OnDown(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }

        private void Add_OnClick(object sender, EventArgs e)
        {
            if (_modelView == null)
                return;
            _modelView?.AddJobCommand.Execute(null);
        }

        private void Edit_OnClick(object sender, EventArgs e)
        {
            if (_modelView == null)
                return;
            _modelView?.EditJobCommand.Execute(null);
        }

        private void Remove_OnClick(object sender, EventArgs e)
        {
            if (_modelView == null)
                return;
            _modelView?.RemoveJobCommand.Execute(null);
        }

        private void RunAll_OnClick(object sender, EventArgs e)
        {
            if (_modelView == null)
                return;
            _modelView?.RunAllJobCommand.Execute(null);
        }
        #endregion

        #region event from model

        private void ModelView_OnRunAll(object sender, GuidSenderEventArg e)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var item in ScrollPanel.Controls)
            {
                if (!(item is JobChoiceUC))
                    continue;

                if ((item as JobChoiceUC).IsSelected)
                    guids.Add((Guid)(item as JobChoiceUC).Tag);
            }

            foreach (Guid item in guids)
            {
                _modelView.RunJobCommand.Execute(item);
            }
            //_modelView.RunAllJobCommand.Execute(guids);
        }

        private void ModelView_OnEditing(object sender, GuidSenderEventArg e)
        {
            EditJobUC.SetJob(_modelView.Model.Jobs[e.Guid]);
        }

        private void ModelView_OnRemoving(object sender, GuidSenderEventArg e)
        {
            ScrollPanel.Remove(e.Guid);
        }

        private void ModelView_OnAdding(object sender, GuidSenderEventArg e)
        {
            ScrollPanel.Add(new JobChoiceUC(_modelView.Model.Jobs[e.Guid]), e.Guid);
        }

        #endregion

    }
}
