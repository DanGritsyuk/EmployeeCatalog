namespace EmployeeCatalog.Common.Entities.Enums
{
    public enum ChangeEventType
    {
        Created,  // Создана новая запись
        Updated,  // Обновлена существующая запись
        Deleted,  // Удалена запись
        Recovered   // Восстановлена запись
    }
}
