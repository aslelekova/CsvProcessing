using System.Text;

namespace CsvLibrary;

/// <summary>
/// Provides static methods for processing and manipulating two-dimensional arrays of strings.
/// </summary>
public static class DataProcessing
{
    /// <summary>
    /// Selects rows from a two-dimensional string array based on a specified filter.
    /// </summary>
    /// <param name="valueIndex">Index of the value in each row to use as the filter.</param>
    /// <param name="data">Two-dimensional array of strings containing the data.</param>
    /// <param name="filter">The name of the filter used to select rows.</param>
    /// <returns>A two-dimensional array of strings representing the selected rows that match the filter criteria.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided data is null or empty, the filter is null or empty, or if the valueIndex is out of range.</exception>
    public static string[][] SelectionByValue(int valueIndex, string[][] data, string? filter)
    {
        // Check for null or empty data, null or empty filter, and valid valueIndex range.
        if (data == null || data.Length == 0 || string.IsNullOrEmpty(filter) ||
            !(0 <= valueIndex && valueIndex <= 9))
        {
            throw new ArgumentNullException("","Ошибка. Данные некорректны.");
        }
        
        // Select rows based on the specified filter and value Index. 
        string[][] selectedData = Array.FindAll(data, strings => strings[valueIndex].ToLower().Contains(filter.ToLower()));
        
        // Check if any rows were selected.
        if (selectedData == null || selectedData.Length == 0)
        {
            throw new ArgumentNullException("","Ошибка. Значения не найдены.");
        }
        
        return selectedData;
    }

    /// <summary>
    /// Reads and returns a filter value from the console input.
    /// </summary>
    /// <param name="name">The optional name of the parameter for filtering.</param>
    /// <returns>The entered filter value as a string.</returns>
    public static string ReadFilterValue(string name = "")
    {
        // Prompt the user to enter a filter parameter, including the optional parameter name.
        Console.Write("\nВведите параметр для выборки" + (!string.IsNullOrEmpty(name) ? $" по {name}" : "") + ": ");
        string? filterName = Console.ReadLine();
        
        // Ensure that the entered value is not empty.
        while (string.IsNullOrEmpty(filterName))
        {
            Console.Write("\nНекорректное значение. Повторите попытку: ");
            filterName = Console.ReadLine();
        }
        
        return filterName;
    }
    
    /// <summary>
    /// Removes punctuation characters from the input string.
    /// </summary>
    /// <param name="input">The input string containing punctuation.</param>
    /// <returns>A new string without punctuation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided input string is null or empty.</exception>
    private static string RemovePunctuation(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException("", "Ошибка. Передана пустая строка.");
        }
        
        StringBuilder result = new StringBuilder();

        foreach (char c in input)
        {
            // Check if the character is not a punctuation character.
            if (!char.IsPunctuation(c))
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }
    
    /// <summary>
    /// Sorts a two-dimensional string array based on the values in the specified column.
    /// </summary>
    /// <param name="data">Two-dimensional array of strings to be sorted.</param>
    /// <param name="descending">A boolean flag indicating whether to sort in descending order (true) or ascending order (false).</param>
    /// <returns>A two-dimensional array of strings representing the sorted data.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided data is null or empty.</exception>
    public static string[][] SortByValue(string[][] data, bool descending)
    {
        // Check for null or empty data.
        if (data == null || data.Length <= 2 )
        {
            throw new ArgumentNullException("","Ошибка. Данные некорректны.");
        }
        
        // Use a bubble sort algorithm to sort the data based on the values in the third column (LatinName, index 2).
        int n = data.Length;
        
        // Sort in reverse alphabetical order.
        if (descending)
        {
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 2; j < n - i - 1; j++)
                {
                    // Compare strings and swap them if necessary.
                    if (string.Compare(RemovePunctuation(data[j][2]), RemovePunctuation(data[j + 1][2]), StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        (data[j], data[j + 1]) = (data[j + 1], data[j]);
                    }
                }
            }
        }
        // В качестве альтернативного решения для сортировки в прямом алфавитном порядке можно использовать Array.Sort() => string.Compare().
        // Sort in direct alphabetical order.
        else
        {
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 2; j < n - i - 1; j++)
                {
                    // Compare strings and swap them if necessary.
                    if (string.Compare(RemovePunctuation(data[j][2]), RemovePunctuation(data[j + 1][2]),
                            StringComparison.Ordinal) > 0)
                    {
                        (data[j], data[j + 1]) = (data[j + 1], data[j]);
                    }
                }
            }
        }
        
        return data;
    }
}