using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MauiApp2
{
    class LINQParsingStrategy : IXmlParserStrategy
    {
        public List<Student> Analyze(Student student, string path)
        {
            // Завантаження XML-документу
            var doc = XDocument.Load(path);
            var matches = doc.Descendants("student")
                .Where(val =>
                {
                    // Перевірка оцінки (якщо задано)
                    int grade = int.Parse(val.Attribute("grade")?.Value ?? "0");

                    // Перевірка фільтрів: факультет, кафедра, дисципліна, ім'я та оцінка
                    bool matchesFaculty = string.IsNullOrEmpty(student.Faculty) || val.Parent.Parent.Parent.Attribute("name")?.Value.Contains(student.Faculty, StringComparison.OrdinalIgnoreCase) == true;
                    bool matchesDepartment = string.IsNullOrEmpty(student.Department) || val.Parent.Parent.Attribute("name")?.Value.Contains(student.Department, StringComparison.OrdinalIgnoreCase) == true;
                    bool matchesDiscipline = string.IsNullOrEmpty(student.Discipline) || val.Parent.Attribute("name")?.Value.Contains(student.Discipline, StringComparison.OrdinalIgnoreCase) == true;
                    bool matchesName = string.IsNullOrEmpty(student.Name) || val.Attribute("name")?.Value.Contains(student.Name, StringComparison.OrdinalIgnoreCase) == true;

                    // Перевірка оцінки за діапазоном
                    bool matchesGrade = (student.MinGrade == -1 || grade >= student.MinGrade) && (student.MaxGrade == 101 || grade <= student.MaxGrade);

                    // Всі умови мають бути виконані
                    return matchesFaculty && matchesDepartment && matchesDiscipline && matchesName && matchesGrade;
                })
                .Select(val => new Student
                {
                    Faculty = val.Parent.Parent.Parent.Attribute("name")?.Value,
                    Department = val.Parent.Parent.Attribute("name")?.Value,
                    Discipline = val.Parent.Attribute("name")?.Value,
                    Name = val.Attribute("name")?.Value,
                    Grade = val.Attribute("grade")?.Value
                })
                .ToList();

            return matches;
        }
    }
}
