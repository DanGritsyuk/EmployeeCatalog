namespace EmployeeCatalog.ConsoleApp.Services
{
    /// <summary>
    /// Парсер аргументов командной строки.
    /// </summary>
    public static class CommandLineArgumentsParser
    {
        /// <summary>
        /// Разбирает аргументы командной строки и возвращает их в виде словаря.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        /// <returns>Словарь, где ключ — это имя параметра, а значение — его значение.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если количество аргументов нечетное или отсутствует значение для ключа.</exception>
        public static Dictionary<string, string> Parse(string[] args)
        {
            if (args.Length % 2 != 0)
            {
                throw new ArgumentException("The number of arguments must be even.");
            }

            var parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < args.Length; i += 2)
            {
                if (args[i].StartsWith("--", StringComparison.Ordinal))
                {
                    string key = args[i].Substring(2);
                    string value = args[i + 1];

                    if (!parameters.TryAdd(key, value))
                    {
                        throw new ArgumentException($"Duplicate key: {key}");
                    }
                }
                else
                {
                    throw new ArgumentException($"Invalid key format: {args[i]}");
                }
            }

            return parameters;
        }
    }
}
