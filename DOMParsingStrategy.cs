using System.Xml;
namespace MauiApp2
{
    class DOMParsingStrategy : IXmlParserStrategy
    {
        XmlDocument doc = new XmlDocument();
        public List<Student> Analyze(Student student, string path)
        {
            doc.Load(path);

            List<List<Student>> info = new List<List<Student>>();
            try
            {
                if (student.Faculty != null) info.Add(SearchByParam("faculty", "name", student.Faculty, doc, 0));
                if (student.Department != null) info.Add(SearchByParam("department", "name", student.Department, doc, 1));
                if (student.Discipline != null) info.Add(SearchByParam("discipline", "name", student.Discipline, doc, 2));
                if (student.Name != null) info.Add(SearchByParam("student", "name", student.Name, doc, 3));
                if (student.Grade != null) info.Add(SearchByParam("student", "grade", student.Grade, doc, 3));

                if (student.Faculty == null &&
                    student.Department == null &&
                    student.Discipline == null &&
                    student.Name == null &&
                    student.Grade == null)
                {
                    return ApplyGradeFilter(AllStudents(doc), student);
                }
            }
            catch { }

            return ApplyGradeFilter(Cross(info, student), student);
        }

        private List<Student> ApplyGradeFilter(List<Student> students, Student student)
        {
            if (student.MinGrade != -1 || student.MaxGrade != 101)
            {
                return students.Where(s =>
                {
                    int grade = int.Parse(s.Grade);
                    return (student.MinGrade == -1 || grade >= student.MinGrade) &&
                           (student.MaxGrade == 101 || grade <= student.MaxGrade);
                }).ToList();
            }

            return students;
        }

        public static Student Info(XmlNode node)
        {
            Student nw = new Student();
            nw.Faculty = node.ParentNode.ParentNode.ParentNode.Attributes.GetNamedItem("name").Value;
            nw.Department = node.ParentNode.ParentNode.Attributes.GetNamedItem("name").Value;
            nw.Discipline = node.ParentNode.Attributes.GetNamedItem("name").Value;
            nw.Name = node.Attributes.GetNamedItem("name").Value;
            nw.Grade = node.Attributes.GetNamedItem("grade").Value;
            return nw;
        }

        public static List<Student> AllStudents(XmlNode doc)
        {
            List<Student> data2 = new List<Student>();
            XmlNodeList elem = doc.SelectNodes("//student");
            try
            {
                foreach (XmlNode el in elem)
                {
                    data2.Add(Info(el));
                }
            }
            catch { }
            return data2;
        }

        public static List<Student> SearchByParam(string nodename, string val, string param, XmlDocument doc, int n)
        {
            List<Student> students = new List<Student>();

            if (param != String.Empty && param != null)
            {
                switch (n)
                {
                    case 0:
                        {
                            XmlNodeList elem = doc.SelectNodes("//" + nodename + "[@" + val + "=\"" + param + "\"]");
                            try
                            {
                                foreach (XmlNode e in elem)
                                {
                                    XmlNodeList list1 = e.ChildNodes;
                                    foreach (XmlNode el1 in list1)
                                    {
                                        XmlNodeList list2 = el1.ChildNodes;
                                        foreach (XmlNode el2 in list2)
                                        {
                                            XmlNodeList list3 = el2.ChildNodes;
                                            foreach (XmlNode el3 in list3)
                                            {
                                                students.Add(Info(el3));
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }
                            return students;
                        }

                    case 1:
                        {
                            XmlNodeList elem = doc.SelectNodes("//" + nodename + "[@" + val + "=\"" + param + "\"]");
                            try
                            {
                                foreach (XmlNode e in elem)
                                {
                                    XmlNodeList list1 = e.ChildNodes;
                                    foreach (XmlNode el1 in list1)
                                    {
                                        XmlNodeList list2 = el1.ChildNodes;
                                        foreach (XmlNode el2 in list2)
                                        {
                                            students.Add(Info(el2));
                                        }
                                    }
                                }
                            }
                            catch { }
                            return students;
                        }

                    case 2:
                        {
                            XmlNodeList elem = doc.SelectNodes("//" + nodename + "[@" + val + "=\"" + param + "\"]");
                            try
                            {
                                foreach (XmlNode e in elem)
                                {
                                    XmlNodeList list1 = e.ChildNodes;
                                    foreach (XmlNode el1 in list1)
                                    {
                                        students.Add(Info(el1));
                                    }
                                }
                            }
                            catch { }
                            return students;
                        }

                    case 3:
                        {
                            XmlNodeList elem = doc.SelectNodes("//" + nodename + "[@" + val + "=\"" + param + "\"]");
                            try
                            {
                                foreach (XmlNode e in elem)
                                {
                                    students.Add(Info(e));
                                }
                            }
                            catch { }
                            return students;
                        }
                    default:
                        break;
                }
            }
            return AllStudents(doc);
        }

        private static List<Student> Cross(List<List<Student>> list, Student student)
        {
            List<Student> result = new List<Student>();
            try
            {
                if (list != null)
                {
                    Student[] stud = list[0].ToArray();
                    if (stud != null)
                    {
                        foreach (Student elem in stud)
                        {
                            bool IsIn = false;
                            foreach (List<Student> t in list)
                            {
                                if (t.Count <= 0) return new List<Student>();

                                foreach (Student s in t)
                                {
                                    IsIn = false;
                                    if (elem.Compare(s))
                                    {
                                        IsIn = true;
                                        break;
                                    }
                                }
                                if (!IsIn) break;
                            }

                            int grade = int.Parse(elem.Grade);
                            if ((student.MinGrade == -1 || grade >= student.MinGrade) &&
                                (student.MaxGrade == 101 || grade <= student.MaxGrade))
                            {
                                if (IsIn) result.Add(elem);
                            }
                        }
                    }
                }
            }
            catch { }
            return result;
        }
    }

}