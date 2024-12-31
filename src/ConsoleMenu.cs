namespace UFOX
{
    /*
    *   This class is responsible for parsing the args
    *   and invoking the assembler.
    *
    *   List of commands
    *   Help - Takes other commands but using another command shows help for that command
    *   Build - Takes -t (target) -o (output file) -s (source)
    *   Version - Takes no other arguments
    */
    internal class ConsoleMenu
    {
        private const string version = "1.0.0";
        internal ConsoleMenu(string[] args)
        {
            // Check a command has been given to the program
            if (args.Length < 1)
            {
                ShowVersion();
                Environment.Exit(0);
            }

            // Execute the command
            switch (args[0])
            {
                case "version":
                    ShowVersion();
                    break;

                case "help":
                    ShowHelp(args);
                    break;

                case "build":
                    Build(args);
                    break;

                default:
                    UnknownCommand(args[0]);
                    break;
            }
        }

        private static void Build(string[] args)
        {
            // build -t <target> -s <source> -o <output>
            // Check the number of args is at least 7
            if (args.Length < 7)
            {
                Console.WriteLine("Error: Expected -t <target> -s <source> -o <output>");
                Environment.Exit(-1);
            }

            // Find the index of -t
            var index = args.ToList().IndexOf("-t");
            if (index == -1)
            {
                Console.WriteLine("Error: '-t' argument not found.");
                Environment.Exit(-1);
            }

            // Get the arg after corresponding to the target
            var target = args[index + 1];

            // Find the index of -s
            index = args.ToList().IndexOf("-s");
            if (index == -1)
            {
                Console.WriteLine("Error: '-s' argument not found.");
                Environment.Exit(-1);
            }

            // Get the arg after corresponding to the source
            var source = args[index + 1];

            // Try and convert the source relative path to an absolute path
            try
            {
                source = Path.GetFullPath(source);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid path {e}");
                Environment.Exit(-1);
            }

            // Check the source file exists
            if (!File.Exists(source))
            {
                Console.WriteLine("File does not exist!");
                Environment.Exit(-1);
            }

            // Find the index of -o
            index = args.ToList().IndexOf("-o");
            if (index == -1)
            {
                Console.WriteLine("Error: '-o' argument not found.");
                Environment.Exit(-1);
            }

            // Get the arg after corresponding to the source
            var output = args[index + 1];

            // Try and convert the source relative path to an absolute path
            try
            {
                output = Path.GetFullPath(output);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid path {e}");
                Environment.Exit(-1);
            }

            // Attempt to create a temporary file to ensure it is possible
            try
            {
                var dir = Path.GetDirectoryName(output) ?? throw new Exception("Invalid source dir");
                Directory.CreateDirectory(dir);
                File.Create(output);
                
            } catch (Exception e)
            {
                throw new Exception($"Could not create placeholder output file: {e}");
            }

            Console.WriteLine($"Attempting to compile {source} to {target} and write contents to {output}");
        }

        private static void ShowHelp(string[] args)
        {
            // Check if second arg is provide to get help for
            if (args.Length < 2)
            {
                Console.WriteLine("UFOX Help page.");
                Console.WriteLine("help - shows commands or usage for a specific command.");
                Console.WriteLine("build - build assembly file.");
                Console.WriteLine("version - show the version of the assembler.");
            }
            else
            {
                switch (args[1])
                {
                    case "help":
                        Console.WriteLine("help to show a list of commands.");
                        Console.WriteLine("help help to show this.");
                        Console.WriteLine("help <command> to show usage for that command.");
                        break;

                    case "version":
                        Console.WriteLine("Display the version.");
                        break;

                    case "build":
                        Console.WriteLine("Build assembly file into binary.");
                        Console.WriteLine("Required flags:");
                        Console.WriteLine("-t <target>");
                        Console.WriteLine("-s <source>");
                        Console.WriteLine("-o <output>");
                        Console.WriteLine("Supported targets:");
                        Console.WriteLine("- visofox16 (rom)");
                        Console.WriteLine("- tokens (tokens from the tokeniser)");
                        Console.WriteLine("- preprocessed (post preprocessed code)");
                        break;
                }
            }
        }

        private static void ShowVersion()
        {
            Console.WriteLine($"UFOX version {version}.");
        }

        private static void UnknownCommand(string command)
        {
            Console.WriteLine($"{command} is not a command.");
        }
    }
}