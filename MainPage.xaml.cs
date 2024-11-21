using MauiApp2;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.IO;

namespace MauiApp2
{
    public partial class MainPage : ContentPage
    {
        private string _xmlFilePath = string.Empty;
        private IXmlParserStrategy _parserStrategy;

        public MainPage()
        {
            InitializeComponent();

            // Призначення ItemsSource для Picker
            parsingStrategyPicker.ItemsSource = new List<string>
            {
                "SAX Parsing",
                "DOM Parsing",
                "LINQ Parsing"
            };
        }

        // Обробка вибору XML файлу
        private async void OnChooseXmlFileClicked(object sender, EventArgs e)
        {
            var fileResult = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select XML File"
            });

            if (fileResult != null)
            {
                _xmlFilePath = fileResult.FullPath;
                xmlFileButton.Text = $"Selected: {fileResult.FileName}";
            }
            else
            {
                await DisplayAlert("Error", "No file selected.", "OK");
            }
        }

        // Обробка вибору стратегії парсингу
        private void OnStrategyChanged(object sender, EventArgs e)
        {
            int selectedIndex = parsingStrategyPicker.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    _parserStrategy = new SAXParsingStrategy();
                    break;
                case 1:
                    _parserStrategy = new DOMParsingStrategy();
                    break;
                case 2:
                    _parserStrategy = new LINQParsingStrategy();
                    break;
                default:
                    _parserStrategy = null;
                    break;
            }
        }

        // Обробка фільтрів
        private void OnApplyFiltersClicked(object sender, EventArgs e)
        {
            var filterStudent = new Student
            {
                Faculty = facultyEntry.Text,
                Department = departmentEntry.Text,
                Discipline = disciplineEntry.Text,
                Name = nameEntry.Text
            };
            
            if (!IsValidText(facultyEntry.Text) || !IsValidText(departmentEntry.Text) ||
        !IsValidText(disciplineEntry.Text) || !IsValidText(nameEntry.Text))
            {
                DisplayAlert("Error", "Please enter valid text (letters and spaces only) in filter fields.", "OK");
                return;
            }

            // Перевірка оцінки
            if (!string.IsNullOrWhiteSpace(gradeEntry.Text))
            {
                if (int.TryParse(gradeEntry.Text, out int grade))
                {
                    if (grade < 1 || grade > 100)
                    {
                        DisplayAlert("Error", "Grade must be between 1 and 100.", "OK");
                        return; 
                    }

                    filterStudent.MinGrade = grade;
                    filterStudent.MaxGrade = grade;
                }
                else
                {
                    DisplayAlert("Error", "Please enter a valid numeric grade.", "OK");
                    return; 
                }
            }

            // Перевірка стратегії парсингу та виклик аналізу
            if (!string.IsNullOrEmpty(_xmlFilePath) && _parserStrategy != null)
            {
                try
                {
                    var students = _parserStrategy.Analyze(filterStudent, _xmlFilePath);
                    DisplayStudentResults(students);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Failed to parse XML: {ex.Message}", "OK");
                }
            }
            else
            {
                DisplayAlert("Error", "Please select a valid file and parsing strategy.", "OK");
            }
        }
        private void OnClearFiltersClicked(object sender, EventArgs e)
        {
            // Очищення текстових полів
            facultyEntry.Text = string.Empty;
            departmentEntry.Text = string.Empty;
            disciplineEntry.Text = string.Empty;
            nameEntry.Text = string.Empty;
            gradeEntry.Text = string.Empty;

           // Очищення поля результатів
    resultLabel.Text = "Results will appear here";
            resultLabel.TextColor = Colors.Black;

            // Скидання результатів у CollectionView
            studentsCollectionView.ItemsSource = null;
            studentsCollectionView.IsVisible = false;

            // Додатковий візуальний індикатор
            DisplayAlert("Filters Cleared", "All filter fields and results have been cleared.", "OK");
        }
        // Обробка трансформації XML
        private void OnTransformClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_xmlFilePath) || _parserStrategy == null)
            {
                DisplayAlert("Error", "Please select a valid file and parsing strategy.", "OK");
                return;
            }

            try
            {
                var transformator = new Transformator();
                var students = _parserStrategy.Analyze(new Student(), _xmlFilePath);
                string xslPath = "C:\\Users\\User\\source\\repos\\MauiApp2\\transform.xsl";
                string htmlPath = "C:\\Users\\User\\source\\repos\\MauiApp2\\file.html";

                transformator.Transform(students, _xmlFilePath, xslPath, htmlPath);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Failed to transform XML: {ex.Message}", "OK");
            }
        }

        // Відображення результатів студентів
        private void DisplayStudentResults(List<Student> students)
        {
            if (students != null && students.Count > 0)
            {
                // Виводимо загальну інформацію
                resultLabel.Text = $"Found {students.Count} students";
                resultLabel.TextColor = Colors.Green;

                // Призначаємо джерело даних для CollectionView
                studentsCollectionView.ItemsSource = students;
                studentsCollectionView.IsVisible = true;
            }
            else
            {
                resultLabel.Text = "No students found matching the criteria.";
                resultLabel.TextColor = Colors.Red;

                // Очищаємо і ховаємо CollectionView
                studentsCollectionView.ItemsSource = null;
                studentsCollectionView.IsVisible = false;
            }
        }
        private bool IsValidText(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return true; 
            var regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z\s]+$");
            return regex.IsMatch(input);
        }
    }
}
