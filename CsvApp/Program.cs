using System.Text;
using CsvLibrary;

class Program
{
    /// <summary>
    /// The main entry point for the console application.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if an error occurs when working with data.</exception>
    public static void Main()
    {
        // Set the console encoding to UTF-8 for correct character display.
        Console.OutputEncoding = Encoding.UTF8;
        
        // For Windows you should use a different code.
        // Console.OutputEncoding = Encoding.Unicode;
        
        do
        {
            try
            {
                Console.Clear();


                string path = CsvProcessing.ReadPath();
                CsvProcessing.FilePath = path;

                // Read and process the CSV file data.
                string[] lines = CsvProcessing.Read();
                string[][] CsvData = CsvProcessing.SplitRows(lines);

                // Display the menu of possible actions to the user.
                Console.Write(@"
Меню возможных действий:
1. Произвести выборку по значению LandscapingZone.
2. Произвести выборку по значению LocationPlace.
3. Произвести выборку по значению LandscapingZone и ProsperityPeriod.
4. Отсортировать таблицу по значению LatinName (прямой порядок).
5. Отсортировать таблицу по значению LatinName (обратный порядок).
6. Выйти из программы.

Укажите номер выбранного пункта: ");

                int numberOfAction;
                string? firstParameter;

                // Input the action number.
                while (!(int.TryParse(Console.ReadLine(), out numberOfAction) && 1 <= numberOfAction &&
                         numberOfAction <= 6))
                {
                    Console.Write("\nТакого пункта меню не существует. Повторите попытку: ");
                }

                string[][] result = Array.Empty<string[]>();
                switch (numberOfAction)
                {
                    case 1:
                        // Perform a selection based on the value of LandscapingZone.
                        firstParameter = DataProcessing.ReadFilterValue();

                        result = DataProcessing.SelectionByValue(4, CsvData, firstParameter);

                        // Display a header.
                        for (int i = 0; i < 2 && i < CsvData.Length; i++)
                        {
                            Console.WriteLine(string.Join(" | ", CsvData[i]));
                        }

                        break;

                    case 2:
                        // Perform a selection based on the value of LocationPlace.
                        firstParameter = DataProcessing.ReadFilterValue();

                        result = DataProcessing.SelectionByValue(7, CsvData, firstParameter);

                        // Display a header.
                        for (int i = 0; i < 2 && i < CsvData.Length; i++)
                        {
                            Console.WriteLine(string.Join(" | ", CsvData[i]));
                        }

                        break;

                    case 3:
                        // Perform a selection based on the values of LandscapingZone and ProsperityPeriod.
                        firstParameter = DataProcessing.ReadFilterValue("LandscapingZone");
                        string secondParameter = DataProcessing.ReadFilterValue("ProsperityPeriod");

                        // Intersect the results of two selections.
                        result = DataProcessing.SelectionByValue(4, CsvData, firstParameter)
                            .Intersect(DataProcessing.SelectionByValue(5, CsvData, secondParameter)).ToArray();

                        // Display a header.
                        for (int i = 0; i < 2 && i < CsvData.Length; i++)
                        {
                            Console.WriteLine(string.Join(" | ", CsvData[i]));
                        }

                        break;

                    case 4:
                        // Sort the table by the value of LatinName in ascending order.
                        result = DataProcessing.SortByValue(CsvData, false);
                        break;

                    case 5:
                        // Sort the table by the value of LatinName in descending order.
                        result = DataProcessing.SortByValue(CsvData, true);
                        break;

                    case 6:
                        Environment.Exit(0);
                        break;

                    default:
                        throw new ArgumentNullException("", "Непредвиденная ошибка.");
                }

                // Display the result data, replacing empty fields with a placeholder.
                foreach (string[] el in result)
                {
                    for (int i = 0; i < el.Length; i++)
                    {
                        if (el[i].Length == 0)
                        {
                            el[i] = "Пустое поле";
                        }
                    }

                    Console.WriteLine(string.Join(" | ", el));
                }

                // Read user response, handling potential null inputs.
                Console.WriteLine("\nХотите ли Вы сохранить файл?\nВведите: yes/no");

                string? answer = Console.ReadLine();
                while (answer == null && !(answer.Equals("yes") || answer.Equals("no")))
                {
                    Console.Write("Некорректный ответ. Повторите попытку: ");
                    answer = string.Join("",
                        Console.ReadLine()?.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries) ??
                        Array.Empty<string>());
                }


                if (answer.Equals("yes"))
                {
                    string fileName = CsvProcessing.ReadFileName();
                    string nPath = $"{fileName}.txt";
                    File.WriteAllText(nPath, null);

                    string[] rows = Array.Empty<string>();

                    if (numberOfAction == 1 || numberOfAction == 2 || numberOfAction == 3)
                    {
                        for (int i = 0; i < 2 && i < CsvData.Length; i++)
                        {
                            CsvProcessing.Write('"' + string.Join("\";\"", CsvData[i]) + "\";", nPath);
                        }
                    }

                    foreach (string[] line in result)
                    {
                        string rowResult = '"' + string.Join("\";\"", line) + "\";";
                        rows = rows.Append(rowResult).ToArray();
                    }

                    CsvProcessing.Write(rows, nPath);
                }
            }
            catch (ArgumentNullException exception)
            {
                Console.WriteLine(exception.Message);
            }

            catch (Exception)
            {
                Console.WriteLine("Неизвестная ошибка.");
            }
            
            Console.WriteLine("\nДля продолжения нажмите любую клавишу, для выхода из программы - Escape...");
        } while (Console.ReadKey().Key != ConsoleKey.Escape);
    }
}
