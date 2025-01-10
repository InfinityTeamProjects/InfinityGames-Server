using System.Globalization;
using UserService.Domain.Exceptions;

namespace UserService.Application.Extensions;

public static class DateFormatExtension
{
    /// <summary>
    /// Возвращает дату с типом DateTime
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime ToDateTime(this string date)
    {
        try
        {
            return DateTime.SpecifyKind(DateTime.Parse(date, CultureInfo.InvariantCulture), DateTimeKind.Utc);
        }
        catch
        {
            throw new CustomException(400, "Формат даты введён не правильно!");
        }
    }
}