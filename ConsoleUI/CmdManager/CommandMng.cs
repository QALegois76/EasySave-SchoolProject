using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Input;
using LibEasySave;

namespace ConsoleUI
{
    /// <summary>
    /// handle console to get command and write message
    /// </summary>
    public class CommandMng
    {
        #region WelcomeString
        private const string WELCOME = @"


                             _______                       ______                      
                            | ______)                     / _____)                       
                            | |____   _____   ___  _   _ ( (____   _____  _   _  _____  
                            |  ___)  (____ | /___)| | | | \____ \ (____ || | | || ___ |  
                            | |_____ / ___ ||___ || |_| | _____) )/ ___ | \ V / | ____|  
                            |_______)\_____|(___/  \__  |(______/ \_____|  \_/  |_____)  
                                                  (____/                               

                                                 ___                          
                                                / __)                         
                                              _| |__   ____   ___   ____      
                                             (_   __) / ___) / _ \ |    \     
                                               | |   | |    | |_| || | | |    
                                               |_|   |_|     \___/ |_|_|_|    

                              
                                  ______                  ______            ___        
                                 (_____ \                / _____)          / __)   _   
                                  _____) )  ____   ___  ( (____    ___   _| |__  _| |_ 
                                 |  ____/  / ___) / _ \  \____ \  / _ \ (_   __)(_   _)
                                 | |      | |    | |_| | _____) )| |_| |  | |     | |_ 
                                 |_|      |_|     \___/ (______/  \___/   |_|      \__)
                                                                                       
                              
