using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        while (true)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Оберіть завдання:");
            Console.WriteLine("1. Пошук edu.ua посилань");
            Console.WriteLine("2. Видалення українських слів, що починаються на голосну літеру");
            Console.WriteLine("3. Видалення всіх входжень останньої букви слова");
            Console.WriteLine("4. Виведення чисел в заданому діапазоні");
            Console.WriteLine("5. Робота з папками");
            Console.WriteLine("6. Вихід");
            Console.Write("Введіть номер завдання: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Task1();
                    break;
                case "2":
                    Task2();
                    break;
                case "3":
                    Task3();
                    break;
                case "4":
                    Task4();
                    break;
                case "5":
                    task5();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    static void Task1()
    {
        string inputFilePath = @"C:\Users\Катя\Source\Repos\csharplab8-MahdaKateryna\Lab8CSharp\input.txt";
        string outputFilePath = @"C:\Users\Катя\Source\Repos\csharplab8-MahdaKateryna\Lab8CSharp\output.txt";

            string content = File.ReadAllText(inputFilePath);
            string pattern = @"\bhttps?://[a-zA-Z0-9-]+\.edu\.ua\b";

            // знаходження edu.ua URLs в тексті
            MatchCollection matches = Regex.Matches(content, pattern);

            using (StreamWriter writer = new StreamWriter(outputFilePath, false))
            {
                writer.WriteLine($"Знайдено {matches.Count} адрес edu.ua:");
                foreach (Match match in matches)
                {
                    Console.WriteLine(match.Value);
                    writer.WriteLine(match.Value);
                }
            }

            Console.WriteLine("\nВведіть посилання, які ви хочете вилучити (розділіть їх комами):");
            string[] linksToRemove = Console.ReadLine().Split(',');

            Console.WriteLine("\nВведіть посилання, на які ви хочете замінити (розділіть їх комами, в тому ж порядку):");
            string[] linksToReplace = Console.ReadLine().Split(',');

          

            // Replacing URLs in the content
            for (int i = 0; i < linksToRemove.Length; i++)
            {
                content = content.Replace(linksToRemove[i].Trim(), linksToReplace[i].Trim());
            }

            // Writing the modified content back to the input file
            File.WriteAllText(inputFilePath, content);

            Console.WriteLine($"\nГотово! Результати збережено в {outputFilePath}");
        
        
    }

    static void Task2()
    {
        string inputFilePath = @"C:\Users\Катя\Source\Repos\csharplab8-MahdaKateryna\Lab8CSharp\input.txt";
        string outputFilePath = @"C:\Users\Катя\Source\Repos\csharplab8-MahdaKateryna\Lab8CSharp\output.txt";

        try
        {
            // Зчитування тексту з файлу
            string text = File.ReadAllText(inputFilePath);

            // Регулярний вираз для знаходження українських слів, які починаються на голосну літеру
            string pattern = @"\b[аеиоуюяєїіАЕИОУЮЯЄЇІ][\w']*";

            // Видалення знайдених слів
            string result = Regex.Replace(text, pattern, "");

            // Запис результату у новий файл
            File.WriteAllText(outputFilePath, result);

            Console.WriteLine("Готово!");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Файл не знайдено!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    static void Task3()
    {
        string inputFilePath = @"C:\Users\Катя\Source\Repos\csharplab8-MahdaKateryna\Lab8CSharp\input.txt";
        string outputFilePath = @"C:\Users\Катя\Source\Repos\csharplab8-MahdaKateryna\Lab8CSharp\output.txt";

        string[] lines = File.ReadAllLines(inputFilePath);
        string[] results = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            results[i] = RemovePreviousOccurrences(lines[i]);
        }

        // Запис результату у новий файл
        File.WriteAllLines(outputFilePath, results);

        Console.WriteLine("Результат збережено у файлі output.txt");
    }

    static string RemovePreviousOccurrences(string text)
    {
        // Розділення рядка на слова та розділові знаки за допомогою регулярних виразів
        string[] parts = Regex.Split(text, @"(\W+)");

        // Змінна для зберігання результату
        string result = "";

        int wordCount = 0;
        foreach (string part in parts)
        {
            // Якщо частина не порожня
            if (!string.IsNullOrEmpty(part))
            {
                // Якщо частина є словом
                if (Regex.IsMatch(part, @"\w+"))
                {
                    // Вилучення попередніх входжень останньої літери у слові
                    char lastChar = part[part.Length - 1];
                    string newWord = part.Substring(0, part.Length - 1).Replace(lastChar.ToString(), "") + lastChar;

                    // Додавання нового слова до результату
                    result += newWord;
                }
                else
                {
                    // Додавання розділового знаку до результату
                    result += part;
                }

                wordCount++;
                // Перевірка чи це не останній елемент
                if (wordCount < parts.Length)
                {
                    // Перевірка на наявність перенесення на новий рядок
                    if (text.IndexOf(part) + part.Length + 1 < text.Length &&
                        text[text.IndexOf(part) + part.Length] == '\r' &&
                        text[text.IndexOf(part) + part.Length + 1] == '\n')
                    {
                        // Додавання перенесення на новий рядок до результату
                        result += "\r\n";
                    }
                }
            }
        }

        // Повернення результату з вилученими попередніми входженнями останньої літери у словах
        return result.TrimEnd();
    }


    static void Task4()
    {
        
        string outputFilePath = @"C:\Users\Катя\Source\Repos\csharplab8-MahdaKateryna\Lab8CSharp\output.txt";

        // Приклад вхідних даних
        int n = 10;
        int[] numbers = { 1, 5, 10, 15, 20, 25, 30, 35, 40, 45 };
        int lowerBound = 10;
        int upperBound = 30;

        // Створення і заповнення списку чисел, які попадають у заданий інтервал
        List<int> selectedNumbers = new List<int>();
        for (int i = 0; i < n; i++)
        {
            if (numbers[i] >= lowerBound && numbers[i] <= upperBound)
            {
                selectedNumbers.Add(numbers[i]);
            }
        }

        // Запис вмісту списку в файл
        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            foreach (int number in selectedNumbers)
            {
                writer.WriteLine(number);
            }
        }

        // Виведення вмісту файлу на екран
        Console.WriteLine("Вміст файлу 'output.txt':");
        string[] lines = File.ReadAllLines(outputFilePath);
        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }
    }

    static void task5()
    {
        Console.WriteLine("Task 5\n");

        string studentName = "Magda";
        string folder1Path = $"D:\\temp\\{studentName}1";
        string folder2Path = $"D:\\temp\\{studentName}2";
        string allFolderPath = $"D:\\temp\\ALL";

        // Task1
        Directory.CreateDirectory(folder1Path);
        Directory.CreateDirectory(folder2Path);

        // Task2
        string t1FilePath = Path.Combine(folder1Path, "t1.txt");
        string t2FilePath = Path.Combine(folder1Path, "t2.txt");

        string t1Text = "Шевченко Степан Іванович, 2001 року народження, місце проживання м. Суми";
        string t2Text = "Комар Сергій Федорович, 2000 року народження, місце проживання м. Київ";

        File.WriteAllText(t1FilePath, t1Text);
        File.WriteAllText(t2FilePath, t2Text);

        // Task3
        string t3FilePath = Path.Combine(folder2Path, "t3.txt");
        File.WriteAllText(t3FilePath, File.ReadAllText(t1FilePath) + "\n" + File.ReadAllText(t2FilePath));

        // Task4
        PrintFileInfo(t1FilePath);
        PrintFileInfo(t2FilePath);
        PrintFileInfo(t3FilePath);

        // Task5
        string moveT2FilePath = Path.Combine(folder2Path, "t2.txt");
        if (File.Exists(moveT2FilePath))
        {
            File.Delete(moveT2FilePath);
        }
        File.Move(t2FilePath, moveT2FilePath);

        // Task6
        string copyT1FilePath = Path.Combine(folder2Path, "t1.txt");
        File.Copy(t1FilePath, copyT1FilePath);

        // Task7
        if (Directory.Exists(allFolderPath))
        {
            Directory.Delete(allFolderPath, true);
        }
        Directory.Move(folder1Path, allFolderPath);

        // Task8
        Console.WriteLine("\nFiles in All directory:");
        string[] filesInAll = Directory.GetFiles(allFolderPath);
        foreach (string file in filesInAll)
        {
            PrintFileInfo(file);
        }
    }
    static void PrintFileInfo(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        Console.WriteLine($"File Name: {fileInfo.Name}");
        Console.WriteLine($"Directory: {fileInfo.DirectoryName}");
        Console.WriteLine($"Size (bytes): {fileInfo.Length}");
        Console.WriteLine($"Created: {fileInfo.CreationTime}");
        Console.WriteLine($"Last Modified: {fileInfo.LastWriteTime}");
        Console.WriteLine();
    }

}

