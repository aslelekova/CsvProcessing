namespace CsvLibrary;

/// <summary>
/// Provides methods for reading, writing and manipulating CSV files.
/// </summary>
public static class CsvProcessing
{
    /// <summary>
    /// A path to the input Csv file.
    /// </summary>
    private static string? fPath;
    
    /// <summary>
    /// Gets or sets the file path for CsvProcessing operations.
    /// </summary>
    /// <remarks>
    /// This property provides access to the file path used by the CsvProcessing class for reading and writing data to a CSV file. 
    /// </remarks>
    public static string? FilePath
    {
        get => fPath;
        set => fPath = value;
    }
    
    /// <summary>
    /// Checks for the presence and correctness of the header in the file.
    /// </summary>
    /// <param name="data">An array of strings containing data lines from the file.</param>
    /// <returns>True if the data header is correct; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the file is empty, insufficient data is provided, or if the header is incorrect.</exception>
    private static bool IsDataCorrect(string[] data)
    {
        // Expected column headers for comparison.
        string[] expectedHeader =
        {
            "\"ID\";\"Name\";\"LatinName\";\"Photo\";\"LandscapingZone\";\"ProsperityPeriod\";\"Description\";\"LocationPlace\";\"ViewForm\";\"global_id\";",
            "\"Код\";\"Название\";\"Латинское название\";\"Фотография\";\"Ландшафтная зона\";\"Период цветения\";\"Описание\";\"Расположение в парке\";\"Форма осмотра\";\"global_id\";"
        };

        // Check if the provided data is null or insufficient.
        if (data == null || data.Length < 3)
        {
            throw new ArgumentNullException("","Файл пуст или предоставлено недостаточно данных.");
        }

        // Compare the actual header with the expected header.
        for (int i = 0; i < 2; i++)
        {
            // Check if the current line of the data matches the expected header.
            if (data[i] != expectedHeader[i])
            {
                throw new ArgumentNullException("","Некорректный заголовок файла.");
            }
        }
        return true;
    }
    
    /// <summary>
    ///  Reads all lines from a file, concatenates them, and splits the concatenated text into rows based on a specified condition.
    /// </summary>
    /// <returns>An array of strings representing the rows of the CSV file.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the file path is empty, the file is empty, or the data is incorrect.</exception>
    public static string[] Read()
    {
        // Check if the file path is empty; if so, prompt the user to enter a valid path.
        if (string.IsNullOrEmpty(FilePath))
        {
            FilePath = ReadPath();
        }
        
        string[] lines = File.ReadAllLines(FilePath);
        
        // Concatenate the lines into a single string for further processing.
        string mergedLines = string.Join("", lines);
        
        // Split the concatenated text into rows using the specified condition.
        string[] rows = FormEntry(mergedLines, "\";\"", 10);
        
        // Check if the data in the rows is correct; if not, throw an exception.
        if (!IsDataCorrect(rows))
        {
            throw new ArgumentNullException("","Недопустимый формат данных в файле.");
        }

        return rows;
    }
    
