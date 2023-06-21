using System.ComponentModel.DataAnnotations;
using System;

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
    public string DisplayName { get; set; }
    public virtual string Value { get; }

    public Field (string name)
    {
        Name = name;
        DisplayName = name;
        Value = string.Empty;
    }
}

public class TextField : Field
{
    public string Text { get; set; }
    public override string Value => Text;

    public TextField (string name) : base(name)
    {
        Text = string.Empty;
    }
}

public class NumericField : Field
{
    public long Number { get; set; }
    public override string Value => Number.ToString();

    public NumericField (string name) : base(name)
    {
        Number = 0L;
    }
}

public class DateField : Field
{
    public DateTime Date { get; set; }
    public override string Value => Date.ToString();

    public DateField (string name) : base(name)
    {
        Date = DateTime.Today;
    }
}

public class PersonNameField : Field
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public override string Value => string.Join(' ', FirstName, MiddleName, LastName);

    public PersonNameField (string name) : base(name)
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        MiddleName = string.Empty;
    }
}