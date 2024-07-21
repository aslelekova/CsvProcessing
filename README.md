# CsvProcessing

## Overview

This project involves creating a console application and a class library for handling CSV files with plant data. The solution includes reading from, processing, sorting, and saving data from CSV files.

## Main Objective

The main goal is to develop a console application and a class library with static methods for managing CSV data. The console application should enable users to:

1. **Provide File Path**: Request the absolute path to a CSV file and load data using the `Read` method from the `CsvProcessing` class.
2. **Display Menu**: Offer a menu for the user to:
   - Perform data filtering based on specified field values.
   - Sort data based on selected fields.
   - Save results of the operations to a new CSV file.
3. **Handle Exceptions**: Manage all exceptions related to file operations and data processing.

## Console Application

The console application performs the following tasks:

1. **File Path Input**: Prompts the user to enter the absolute path of the CSV file. The data is loaded using the `Read` method from the `CsvProcessing` class.
2. **Menu Options**: Presents a menu with options to:
   - Filter data by field values (e.g., `LandscapingZone`, `LocationPlace`).
   - Sort data by fields (e.g., `LatinName` in ascending or descending order).
   - Save filtered and sorted data to a new CSV file.
3. **Error Handling**: Includes robust error handling for file operations and user input.

## Class Library

The class library contains static classes with methods for CSV file operations:

### `CsvProcessing` Class

- **`Read` Method**: Reads data from the CSV file specified by the static field `fPath`. Returns the data as a string array. Throws an `ArgumentNullException` if the file is missing or has an incorrect format.
- **`Write` Method**:
  - Overloaded to append a single string to a file or create a new file if it does not exist.
  - Overloaded to write an array of strings to a file, overwriting existing content or creating a new file if necessary.

### `DataProcessing` Class

- Contains methods for:
  - Filtering data based on specified field values.
  - Sorting data by specified fields.
  
## Running the Project

1. **Compile**: Build the project using .NET 6.0.
2. **Run**: Execute the console application.
3. **Input**: Provide the absolute path to the CSV file when prompted.
4. **Use Menu**: Navigate through the menu options to filter, sort, and save data.

## Conclusion

This project provides a complete solution for managing CSV files in .NET. It includes a class library for data processing and a console application for user interaction.
