using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Input;
using LibEasySave;
using SharpDX.DirectInput;
using SharpDX;

namespace ConsoleUI
{



    public class CommandMng
    {
        private const string ALL_JOB_DENOMINATION = "ALL";
        private const string END_PROMPT = ">";
        private const string VALUE = "VALUE";
        private const string DISABLE_PROMPT = "DISABLE" + END_PROMPT;
        private const string ENABLE_PROMPT = "ENABLE" + END_PROMPT;
        private const string EDIT_PROMPT = "EDIT-" + VALUE + END_PROMPT;
        private const string RUNNING_PROMPT = "RUNNING-" + VALUE + END_PROMPT;

        private bool _maintainLoop = true;

        private string _activPrompt = DISABLE_PROMPT;
        private string _currentString = "";

        private EModeConsole _consoleMode = EModeConsole.Disable;

        protected ModelViewJobs _viewModel = null;

        private List<string> _commandList = null;


        public EModeConsole ModeConsole { get => _consoleMode; set { _consoleMode = value; UpdatePrompt(); } }


        // constuctor
        public CommandMng(ModelViewJobs viewModel)
        {
            _viewModel = viewModel;

            // init listCommand
            _commandList = new List<string>();
            foreach (ECommand command in (ECommand[])Enum.GetValues(typeof(ECommand)))
            {
                _commandList.Add(command.ToString());
            }
        }

        public void Start()
        {

            Thread t = new Thread(ReadKeyCommand);
            t.Start();

            while (_maintainLoop)
            {
                ReadCommand();
                //ReadKey();
            }
        }

        public void Stop()
        {
            _maintainLoop = false;
        }


        public void CheckFireCommand(string strCommand, string param)
        {
            ECommand command;
            if (!Enum.TryParse(strCommand.ToUpper(), out command))
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

                        case ECommand.OK:
                            throw new NotImplementedException();

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


                        case ECommand.RUN:
                            if (param == ALL_JOB_DENOMINATION)
                                throw new NotImplementedException();
                            else
                                commandJob = _viewModel.RunJobCommand;
                            break;
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

            if (commandJob == null)
            {

            }
            else if (commandJob.CanExecute(param))
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
            Console.Beep();
            Console.Write(_activPrompt);

            string str = Console.ReadLine();


            string cmd = null;
            string param = null;
            if (str.Contains(" "))
            {
                cmd = str.Split(' ')[0];
                param = str.Split(' ')[1];
            }
            else
            {
                cmd = str;
            }

            CheckFireCommand(cmd, param);
        }

        private void ReadKey()
        {
            while (true)
            {
                ConsoleKeyInfo consoleKey = Console.ReadKey();
                if (consoleKey.Key == ConsoleKey.Tab)
                {
                    var listTemp = GetPossibleFrom(_currentString);
                    Console.WriteLine();
                    if (listTemp.Count == 1)
                    {
                        _currentString = listTemp[0].ToString();
                        Console.WriteLine( _activPrompt + _currentString );
                    }
                    else
                        Console.Beep();
                }


                else if (consoleKey.KeyChar == '?')
                {
                    var lisTemp = GetPossibleFrom(_currentString);
                    Console.WriteLine("?");
                    if (lisTemp != null)
                        foreach (ECommand command in lisTemp)
                        {
                            Console.WriteLine(_activPrompt + command.ToString());
                        }
                    Console.Write(_activPrompt + _currentString);
                }

                else if (consoleKey.Key == ConsoleKey.Enter)
                {
                    string cmd = null;
                    string param = null;
                    if (_currentString.Contains(" "))
                    {
                        cmd = _currentString.Split(' ')[0];
                        param = _currentString.Split(' ')[1];
                    }
                    else
                    {
                        cmd = _currentString;
                    }

                    CheckFireCommand(cmd, param);
                    _currentString = "";
                    Console.WriteLine();
                    Console.Write(_activPrompt);
                }
                else if (consoleKey.Key == ConsoleKey.Backspace)
                {
                }
                else if (consoleKey.Key == ConsoleKey.LeftArrow)
                {

                }
                else if (consoleKey.Key == ConsoleKey.RightArrow)
                {

                }
                else if (consoleKey.Key == ConsoleKey.Delete)
                {

                }
                else
                {
                    _currentString += consoleKey.KeyChar;
                }
                //else if (consoleKey.Key == ConsoleKey.DownArrow)
                //{

                //}
                //else if (consoleKey.Key == ConsoleKey.UpArrow)
                //{

                //}
                //else if (consoleKey.Key == ConsoleKey.Escape)
                //{

                //}
            }
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
                    _activPrompt = EDIT_PROMPT.Replace(VALUE, _viewModel.ActivJob);
                    break;

                case EModeConsole.Running:
                    _activPrompt = RUNNING_PROMPT.Replace(VALUE, _viewModel.ActivJob);
                    break;


                case EModeConsole.Disable:
                    _activPrompt = DISABLE_PROMPT;
                    break;
            }
        }

        private List<ECommand> GetPossibleFrom(string str)
        {
            List<ECommand> listPossibilities = new List<ECommand>((ECommand[])Enum.GetValues(typeof(ECommand)));
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

        //private string GetStringConsole()
        //{
        //    using(StreamReader strmR = new StreamReader())
        //    {
        //        Console.SetIn(strmR);
        //        return strmR.ReadLine();     
        //    }
        //}

        private void ReadKeyCommand()
        {
            
            while(_maintainLoop)
            {
                KeyboardState stateK = new KeyboardState();
                if (stateK.IsPressed(Key.Tab))
                {
                    ;
                }
                Thread.Sleep(10);

            }
        }

    }

    public enum EModeConsole
    {
        Enable,
        Edit,
        Running,
        Disable,
    }



}
