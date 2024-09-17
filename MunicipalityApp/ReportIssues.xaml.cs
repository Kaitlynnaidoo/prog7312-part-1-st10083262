using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MunicipalityApp
{
    /// <summary>
    /// Interaction logic for ReportIssues.xaml
    /// </summary>
    public partial class ReportIssues : Window
    {
        private List<Issue> reportedIssues = new List<Issue>();
        private string attachedFilePath;

        public ReportIssues()
        {
            InitializeComponent();
        }

        private void AttachFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.png)|*.jpg;*.png|Document files (*.pdf)|*.pdf"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                attachedFilePath = openFileDialog.FileName;
                EngagementLabel.Text = "File attached: " + attachedFilePath;
                ProgressIndicator.Value += 20; // Engage user by showing progress
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string location = LocationInput.Text;
            string category = (CategorySelection.SelectedItem as ComboBoxItem)?.Content.ToString();
            TextRange description = new TextRange(DescriptionBox.Document.ContentStart, DescriptionBox.Document.ContentEnd);

            if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(description.Text.Trim()))
            {
                MessageBox.Show("Please complete all fields before submitting.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Issue newIssue = new Issue
            {
                Location = location,
                Category = category,
                Description = description.Text,
                Attachment = attachedFilePath
            };

            reportedIssues.Add(newIssue);
            MessageBox.Show("Issue reported successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            EngagementLabel.Text = "Thank you for your submission!";
            ProgressIndicator.Value = 100;
            ResetForm();
        }

        private void ResetForm()
        {
            LocationInput.Clear();
            CategorySelection.SelectedIndex = -1;
            DescriptionBox.Document.Blocks.Clear();
            attachedFilePath = null;
            ProgressIndicator.Value = 0;
            EngagementLabel.Text = "Fill the form to submit your issue.";
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}