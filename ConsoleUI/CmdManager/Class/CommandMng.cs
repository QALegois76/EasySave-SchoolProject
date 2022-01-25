using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Input;
using LibEasySave;

namespace ConsoleUI
{



    public class CommandMng
    {
        private const string ALL_JOB_DENOMINATION = "ALL";
        private const string END_PROMPT = ">";
        private const string HELP_CALL = "?";
        private const string VALUE = "VALUE";
        private const string DISABLE_PROMPT = "DISABLE" + END_PROMPT;
        private const string ENABLE_PROMPT = "ENABLE" + END_PROMPT;
        private const string EDIT_PROMPT = "EDIT-" + VALUE + END_PROMPT;
        private const string RUNNING_PROMPT = "RUNNING-" + VALUE + END_PROMPT;
        private const string LIB_SAVE_ANSWER = "EasySaveV1.0" + END_PROMPT;

        private readonly ConsoleColor DEFAULT_COLOR = ConsoleColor.White;
        private readonly ConsoleColor INFO_COLOR = ConsoleColor.White;
        private readonly ConsoleColor WARNIN_COLOR = ConsoleColor.Yellow;
        private readonly ConsoleColor ERRROR_COLOR = ConsoleColor.Red;
        private readonly ConsoleColor SUCESS_COLOR = ConsoleColor.Green;

        private bool _maintainLoop = true;

        private string _activPrompt = DISABLE_PROMPT;

        private EModeConsole _consoleMode = EModeConsole.Disable;

        protected IModelViewJob _viewModel = null;

        private Dictionary<EModeConsole, List<string>> _commandList = null;


        public EModeConsole ModeConsole { get => _consoleMode; set { _consoleMode = value; UpdatePrompt(); } }


        // constuctor
        public CommandMng(ModelViewJobs viewModel)
        {
            _viewModel = viewModel;

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

        public void Start()
        {
            while (_maintainLoop)
            {
                ReadCommand();
            }
        }

        public void Stop()
        {
            _maintainLoop = false;
        }




        public void CheckFireCommand(string strCommand, string param)
        {
            ECommand command;
            if (!Enum.TryParse(strCommand?.ToUpper(), out command))
                return;

            ICommand commandJob = null;


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
                            throw new NotImplementedException();
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
                            break;

                        case ECommand.EXIT:
                            Exit();
                            return;
                    }
                    break;

                default:
                    break;
            }

            if ((bool)commandJob?.CanExecute(param))
            {
                commandJob.Execute(param);
            }
        }

        private void Exit()
        {
            Stop();
        }

        private void ReadCommand()
        {
            Console.Write(_activPrompt);

            string str = Console.ReadLine();

            string cmd = null;
            string param = null;

            if (str.EndsWith(HELP_CALL))
            {
                var list = GetPossibleFrom("");
                if (list?.Count > 0)
                {
                    foreach (var item in list)
                    {
                        Write(item);
                    }
                }    
            }
            else
            {
                if (str.Contains(" "))
                {
                    cmd = str.Split(' ')[0];
                    param = str.Split(' ')[1];
                }
                else
                {
                    cmd = str;
                }
            }


            CheckFireCommand(cmd, param);
        }


        #region utility
        private void Write(string msg)
        {
            Console.WriteLine(_activPrompt+msg);
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

            Console.WriteLine((prompt == null) ? _activPrompt : prompt + END_PROMPT, consoleColor);
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
                    _activPrompt = EDIT_PROMPT.Replace(VALUE, _viewModel.ActivName);
                    break;

                case EModeConsole.Running:
                    _activPrompt = RUNNING_PROMPT.Replace(VALUE, _viewModel.ActivName);
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


        #region disable
        //private void ReadKey()
        //{
        //    while (true)
        //    {
        //        ConsoleKeyInfo consoleKey = Console.ReadKey();


        //        if (consoleKey.Key == ConsoleKey.Tab)
        //        {
        //            var listTemp = GetPossibleFrom(_currentString);
        //            Console.WriteLine();
        //            if (listTemp.Count == 1)
        //            {
        //                _currentString = listTemp[0].ToString();
        //                Console.WriteLine(_activPrompt + _currentString);
        //            }
        //            else
        //                Console.Beep();
        //        }


        //        else if (consoleKey.KeyChar == '?')
        //        {
        //            var lisTemp = GetPossibleFrom(_currentString);
        //            Console.WriteLine("?");
        //            if (lisTemp != null)
        //                //foreach (ECommand command in lisTemp)
        //                //{
        //                //    Console.WriteLine(_activPrompt + command.ToString());
        //                //}
        //            Console.Write(_activPrompt + _currentString);
        //        }

        //        else if (consoleKey.Key == ConsoleKey.Enter)
        //        {
        //            string cmd = null;
        //            string param = null;
        //            if (_currentString.Contains(" "))
        //            {
        //                cmd = _currentString.Split(' ')[0];
        //                param = _currentString.Split(' ')[1];
        //            }
        //            else
        //            {
        //                cmd = _currentString;
        //            }

        //            CheckFireCommand(cmd, param);
        //            _currentString = "";
        //            Console.WriteLine();
        //            Console.Write(_activPrompt);
        //        }
        //        else if (consoleKey.Key == ConsoleKey.Backspace)
        //        {
        //            _currentString = _currentString.Substring(0, _currentString.Length - 1);
        //        }
        //        else if (consoleKey.Key == ConsoleKey.LeftArrow)
        //        {

        //        }
        //        else if (consoleKey.Key == ConsoleKey.RightArrow)
        //        {

        //        }
        //        else if (consoleKey.Key == ConsoleKey.Delete)
        //        {

        //        }
        //        else
        //        {
        //            _currentString += consoleKey.KeyChar;
        //        }
        //        //else if (consoleKey.Key == ConsoleKey.DownArrow)
        //        //{

        //        //}
        //        //else if (consoleKey.Key == ConsoleKey.UpArrow)
        //        //{

        //        //}
        //        //else if (consoleKey.Key == ConsoleKey.Escape)
        //        //{

        //        //}
        //    }
        //}

        //private void ReadKeyCommand()
        //{
        //    Keyboard keyboard = new Keyboard(new DirectInput());
        //    keyboard.Properties.BufferSize = 128;
        //    keyboard.Acquire();


        //    while (_maintainLoop)
        //    {

        //        keyboard.Poll();
        //        foreach (var item in keyboard.GetBufferedData())
        //        {
        //            if (item.IsReleased)
        //            if (item.Key == Key.Tab)
        //            {
        //                var list = GetPossibleFrom(_currentString);
        //                if (list?.Count == 1)
        //                {
        //                    Console.WriteLine();
        //                    Console.Write(_activPrompt + list[0]);
        //                }
        //            }
        //            else if (((char)item.Key) == '?')
        //            {
        //                ;
        //            }
        //            else if (item.Key == Key.Escape || item.Key == Key.Return)
        //                _currentString = "";
        //            else
        //            {
        //                _currentString += item.Key.ToString();
        //            }
        //        }    
        //        Thread.Sleep(100);

        //    }
        //}
        #endregion

        #endregion

    }

    public enum EModeConsole
    {
        Enable,
        Edit,
        Running,
        Disable,
    }

    public enum ETypeMsg
    {
        Info,
        Warning,
        Error,
        Sucess
    }


}
