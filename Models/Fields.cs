using System.ComponentModel.DataAnnotations;

namespace Documently.Models;

/*
 * Базовый класс для всех полей шаблона
 * Пожалуйста, наследуйте от него и перегрузите свойство Value конкретным типом данных
 * К этому свойству также можно добавить атрибут проверки корректности значения, подробнее:
 * https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationattribute?view=net-6.0
 */
public class Field
{
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual string Value { get; set; }

    public Field ()
    {
        Name = string.Empty;
        Description = string.Empty;
        Value = string.Empty;
    }

    public Field (string name)
    {
        Name = name;
        Description = string.Empty;
        Value = string.Empty;
    }
}