";
        #endregion

        #region VARIABLES
        private const string END_PROMPT = ">";
        private const string HELP_CALL = "?";
        private const string VALUE = "VALUE";
        private const string DISABLE_PROMPT = "DISABLE" + END_PROMPT;
        private const string ENABLE_PROMPT = "ENABLE" + END_PROMPT;
        private const string EDIT_PROMPT = "EDIT-" + VALUE + END_PROMPT;
        private const string RUNNING_PROMPT = "RUNNING-" + VALUE + END_PROMPT;
        private const string LIB_SAVE_ANSWER = "EasySaveV1.0" + END_PROMPT;
        private const string INFO = "INFO";
        private const string ERROR = "ERROR";
        private const string SPACE_COMMAND = " ";
        private const string NEW_LINE_COMMAND = "\n";
        private const string QUOTE_COMMAND = "\"";
        private const char COMMENT_COMMAND = '#';

        private readonly ConsoleColor DEFAULT_COLOR = ConsoleColor.White;
        private readonly ConsoleColor INFO_COLOR = ConsoleColor.Blue;
        private readonly ConsoleColor WARNIN_COLOR = ConsoleColor.Yellow;
        private readonly ConsoleColor ERRROR_COLOR = ConsoleColor.Red;
        private readonly ConsoleColor SUCESS_COLOR = ConsoleColor.Green;

        private bool _maintainLoop = true;

        private string _activPrompt = DISABLE_PROMPT;

        private EModeConsole _consoleMode = EModeConsole.Disable;

        protected IModelViewJob _viewModel = null;

        private Dictionary<EModeConsole, List<string>> _commandList = null;


        //public EModeConsole ModeConsole { get => _consoleMode; set { _consoleMode = value; UpdatePrompt(); } }
        #endregion

        // constuctor
        public CommandMng(IModelViewJob viewModel)
        {
            _viewModel = viewModel;

            _viewModel.OnPopingMsgInfo -= ViewModel_OnPopingMsg;
            _viewModel.OnPopingMsgInfo += ViewModel_OnPopingMsg;

            _viewModel.OnPopingMsgError -= ViewModel_OnPopingMsgError;
            _viewModel.OnPopingMsgError += ViewModel_OnPopingMsgError;

            _viewModel.OnEditing -= Job_OnEditing;
            _viewModel.OnEditing += Job_OnEditing;

            // init listCommand
            _commandList = new Dictionary<EModeConsole, List<string>>();
            _commandList.Add(EModeConsole.Disable, new List<string>());
            _commandList.Add(EModeConsole.Enable, new List<string>());
            _commandList.Add(EModeConsole.Edit, new List<string>());
            _commandList.Add(EModeConsole.Running, new List<string>());

            // disable mode
            _commandList[EModeConsole.Disable].Add(ECommand.OK.ToString());
            _commandList[EModeConsole.Disable].Add(ECommand.EXIT.ToString());

            // enable mode
            _commandList[EModeConsole.Enable].Add(ECommand.ADD.ToString());
            _commandList[EModeConsole.Enable].Add(ECommand.REMOVE.ToString());
            _commandList[EModeConsole.Enable].Add(ECommand.EDIT.ToString());
            _commandList[EModeConsole.Enable].Add(ECommand.EXIT.ToString());
            _commandList[EModeConsole.Enable].Add(ECommand.RUN.ToString());
            _commandList[EModeConsole.Enable].Add(ECommand.GET_ALL_NAME.ToString());
            _commandList[EModeConsole.Enable].Add(ECommand.CHANGE_LANG.ToString());

            _commandList[EModeConsole.Edit].Add(ECommand.GET_REP_DEST.ToString());
            _commandList[EModeConsole.Edit].Add(ECommand.SET_REP_DEST.ToString());
            _commandList[EModeConsole.Edit].Add(ECommand.GET_REP_SRC.ToString());
            _commandList[EModeConsole.Edit].Add(ECommand.SET_REP_SRC.ToString());
            _commandList[EModeConsole.Edit].Add(ECommand.GET_SAVING_MODE.ToString());
            _commandList[EModeConsole.Edit].Add(ECommand.SET_SAVING_MODE.ToString());
            _commandList[EModeConsole.Edit].Add(ECommand.EXIT.ToString());
            _commandList[EModeConsole.Edit].Add(ECommand.RENAME.ToString());

            // runing mode
            _commandList[EModeConsole.Running].Add(ECommand.EXIT.ToString());
            _commandList[EModeConsole.Running].Add(ECommand.CANCEL.ToString());
        }



        #region METHOD


        #region from event
        private void Job_OnEditing(object sender, EventArgs e)
        {
            _consoleMode = EModeConsole.Edit;
            UpdatePrompt();
        }
        private void ViewModel_OnPopingMsg(object sender, MsgEventArgs eMsg)=> PopMsg(eMsg.Msg, ETypeMsg.Info,INFO);
        private void ViewModel_OnPopingMsgError(object sender, MsgEventArgs eMsg) => PopMsg(eMsg.Msg, ETypeMsg.Error, ERROR);
        #endregion


        #region public
        public void Start()
        {
            Console.WriteLine(WELCOME);
            while (_maintainLoop)
            {
                ReadCommand();
            }
        }

        public void ExecuteCommand(string strCommand, string param)
        {
            ECommand command;
            ICommand commandJob = null;
            if (strCommand[0] == COMMENT_COMMAND)
                return;

            if (Enum.TryParse(strCommand?.ToUpper(), out command))
            {
                switch (_consoleMode)
                {
                    // ENABLE MODE => possible commands
                    case EModeConsole.Enable:
                        switch (command)
                        {
                            case ECommand.ADD:
                                commandJob = _viewModel.AddJobCommand;
                                break;

                            case ECommand.REMOVE:
                                commandJob = _viewModel.RemoveJobCommand;
                                break;

                            case ECommand.EDIT:
                                commandJob = _viewModel.EditJobCommand;
                                break;

                            case ECommand.EXIT:
                                Exit();
                                return;

                            case ECommand.RUN:
                                commandJob = _viewModel.RunJobCommand;
                                break;

                            case ECommand.GET_ALL_NAME:
                                commandJob = _viewModel.GetAllNameJobCommand;
                                break;
                        }
                        break;


                    // EDIT MODE => possible commands
                    case EModeConsole.Edit:
                        switch (command)
                        {
                            case ECommand.RENAME:
                                commandJob = _viewModel.RenameJobCommand;
                                break;

                            case ECommand.SET_REP_SRC:
                                commandJob = _viewModel.SetSrcRepJobCommand;
                                break;

                            case ECommand.GET_REP_SRC:
                                commandJob = _viewModel.GetSrcRepJobCommand;
                                break;

                            case ECommand.SET_REP_DEST:
                                commandJob = _viewModel.SetDestRepJobCommand;
                                break;

                            case ECommand.GET_REP_DEST:
                                commandJob = _viewModel.GetDestRepJobCommand;
                                break;

                            case ECommand.SET_SAVING_MODE:
                                commandJob = _viewModel.SetSavingModeJobCommand;
                                break;

                            case ECommand.GET_SAVING_MODE:
                                commandJob = _viewModel.GetSavingModeJobCommand;
                                break;

                            case ECommand.EXIT:
                                _consoleMode = EModeConsole.Enable;
                                UpdatePrompt();
                                return;
                        }
                        break;


                    // RUNNIG MODE => possible commands
                    case EModeConsole.Running:
                        switch (command)
                        {

                            case ECommand.EXIT:
                                Exit();
                                return;

                            case ECommand.CANCEL:
                                throw new NotImplementedException();
                        }
                        break;


                    case EModeConsole.Disable:
                        switch (command)
                        {
                            case ECommand.OK:
                                _consoleMode = EModeConsole.Enable;
                                UpdatePrompt();
                                return;

                            case ECommand.EXIT:
                                Exit();
                                return;
                        }
                        break;

                    default:
                        break;
                }


                if (commandJob != null)
                {
                    commandJob.Execute(param);
                }
                else
                {
                    _viewModel.CommandUnknownJobCommand.Execute(true);
                }


            }
            else
                _viewModel.CommandUnknownJobCommand.Execute(false);
        }
        
        
        public void ExecuteCommandV2(string strCommand, string param)
        {
            ECommand command;
            if (strCommand[0] == COMMENT_COMMAND)
                return;

            if (!Enum.TryParse(strCommand?.ToUpper(), out command))
            {
                _viewModel.CommandUnknownJobCommand.Execute(false);
            }

            switch (_consoleMode)
            {
                // ENABLE MODE => possible commands
                case EModeConsole.Enable:
                    switch (command)
                    {
                        case ECommand.ADD:
                        case ECommand.REMOVE:
                        case ECommand.EDIT:
                        case ECommand.RUN:
                        case ECommand.GET_ALL_NAME:
                        case ECommand.CHANGE_LANG:
                            _viewModel.InterpreteJobCommand(strCommand, param);
                            break;

                        case ECommand.EXIT:
                            Exit();
                            break;
                    }
                    break;


                // EDIT MODE => possible commands
                case EModeConsole.Edit:
                    switch (command)
                    {
                        case ECommand.RENAME:
                        case ECommand.SET_REP_SRC:
                        case ECommand.GET_REP_SRC:
                        case ECommand.SET_REP_DEST:
                        case ECommand.GET_REP_DEST:
                        case ECommand.SET_SAVING_MODE:
                        case ECommand.GET_SAVING_MODE:
                            _viewModel.InterpreteJobCommand(strCommand, param);
                            break;

                        case ECommand.EXIT:
                            _consoleMode = EModeConsole.Enable;
                            UpdatePrompt();
                            return;
                    }
                    break;


                // RUNNIG MODE => possible commands
                case EModeConsole.Running:
                    switch (command)
                    {

                        case ECommand.EXIT:
                            Exit();
                            return;

                        case ECommand.CANCEL:
                            throw new NotImplementedException();
                    }
                    break;


                case EModeConsole.Disable:
                    switch (command)
                    {
                        case ECommand.OK:
                            _consoleMode = EModeConsole.Enable;
                            UpdatePrompt();
                            return;

                        case ECommand.EXIT:
                            Exit();
                            return;
                    }
                    break;
            }


        }


        #endregion


        #region utility
        private void Exit()
        {
            _viewModel.ExitJobCommand.Execute(null);
            _maintainLoop = false;
        }

        private void ReadCommand()
        {
            UpdatePrompt();
            Console.Write(_activPrompt);

            string str = Console.ReadLine();

            string cmd = null;
            string param = null;
            if (string.IsNullOrWhiteSpace(str))
                return;
            if (str.EndsWith(HELP_CALL) && !str.Contains(' '))
            {
                var list = GetPossibleFrom("");
                if (list?.Count > 0)
                {
                    foreach (var item in list)
                    {
                        PopMsg(item,ETypeMsg.Info,INFO);
                    }
                }    
            }
            else
            {
                if (str.Contains(" "))
                {
                    var splitStr = str.Split(' ');
                    cmd = str.Split(' ')[0];

                    for (int i = 1; i < splitStr.Length; i++)
                    {
                        param +=splitStr[i]+" ";

                    }
                    param = param.Trim();

                    if (param.StartsWith('"') && param.EndsWith('"'))
                    {
                        param = param.Substring(1, param.Length - 2);
                    }
                  
                }
                else
                {
                    cmd = str;
                }
                ExecuteCommand(cmd, param);
            }


        }


        public void PopMsg(string msg , ETypeMsg typeMsg = ETypeMsg.Info , string prompt = null)
        {
            ConsoleColor consoleColor;
            switch (typeMsg)
            {
                default:
                case ETypeMsg.Info:
                    consoleColor = INFO_COLOR;
                    break;

                case ETypeMsg.Warning:
                    consoleColor = WARNIN_COLOR;
                    break;
                case ETypeMsg.Error:
                    consoleColor = ERRROR_COLOR;
                    break;

                case ETypeMsg.Sucess:
                    consoleColor = SUCESS_COLOR;
                    break;
            }
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(((prompt == null) ? _activPrompt : (prompt+END_PROMPT)) + msg);
            Console.ForegroundColor = DEFAULT_COLOR;
        }

        private void UpdatePrompt()
        {
            switch (_consoleMode)
            {
                default:
                case EModeConsole.Enable:
                    _activPrompt = ENABLE_PROMPT;
                    break;


                case EModeConsole.Edit:
                    _activPrompt = EDIT_PROMPT.Replace(VALUE, _viewModel.Model.BaseJober[_viewModel.EditingJob].Job.Name);
                    break;

                case EModeConsole.Running:
                    //_activPrompt = RUNNING_PROMPT.Replace(VALUE,_viewModel.EditingJobName);
                    break;


                case EModeConsole.Disable:
                    _activPrompt = DISABLE_PROMPT;
                    break;
            }
        }

        private List<string> GetPossibleFrom(string str)
        {
            List<string> listPossibilities = _commandList[_consoleMode];
            for (int iLetter = 0; iLetter < str.Length; iLetter++)
            {
                for (int i = 0; i < listPossibilities.Count; i++)
                {
                    if (listPossibilities[i].ToString()[iLetter] != str[iLetter])
                    {
                        listPossibilities.RemoveAt(i);
                        i--;
                    }

                    if (listPossibilities.Count == 0)
                        return null;
                }
            }
            return listPossibilities;
        }
        #endregion


        #endregion

    }


    public static class JobCommandInterpreter
    {
        private const string SPACE_COMMAND = " ";
        private const string NEW_LINE_COMMAND = "\n";
        private const string QUOTE_COMMAND = "\"";
        private const char COMMENT_COMMAND = '#';

        public static void InterpreteJobCommand(this IModelViewJob viewModel, string strCommand, string param)
        {
            ECommand command;
            ICommand commandJob = null;

            if (strCommand[0] == COMMENT_COMMAND)
                return;

            if (!Enum.TryParse(strCommand?.ToUpper(), out command))
            {
                viewModel.CommandUnknownJobCommand.Execute(false);
                return;
            }

            switch (command)
            {

                case ECommand.ADD:
                    commandJob = viewModel.AddJobCommand;
                    break;

                case ECommand.REMOVE:
                    commandJob = viewModel.RemoveJobCommand;
                    break;

                case ECommand.EDIT:
                    commandJob = viewModel.EditJobCommand;
                    break;

                case ECommand.RUN:
                    commandJob = viewModel.RunJobCommand;
                    break;

                case ECommand.GET_ALL_NAME:
                    commandJob = viewModel.GetAllNameJobCommand;
                    break;

                case ECommand.RENAME:
                    commandJob = viewModel.RenameJobCommand;
                    break;

                case ECommand.SET_REP_SRC:
                    commandJob = viewModel.SetSrcRepJobCommand;
                    break;

                case ECommand.GET_REP_SRC:
                    commandJob = viewModel.GetSrcRepJobCommand;
                    break;

                case ECommand.SET_REP_DEST:
                    commandJob = viewModel.SetDestRepJobCommand;
                    break;

                case ECommand.GET_REP_DEST:
                    commandJob = viewModel.GetDestRepJobCommand;
                    break;

                case ECommand.SET_SAVING_MODE:
                    commandJob = viewModel.SetSavingModeJobCommand;
                    break;

                case ECommand.GET_SAVING_MODE:
                    commandJob = viewModel.GetSavingModeJobCommand;
                    break;

                default:
                    commandJob = viewModel.CommandUnknownJobCommand;
                    break;
            }
            commandJob.Execute(param);
        }


        public static string GetExecutableCommand(IJob job)
        {
            StringBuilder output = new StringBuilder("");

            output.AppendLine(ECommand.ADD + SPACE_COMMAND + job.Name);
            output.AppendLine(ECommand.EDIT + SPACE_COMMAND + job.Guid);
            output.AppendLine(ECommand.SET_REP_SRC + SPACE_COMMAND + job.Name);
            output.AppendLine(ECommand.SET_REP_DEST + SPACE_COMMAND + job.Name);
            output.AppendLine(ECommand.SET_SAVING_MODE + SPACE_COMMAND + job.Name);

            return output.ToString();
        }
    }

}
