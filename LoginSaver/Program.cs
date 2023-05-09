using System;
using Spectre.Console;

namespace LoginSaver
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Clear();
            Console.WindowWidth = 100;
            Console.WindowHeight = 25;
            AnsiConsole.Write(new Markup("[bold yellow]Login Saver v420.69[/]"));
            
            var fruit = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a platform")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                    .AddChoices(new[] {
                        "Apple", "Apricot", "Avocado", 
                        "Banana", "Blackcurrant", "Blueberry",
                        "Cherry", "Cloudberry", "Cocunut",
                    }));

// Echo the fruit back to the terminal
            AnsiConsole.WriteLine($"I agree. {fruit} is tasty!");
        }
    }
}