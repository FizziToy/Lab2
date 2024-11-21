using System.Xml;
namespace MauiApp2
{
    class SAXParsingStrategy : IXmlParserStrategy
    {
        private List<Student> lastResult = null;

        public List<Student> Analyze(Student student, string path)
        {
            XmlReader reader = XmlReader.Create(path);
            List<Student> result = new List<Student>();
            Student stud = null;

            string _faculty = null;
            string _department = null;
            string _discipline = null;

            while (reader.Read())
            {
                if (reader.Name == "faculty")
                {
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.Name == "name")
                        {
                            _faculty = reader.Value;
                        }
                    }
                }
                if (reader.Name == "department")
                {
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.Name == "name")
                        {
                            _department = reader.Value;
                        }
                    }
                }
                if (reader.Name == "discipline")
                {
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.Name == "name")
                        {
                            _discipline = reader.Value;
                        }
                    }
                }
                if (reader.Name == "student")
                {
                    if (stud == null)
                    {
                        stud = new Student();
                        stud.Faculty = _faculty;
                        stud.Department = _department;
                        stud.Discipline = _discipline;
                    }
                    else
                    {
                        stud = new Student();
                        stud.Faculty = _faculty;
                        stud.Department = _department;
                        stud.Discipline = _discipline;
                    }

                    if (reader.HasAttributes)
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            if (reader.Name == "name")
                            {
                                stud.Name = reader.Value;
                            }
                            if (reader.Name == "grade")
                            {
                                stud.Grade = reader.Value;
                            }
                        }
                    }
                    result.Add(stud);
                }
            }

            lastResult = Filter(result, student);
            return lastResult;
        }

        public List<Student> Filter(List<Student> allStud, Student param)
        {
            List<Student> result = new List<Student>();

            if (allStud != null)
            {
                foreach (Student e in allStud)
                {
                    try
                    {
                        int grade = int.Parse(e.Grade);

                        if (
                            (e.Faculty == param.Faculty || param.Faculty == null) &&
                            (e.Department == param.Department || param.Department == null) &&
                            (e.Discipline == param.Discipline || param.Discipline == null) &&
                            (e.Name == param.Name || param.Name == null) &&
                            (e.Grade == param.Grade || param.Grade == null) &&
                            (param.MinGrade == -1 || grade >= param.MinGrade) &&
                            (param.MaxGrade == 101 || grade <= param.MaxGrade)
                            )
                        {
                            result.Add(e);
                        }
                    }
                    catch { }
                }
            }
            return result;
        }
    }
}
