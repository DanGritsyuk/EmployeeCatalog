using EmployeeCatalog.Common.Entities.Enums;
using EmployeeCatalog.Common.Entities;

namespace EmployeeCatalog.ConsoleApp.Services
{
    /// <summary>
    /// Генератор случайных данных о сотрудниках.
    /// </summary>
    /// <remarks>
    /// Этот класс предоставляет функциональность для генерации случайных данных о сотрудниках,
    /// включая ФИО, пол и дату рождения.
    /// </remarks>
    internal class EmployeeDataGenerator
    {
        private static readonly string[] MaleFirstNames = { "Aleksey", "Ivan", "Petr", "Sergey", "Vladimir", "Artur" };
        private static readonly string[] FemaleFirstNames = { "Anna", "Elena", "Maria", "Natalia", "Olga", "Tatiana" };

        private static readonly string[] MaleMiddleNames = { "Alekseevich", "Ivanovich", "Petrovich", "Sergeevich", "Vladimirovich", "Arturovich" };
        private static readonly string[] FemaleMiddleNames = { "Alekseevna", "Ivanovna", "Petrovna", "Sergeevna", "Vladimirovna", "Arturovna" };

        /// <summary>
        /// Список фамилий для мужчин.
        /// </summary>
        private static readonly string[] MaleLastNames =
        {
            "Andreev", "Belov", "Egorov", "Dmitriev", "Vasilyev",
            "Fedorov", "Filatov", "Frolov", "Fomin", "Fokin", "Gorbunov", "Zaitsev", "Kuznetsov", "Sokolov", "Petrov"
        };

        /// <summary>
        /// Список фамилий для женщин.
        /// </summary>
        private static readonly string[] FemaleLastNames =
        {
            "Alexandrova", "Borodina", "Ermolenko", "Davydova", "Vasileva",
            "Fedorenko", "Frolova", "Gorbunova", "Kuznetsova", "Petrova", "Sokolova", "Zaitseva"
        };

        private readonly Random _random = new();
        private readonly HashSet<string> _uniqueEntries = new();

        /// <summary>
        /// Генерирует сотрудников порциями (batch) заданного размера.
        /// </summary>
        /// <param name="totalCount">Общее количество сотрудников, которые нужно сгенерировать.</param>
        /// <param name="batchSize">Размер порции (batch) сотрудников.</param>
        /// <returns>Асинхронный поток списков сотрудников.</returns>
        /// <remarks>
        /// Этот метод генерирует сотрудников порциями (batch) заданного размера. Каждая фамилия используется для одного batch,
        /// после чего происходит переход к следующей фамилии. После перебора всех фамилий список начинается с начала.
        /// Имена и отчества выбираются случайным образом из предопределенных списков.
        /// </remarks>
        public async IAsyncEnumerable<List<Employee>> GenerateEmployees(int totalCount, int batchSize)
        {
            int generatedCount = 0;
            bool isMale = true;

            // Индексы для выбора фамилий по порядку
            int maleLastNameIndex = 0;
            int femaleLastNameIndex = 0;

            while (generatedCount < totalCount)
            {
                var batch = new List<Employee>();

                for (int j = 0; j < batchSize && generatedCount < totalCount; j++)
                {
                    string firstName = isMale
                        ? MaleFirstNames[_random.Next(MaleFirstNames.Length)]
                        : FemaleFirstNames[_random.Next(FemaleFirstNames.Length)];

                    string middleName = isMale
                        ? MaleMiddleNames[_random.Next(MaleMiddleNames.Length)]
                        : FemaleMiddleNames[_random.Next(FemaleMiddleNames.Length)];

                    string lastName = isMale
                        ? MaleLastNames[maleLastNameIndex]
                        : FemaleLastNames[femaleLastNameIndex];

                    DateOnly birthDate;
                    string uniqueKey;

                    do
                    {
                        // Генерация случайной даты рождения: год, месяц, день
                        int year = _random.Next(1957, 2005);  // Генерация года между 1957 и 2004
                        int month = _random.Next(1, 13);     // Случайный месяц от 1 до 12
                        int day = _random.Next(1, 29);       // Случайный день (с учетом всех месяцев)

                        birthDate = new DateOnly(year, month, day);
                        uniqueKey = $"{lastName} {firstName} {middleName} {birthDate}";
                    }
                    while (!_uniqueEntries.Add(uniqueKey)); // Гарантия уникальности

                    batch.Add(new Employee
                    {
                        Id = Guid.NewGuid(),
                        FullName = $"{lastName} {firstName} {middleName}",
                        Gender = isMale ? Gender.Male : Gender.Female,
                        BirthDate = birthDate
                    });

                    generatedCount++;

                    // Переход к следующей фамилии
                    if (generatedCount % batchSize == 0)
                    {
                        if (isMale)
                        {
                            maleLastNameIndex = (maleLastNameIndex + 1) % MaleLastNames.Length;
                        }
                        else
                        {
                            femaleLastNameIndex = (femaleLastNameIndex + 1) % FemaleLastNames.Length;
                        }
                    }
                }
                isMale = !isMale; // Чередование полов
                yield return batch;
                await Task.Yield();
            }
        }
    }
}