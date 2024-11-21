using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp2
{
    class Student
    {
        public string Faculty { get; set; } = null;
        public string Department { get; set; } = null;
        public string Discipline { get; set; } = null;
        public string Name { get; set; } = null;
        public string Grade { get; set; } = null;
        public int MinGrade { get; set; } = -1;
        public int MaxGrade { get; set; } = 101;

        public Student() { }

        public Student(string[] data)
        {
            Faculty = data[0];
            Department = data[1];
            Discipline = data[2];
            Name = data[3];
            Grade = data[4];


        }

        public Student(IXmlParserStrategy strategy)
        {
            Faculty = String.Empty;
            Department = String.Empty;
            Discipline = String.Empty;
            Name = String.Empty;
            Grade = String.Empty;
        }

        public bool Compare(Student student)
        {
            return Faculty == student.Faculty &&
               Department == student.Department &&
               Discipline == student.Discipline &&
               Name == student.Name &&
               Grade == student.Grade;
        }

        public IXmlParserStrategy Strategy { get; set; }
        public List<Student> Analyze(Student parametrs, string path)
        {
            return Strategy.Analyze(parametrs, path);
        }
    }

    interface IXmlParserStrategy
    {
        List<Student> Analyze(Student p, string path);
    }
}
