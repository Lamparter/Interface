using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Interface
{
    public class CommandLineInterface : ICommandLineInterface
    {
        private readonly string _cliName;
        private readonly string _helpFilePath;
        private readonly string _firstRunPrompt;
        private readonly string _orgName;
        private readonly Dictionary<string, Action> _commands;
        private readonly FirstRunManager _firstRunManager;

        public CommandLineInterface(string name, string orgName, string helpFilePath, FirstRunManager firstRunManager, string? copyrightExpression = "", bool? doesHaveFirstRunPrompt = true, string? startPrompt = "", string firstRunPrompt = $"Welcome to the CLI! It's good to see you.")
        {
            _helpFilePath = helpFilePath;
            _cliName = name;
            _orgName = orgName;
            _firstRunPrompt = firstRunPrompt;
            _commands = new Dictionary<string, Action>();
            _firstRunManager = firstRunManager;

            if (_firstRunManager.IsFirstRun())
            {
                FirstRunPrompt();
                _firstRunManager.MarkFirstRun();
            }
        }

        public string[] GetArguments()
        {
            return Environment.GetCommandLineArgs();
        }

        public void PrintHelp()
        {
            if (File.Exists(_helpFilePath))
            {
                var helpText = File.ReadAllText(_helpFilePath);
                var helpData = JsonSerializer.Deserialize<Dictionary<string, string>>(helpText);
                if (helpData != null && helpData.ContainsKey("help"))
                {
                    Console.WriteLine(helpData["help"]);
                }
                else
                {
                    throw new KeyNotFoundException("Help data not found in the file.");
                }
            }
            else
            {
                throw new FileNotFoundException("Help file not found.");
            }
        }

        public void Run()
        {
            var args = GetArguments();
            if (args.Length > 1 && _commands.ContainsKey(args[1]))
            {
                _commands[args[1]].Invoke();
            }
            else
            {
                PrintHelp();
            }
        }

        public void AddCommand(string command, Action action)
        {
            if (!_commands.ContainsKey(command))
            {
                _commands.Add(command, action);
            }
        }

        public void FirstRunPrompt()
        {
            Console.WriteLine(_firstRunPrompt);
        }
    }
}