    /// <summary>
    /// Forms entries by dividing the input text into parts using the specified delimiter symbol, and each entry is determined by every tenth occurrence.
    /// </summary>
    /// <param name="input">The entire text from the file.</param>
    /// <param name="delimiter">The delimiter symbol used to split the text.</param>
    /// <param name="expectedColumnCount">The expected number of columns (delimiters) in each formed entry</param>
    /// <returns>An array of strings representing the split lines of the file.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input text or delimiter is empty.</exception>
    private static string[] FormEntry(string input, string delimiter, int expectedColumnCount)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(delimiter))
        {
            throw new ArgumentNullException("", "Ошибка. Данные некорректны."); 
        }
        
        var parts = input.Split(delimiter);

        if (parts.Length % expectedColumnCount != 0)
        {
            throw new ArgumentNullException("","Ошибка. Некорректное количество столбцов.");
        }
        
        // Calculate the number of entries that will be formed.
        var result = new string[(parts.Length + expectedColumnCount - 1) / expectedColumnCount];

        for (int i = 0; i < result.Length; i++)
        {
            // Calculate the start index for each entry.
            int start = i * expectedColumnCount;
            
            // Form the entry by joining parts with the delimiter, adding quotes if needed.
            result[i] = (i != 0 ? "\"" : "") + string.Join(delimiter, parts, start, expectedColumnCount) + (i != result.Length - 1 ? "\";" : "");
        }

        return result;
    }
    
    /// <summary>
    /// Reads a file path from the console input and ensures its validity.
    /// </summary>
    /// <returns>A string representing the valid path to an existing file.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the entered file name is null or empty, or if the file does not exist.</exception>
    public static string ReadPath() 
    {
        Console.WriteLine("Введите путь к файлу:");
        string? filePath = Console.ReadLine();
        
        // Check if the entered file path is null or empty, or if the file does not exist.
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            throw new ArgumentNullException("", "Ошибка. Некорректное имя файла или файл не существует.");
        }

        return filePath;
    }
    
    /// <summary>
    /// Divides a string array into an array of arrays of strings, where each row is split into elements using a specified separator.
    /// </summary>
    /// <param name="rows">Input array of strings representing rows to split.</param>
    /// <returns>A two-dimensional array of strings representing separated elements.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input array is null or empty.</exception>
    public static string[][] SplitRows(string[] rows)
    {
        if (rows == null || rows.Length == 0)
            throw new ArgumentNullException("","Ошибка. Некорректные данные.");
        
        string[][] result = new string[rows.Length][];
    
        for (int i = 0; i < rows.Length; i++)
        {
            // Removing quotes and semicolons at the beginning and end of the line, then separating by the specified separator.
            string[] elements = rows[i].Trim('"', ';').Split("\";\"");
            
            // Initializing a nested array in the resulting array.
            result[i] = new string[elements.Length];
            
            // Copying elements to a new array.
            Array.Copy(elements, result[i], elements.Length);
        }
        
        return result;
    }

    /// <summary>
    /// Appends a single line of data to a specified file path.
    /// </summary>
    /// <param name="line">The string containing the data to be appended to the file.</param>
    /// <param name="nPath">The absolute path to the file where the data will be written.</param>
    /// <exception cref="ArgumentNullException">Thrown if an error occurs during the file write operation.</exception>
    public static void Write(string line, string nPath)
    {
        try
        {
            File.AppendAllLines(nPath, new []{ line });
        }
        catch (Exception)
        {
            throw new ArgumentNullException("", "Ошибка при записи данных в файл.");
        }
    }
    
    /// <summary>
    /// Appends an array of data lines to a specified file path.
    /// </summary>
    /// <param name="line">An array of strings containing the data lines to be appended to the file.</param>
    /// <param name="nPath">The absolute path to the file where the data will be written.</param>
    /// <exception cref="ArgumentNullException">Thrown if an error occurs during the file write operation.</exception>
    public static void Write(string[] line, string nPath)
    {
        try
        {
            File.AppendAllLines(nPath, line);
        }
        catch (Exception)
        {
            throw new ArgumentNullException("", "Ошибка при записи данных в файл.");
        }
    }
    
    /// <summary>
    /// Reads a file name.
    /// </summary>
    /// <returns>A string with the file name if it is correct.</returns>
    public static string ReadFileName()
    {
        Console.Write("\nВведите имя файла: ");
        string? fileName = Console.ReadLine();

        while (string.IsNullOrEmpty(fileName) || !IsFileNameCorrect(fileName))
        {
            Console.Write(
                "\nНекорректное имя файла. Имя файла содержит запрещенные символы.\nПопробуйте еще раз: ");
            fileName = Console.ReadLine();
        }

        return fileName;
    }

    /// <summary>
    /// Checks whether a given file name is valid by ensuring it does not contain forbidden symbols.
    /// </summary>
    /// <param name="fileName">The name of the file to be validated</param>
    /// <returns>True if the file name is valid; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input array is null or empty.</exception>
    private static bool IsFileNameCorrect(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentNullException("", "Ошибка. Некорректное имя файла.");
        }
        
        // Check if the file name contains forbidden symbols.
        string invalidSymbols = new string(Path.GetInvalidFileNameChars());

        foreach (char invalidSymbol in invalidSymbols)
        {
            if (fileName.Contains(invalidSymbol))
            {
                return false;
            }
        }

        return true;
    }
}
