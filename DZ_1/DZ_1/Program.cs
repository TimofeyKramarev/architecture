using System;
using System.Text;

// Устанавливаем кодировку консоли для корректного ввода/вывода русских букв
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

do
{
    Console.Write("Введите первую строку (или 'exit' для выхода): ");
    string? first = Console.ReadLine();
    if (string.Equals(first, "exit", StringComparison.OrdinalIgnoreCase))
        break;

    Console.Write("Введите вторую строку: ");
    string? second = Console.ReadLine();

    int distance = DamerauLevenshteinDistance(first ?? "", second ?? "");
    Console.WriteLine($"Расстояние Дамерау-Левенштейна: '{first}' -> '{second}' = {distance}\n");
} while (true);

static int DamerauLevenshteinDistance(string str1Param, string str2Param)
{
    if (str1Param == null || str2Param == null) return -1;
    int str1Len = str1Param.Length;
    int str2Len = str2Param.Length;

    if (str1Len == 0 && str2Len == 0) return 0;
    if (str1Len == 0) return str2Len;
    if (str2Len == 0) return str1Len;

    // Приведение к верхнему регистру для регистронезависимого сравнения
    string str1 = str1Param.ToUpperInvariant();
    string str2 = str2Param.ToUpperInvariant();

    int[,] matrix = new int[str1Len + 1, str2Len + 1];

    for (int i = 0; i <= str1Len; i++) matrix[i, 0] = i;
    for (int j = 0; j <= str2Len; j++) matrix[0, j] = j;

    for (int i = 1; i <= str1Len; i++)
    {
        for (int j = 1; j <= str2Len; j++)
        {
            int cost = (str1[i - 1] == str2[j - 1]) ? 0 : 1;
            int insert = matrix[i, j - 1] + 1;
            int delete = matrix[i - 1, j] + 1;
            int replace = matrix[i - 1, j - 1] + cost;
            matrix[i, j] = Math.Min(Math.Min(insert, delete), replace);

            // Транспозиция
            if (i > 1 && j > 1 &&
                str1[i - 1] == str2[j - 2] &&
                str1[i - 2] == str2[j - 1])
            {
                int transposition = matrix[i - 2, j - 2] + 1; // стоимость транспозиции всегда 1
                matrix[i, j] = Math.Min(matrix[i, j], transposition);
            }
        }
    }
    return matrix[str1Len, str2Len];
}