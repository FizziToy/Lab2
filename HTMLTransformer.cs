using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace MauiApp2
{
    class Transformator
    {
        public void Transform(List<Student> filteredStudents, string xmlPath, string xslPath, string htmlPath)
        {
            string cxslPath = "C:\\Users\\User\\source\\repos\\MauiApp2\\transform.xsl";  

            // Завантажуємо XSL файл
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(cxslPath);
            XDocument inputDataXml;

            if (filteredStudents != null && filteredStudents.Count > 0)
            {
                var groupedStudents = filteredStudents.GroupBy(s => new { s.Faculty, s.Department, s.Discipline });

                inputDataXml = new XDocument(
                    new XElement("university",
                        from facultyGroup in groupedStudents.GroupBy(g => g.Key.Faculty)
                        select new XElement("faculty",
                            new XAttribute("name", facultyGroup.Key),
                            from departmentGroup in facultyGroup.GroupBy(g => g.Key.Department)
                            select new XElement("department",
                                new XAttribute("name", departmentGroup.Key),
                                from disciplineGroup in departmentGroup.GroupBy(g => g.Key.Discipline)
                                select new XElement("discipline",
                                    new XAttribute("name", disciplineGroup.Key),
                                    from student in disciplineGroup.SelectMany(sg => sg)
                                    select new XElement("student",
                                        new XAttribute("name", student.Name),
                                        new XAttribute("grade", student.Grade)
                                    )
                                )
                            )
                        )
                    )
                );
            }
            else
            {
                using (FileStream fileStream = new FileStream(xmlPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    inputDataXml = XDocument.Load(fileStream);
                }
            }

            using (XmlWriter writer = XmlWriter.Create(htmlPath))
            {
                xslt.Transform(inputDataXml.CreateReader(), null, writer);
            }

            // Use DisplayAlert for MAUI to show the success message
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await App.Current.MainPage.DisplayAlert("Success", "Трансформація успішно виконана!", "OK");
            });
        }

    }

}

