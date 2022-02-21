using AyoControlLibrary;
using LibEasySave;
using LibEasySave.AppInfo;
using LibEasySave.TranslaterSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPFUI.Ctrl;
using WPFUI.Themes;

namespace WPFUI
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IModelViewJob _modelView;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Test => "Sucess";

        public ITranslatedText TranslatedText { get => Translater.Instance.TranslatedText; }

        public IModelViewJob ModelView => _modelView;


        public MainWindow(IModelViewJob modelView)
        {
            DataContext = this;
            _modelView = modelView;

            //bTestEn.CommandParameter = ELangCode.EN;
            //bTestFr.CommandParameter = ELangCode.FR;

            InitializeComponent();

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

            ScrollPanel.OnItemSelected -= ScrollPanel_OnItemSelected;
            ScrollPanel.OnItemSelected += ScrollPanel_OnItemSelected;

            //DataModel.Instance.AppInfo.OnLangUpdate -= Translater_OnLangUpdate;
            //DataModel.Instance.AppInfo.OnLangUpdate += Translater_OnLangUpdate;

            ScrollPanel_OnItemSelected(null, null);

            EnableJob(DataModel.Instance.IsValid());
        }

        //private void Translater_OnLangUpdate(object sender, EventArgs e)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TranslatedText)));
        //    //foreach (var item in GridMainWindow.Children)
        //    //{
        //    //    (item as Control).InvalidateVisual();
        //    //}
        //}

        private void ScrollPanel_OnItemSelected(object sender, GuidSelecEventArg e)
        {
            EditJobUC.SetJob(null);
            EditJobUC.IsEnabled = false;
            EditJobUC.InvalidateVisual();
        }

        private void EditJobUC_JobNameChanged(object sender, TextChangedEventArgs e)
        {
            Control c = ScrollPanel[_modelView.EditingJob];
            if (!(c is JobChoiceUC))
                return;

            (c as JobChoiceUC).Title = _modelView.Model.Jobs[_modelView.EditingJob].Name;


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
            _modelView?.EditJobCommand.Execute(ScrollPanel.SelectedGuid);
        }

        private void Remove_OnClick(object sender, EventArgs e)
        {
            if (_modelView == null)
                return;
            _modelView?.RemoveJobCommand.Execute(ScrollPanel.SelectedGuid);
        }

        private void RunAll_OnClick(object sender, EventArgs e)
        {
            if (_modelView == null)
                return;
            //_modelView?.RunAllJobCommand.Execute(null);


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
        #endregion

        #region event from model

        private void ModelView_OnRunAll(object sender, GuidSenderEventArg e)
        {
            //List<Guid> guids = new List<Guid>();
            //foreach (var item in ScrollPanel.Controls)
            //{
            //    if (!(item is JobChoiceUC))
            //        continue;

            //    if ((item as JobChoiceUC).IsSelected)
            //        guids.Add((Guid)(item as JobChoiceUC).Tag);
            //}

            //foreach (Guid item in guids)
            //{
            //    _modelView.RunJobCommand.Execute(item);
            //}
            //_modelView.RunAllJobCommand.Execute(guids);
        }

        private void ModelView_OnEditing(object sender, GuidSenderEventArg e)
        {
            EditJobUC.IsEnabled = true;
            EditJobUC.SetJob(_modelView.Model.Jobs[e.Guid]);
            EditJobUC.InvalidateVisual();
        }

        private void ModelView_OnRemoving(object sender, GuidSenderEventArg e)
        {
            ScrollPanel.Remove(e.Guid);
        }

        private void ModelView_OnAdding(object sender, GuidSenderEventArg e)
        {
            var temp = new JobChoiceUC(_modelView.Model.Jobs[e.Guid].Name);

            temp.OnPlayClick -= JobChoiceUC_OnPlayClick;
            temp.OnPlayClick += JobChoiceUC_OnPlayClick;

            temp.OnPauseClick -= JobChoiceUC_OnPauseClick;
            temp.OnPauseClick += JobChoiceUC_OnPauseClick;

            temp.OnStopClick -= JobChoiceUC_OnStopClick;
            temp.OnStopClick += JobChoiceUC_OnStopClick;

            ScrollPanel.Add(temp, e.Guid);
        }

        private void JobChoiceUC_OnStopClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void JobChoiceUC_OnPauseClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void JobChoiceUC_OnPlayClick(object sender, EventArgs e)
        {
            if (!(sender is JobChoiceUC))
                return;

            if (!((sender as JobChoiceUC).Tag is Guid))
                return;

            Guid g = (Guid)(sender as JobChoiceUC).Tag;

            _modelView.RunJobCommand.Execute(g);
        }

        #endregion

        private void cBtnActivAll_OnActivStateChanged(object sender, EventArgs e)
        {
            foreach (JobChoiceUC item in ScrollPanel.Controls)
            {
                item.IsSelected = cBtn_ActivAll.IsActiv;
            }
        }

        private void btn_Setting_OnClick(object sender, EventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow(new ViewDataModel(DataModel.Instance));
            settingWindow.ShowDialog();
            DataModel.Instance.SaveAppInfo();
            EnableJob(DataModel.Instance.IsValid());
        }


        #region Utility

        private void EnableJob(bool state)
        {
            cBtn_ActivAll.IsEnabled = state;
            btnAddJob.IsEnabled = state;
            rCtrlActivAll.IsEnabled = state;
            btnEditJob.IsEnabled = state;
            btnRemoveJob.IsEnabled = state;
            btnRunAllJob.IsEnabled = state;
            EditJobUC.IsEnabled = btnEditJob.IsActiv;
            ScrollPanel.IsEnabled = state;

            if (!state)
            {
                RoundedMessageBox.Show("error !\nData Model not correct\nplease vérify DailyLogPath and StateLogPath");
            }
        }
        #endregion

    }
}
