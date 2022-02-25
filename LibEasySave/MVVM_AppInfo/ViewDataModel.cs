using LibEasySave.TranslaterSystem;
using System.ComponentModel;
using System.Windows.Input;

namespace LibEasySave.AppInfo
{

    public class ViewDataModel : ObservableObject, IViewDataModel
    {
        private IDataModel _model;
        private ICommand _setLangcommand;
        private ICommand _setAllowExtCommand;
        private ICommand _setPriorityExtCommand;
        private ICommand _setCryptExtCommand;
        private ICommand _setDailyLogPathCommand;
        private ICommand _setStateLogPathCommand;
        private ICommand _setLogFormatCommand;
        private ICommand _setAppModeCommand;
        private ICommand _setJobAppCommand;


        public bool IsLangEnActiv => _model.AppInfo.ActivLang == TranslaterSystem.ELangCode.EN;

        public bool IsLangFrActiv => _model.AppInfo.ActivLang == TranslaterSystem.ELangCode.FR;


        public bool IsLogFormatJsonActiv => _model.LogInfo.SavingFormat == ESavingFormat.JSON;

        public bool IsLogFormatXmlActiv => _model.LogInfo.SavingFormat == ESavingFormat.XML;


        public bool IsAppModeClient => _model.AppInfo.ModeIHM == EModeIHM.Client;

        public bool IsAppModeServer => _model.AppInfo.ModeIHM == EModeIHM.Server;

        public ELangCode LangCodeFR => ELangCode.FR;
        public ELangCode LangCodeEN => ELangCode.EN;

        public ESavingFormat SavingFormatXML => ESavingFormat.XML;
        public ESavingFormat SavingFormatJSON => ESavingFormat.JSON;

        public EModeIHM ModeIHMClient => EModeIHM.Client;
        public EModeIHM ModeIHMServer => EModeIHM.Server;



        public IDataModel DataModel => _model;

        public ICommand SetAllowExtCommand => _setAllowExtCommand;
        public ICommand SetPriorityExtCommand => _setPriorityExtCommand;
        public ICommand SetCryptExtCommand => _setCryptExtCommand;
        public ICommand SetDailyLogPathCommand => _setDailyLogPathCommand;
        public ICommand SetStateLogPathCommand => _setStateLogPathCommand;
        public ICommand SetLogFormatCommand => _setLogFormatCommand;
        public ICommand SetAppModeCommand => _setAppModeCommand;
        public ICommand SetLangCommand => _setLangcommand;
        public ICommand SetJobAppCommand => _setJobAppCommand;



        //public string CryptKey { get => _model.CryptInfo.Key; set => _model.CryptInfo.Key = value; }

        //public string DailyLogPath => _model.LogInfo.DailyLogPath;

        //public string StateLogPath => _model.LogInfo.StateLogPath;


        // constructor
        public ViewDataModel(IDataModel dataModel)
        {
            _model = dataModel;
            _setLangcommand = new ChangeLangJobCommand(this,FireLangCodeChanged);
            _setLogFormatCommand = new SetLogFormatDataModelCommand(this,FireSavingFormatChanged);
            _setAppModeCommand = new SetAppModeDataModelCommand(this,FireAppModeChanged);

            _setAllowExtCommand = new SetAllowExtListDataModelCommand(this);
            _setPriorityExtCommand = new SetPriorityExtListDataModelCommand(this);
            _setCryptExtCommand = new SetCryptExtListDataModelCommand(this);

            _setDailyLogPathCommand = new SetDailyLogPathDataModelCommand(this);
            _setStateLogPathCommand = new SetStateLogPathDataModelCommand(this);
            _setJobAppCommand = new SetJobAppListDataModelCommand(this);


        }


        public void FireLangCodeChanged()
        {
            PropChanged(nameof(IsLangEnActiv));
            PropChanged(nameof(IsLangFrActiv));
        }
        
        public void FireSavingFormatChanged()
        {
            PropChanged(nameof(IsLogFormatJsonActiv));
            PropChanged(nameof(IsLogFormatXmlActiv));
        }        
        public void FireAppModeChanged()
        {
            PropChanged(nameof(IsAppModeClient));
            PropChanged(nameof(IsAppModeServer));
        }        



    }


    public delegate void UpdateDelegate();

    public interface IViewDataModel : INotifyPropertyChanged
    {

        bool IsLangEnActiv { get; }
        bool IsLangFrActiv { get;  }

        bool IsLogFormatJsonActiv { get; }
        bool IsLogFormatXmlActiv { get; }

        bool IsAppModeClient { get; }
        bool IsAppModeServer { get; }


        //string CryptKey { get; set; }
        //string DailyLogPath { get; }
        //string StateLogPath { get; }



        EModeIHM ModeIHMClient { get; }
        EModeIHM ModeIHMServer { get; }
        ESavingFormat SavingFormatJSON {get;}
        ESavingFormat SavingFormatXML {get;}
        ELangCode LangCodeEN {get;}
        ELangCode LangCodeFR {get;}

        IDataModel DataModel { get; }



        ICommand SetAllowExtCommand{ get; }
        ICommand SetPriorityExtCommand{ get; }
        ICommand SetCryptExtCommand{ get; }
        ICommand SetDailyLogPathCommand{ get; }
        ICommand SetStateLogPathCommand{ get; }
        ICommand SetLogFormatCommand{ get; }
        ICommand SetAppModeCommand{ get; }
        ICommand SetJobAppCommand { get; }
        ICommand SetLangCommand{ get; }


        public void FireLangCodeChanged();
        public void FireSavingFormatChanged();
        public void FireAppModeChanged();
    }




}
