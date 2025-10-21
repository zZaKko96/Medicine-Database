using System;
using System.Text; 
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var model = new DatabaseModel();
        var view = new ConsoleView();
        var controller = new MainController(model, view);

        await controller.RunAsync();

        Console.WriteLine("Роботу програми завершено.");
    }
}