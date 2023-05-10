using System;
using System.Collections.Generic;
using Spectre.Console;

namespace LoginSaver
{
    internal class Program
    {

        public static List<string> Platforms = new List<string>();

        public static void Main(string[] args)
        {
            RefreshPlatforms();
            
            Console.Clear();
            Console.WindowWidth = 100;
            Console.WindowHeight = 25;

            while (true)
            {
                ClearConsole();

                string platform = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose a platform")
                        .PageSize(10)
                        .WrapAround(true)
                        .MoreChoicesText("...")
                        .AddChoices(Platforms)
                        .AddChoices("[green]Add platform[/]", "[red]Exit[/]"));

                if (platform == "[red]Exit[/]") Environment.Exit(0);
            
                string action;
                if (platform == "[green]Add platform[/]")
                {
                    action = "Write";
                }
                else
                {
                    action = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Choose an action")
                            .PageSize(10)
                            .WrapAround(true)
                            .AddChoices(new[] {
                                "Read", "Write", "[red]Remove[/]", "[yellow]Back[/]"
                            }));
                    AnsiConsole.Write(new Markup($"[yellow]{platform}[/] to be ---\n"));
                }

                string actionWord = "";
                switch (action)
                {
                    case "Write":
                    {
                        actionWord = "written";
                        break;
                    }
                    case "Read":
                    {
                        actionWord = "read";
                        break;
                    }
                    case "[red]Remove[/]":
                    {
                        actionWord = "removed";
                        break;
                    }
                    case "[yellow]Back[/]":
                    {
                        continue;
                    }
                }
            
                ClearConsole();
                if (platform != "[green]Add platform[/]") AnsiConsole.Write(new Markup($"[yellow]{platform}[/] to be [yellow]{actionWord}[/]\n"));

                switch (action)
                {
                    case "Read":
                    {
                        ClearConsole();
                        try
                        {
                            var panel = new Panel($"Username: [blue]{LauncherHelper.GetUsername(platform)}[/]\nPassword: [blue]{LauncherHelper.GetPassword(platform)}[/]");
                            panel.Header = new PanelHeader(platform);
                            panel.Header.Justification = Justify.Center;
                            panel.Border = BoxBorder.Rounded;
                            panel.Padding = new Padding(2, 1, 2, 1);
                            AnsiConsole.Write(panel);
                        }
                        catch (KeyNotFoundException)
                        {
                            AnsiConsole.Write(new Markup($"[yellow]{platform}[/] doesn't contain anything!"));
                        }
                        AnsiConsole.Write($"\nPress anything to continue...");
                        Console.ReadKey();
                        break;
                    }
                    case "Write":
                    {
                        Console.WriteLine();

                        if (platform == "[green]Add platform[/]")
                        {
                            platform = AnsiConsole.Prompt(
                                new TextPrompt<string>("[blue]Platform:[/]")
                                    .AllowEmpty());
                        }
                    
                        string username = AnsiConsole.Prompt(
                            new TextPrompt<string>("[blue]Username:[/]")
                                .AllowEmpty());

                        string password = AnsiConsole.Prompt(
                            new TextPrompt<string>("[blue]Password:[/]")
                                .AllowEmpty());

                        LauncherHelper.SetUsername(platform, username);
                        LauncherHelper.SetPassword(platform, password);
                        
                        AnsiConsole.Write(new Markup($"\n[yellow]{platform}[/] has been [green]successfully edited![/]"));
                        AnsiConsole.Write($"\nPress anything to continue...");
                        RefreshPlatforms();
                        Console.ReadKey();
                        break;
                    }
                    case "[red]Remove[/]":
                    {
                        ClearConsole();
                        LauncherHelper.Remove(platform);
                        AnsiConsole.Write(new Markup($"[yellow]{platform}[/] has been [red]removed![/]"));
                        AnsiConsole.Write($"\nPress anything to continue...");
                        RefreshPlatforms();
                        Console.ReadKey();
                        break;
                    }
                }
            }
        }

        private static void ClearConsole()
        {
            Console.Clear();
            Rule rule = new Rule("[bold yellow]Login Saver v420.69[/]\n");
            rule.Style = Style.Parse("yellow");
            AnsiConsole.Write(rule);
        }

        private static void RefreshPlatforms()
        {
            Platforms.Clear();
            foreach (var credential in LauncherHelper.CredentialsMap)
            {
                Platforms.Add(credential.Key);
            }
        }
    }
}