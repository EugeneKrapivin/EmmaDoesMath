using EmmaDoesMath;
using Spectre.Console;
using System.Diagnostics;

var ofekImg = new CanvasImage("ofek.png");
ofekImg.MaxWidth = 480;

AnsiConsole.Write(ofekImg);

var range = AnsiConsole.Prompt(new SelectionPrompt<int>()
    .Title("Lets choose a range of numbers")
    .AddChoices(10, 20, 30, 40, 50, 60, 70, 80, 90, 100));

var operators = AnsiConsole.Prompt(new MultiSelectionPrompt<Operator>()
    .Title("Lets chose with operators we'll play with")
    .AddChoices(Operator.Add, Operator.Sub, Operator.Mul, Operator.Div)
    .Required());

var timespan = TimeSpan.FromSeconds(10);
var cts = new CancellationTokenSource();
cts.CancelAfter(timespan);

var ct = cts.Token;

var rng = Random.Shared;
var totalRounds = 0;
var totalCorrect = 0;
var starttime = DateTime.UtcNow;
var endtime = starttime + timespan;

while (!ct.IsCancellationRequested)
{
    totalRounds++;
    var a = rng.Next(0, range);
    var b = rng.Next(0, (range + 1) - a);

    (a,b) = a > b ? (a,b) : (b,a);
    var op = rng.GetItems(operators.ToArray(), 1)[0];
    var expected = op switch
    {
        Operator.Add => a + b,
        Operator.Sub => a - b,
        Operator.Div => (double)a / b,
        Operator.Mul => a * b,
        _ => throw new NotImplementedException(),
    };

    double ans;
    //do
    //{
    AnsiConsole.MarkupInterpolated($"[green]{a}[/] [yellow]{op.AsString()}[/] [green]{b}[/] = ");
    var ansBuffer = string.Empty;
    
    while (true)
    {
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Enter)
        {
            ans = double.Parse(ansBuffer);
            break;
        }
        
        if (int.TryParse(key.KeyChar.ToString(), out var n))
        {
            AnsiConsole.MarkupInterpolated($"[blue]{n}[/]");
            ansBuffer += n;
        }
        else
        {
            Console.Beep();
        }
    }
    
    if (ans == expected)
    {
        AnsiConsole.MarkupLineInterpolated($"     [green]V[/]");
        totalCorrect++;
    }
    else
    {
        AnsiConsole.MarkupLineInterpolated($"     [red]X[/]");
        //AnsiConsole.WriteLine("this answer is a bit wrong");
        //var tryAgain = AnsiConsole.Confirm("try again?");

        //if (!tryAgain) break;
    }

    //} while (ans != expected && !ct.IsCancellationRequested);
}

AnsiConsole.MarkupInterpolated($"[yellow]Total asked: [blue]{totalRounds}[/], total correct: [green]{totalCorrect}[/] total incorrect: [red]{totalRounds - totalCorrect}[/][/]");