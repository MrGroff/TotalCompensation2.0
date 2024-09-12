using System;
using System.IO;
using System.Windows.Forms;
namespace TotalCompensation2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a file",
                Filter = "All files (*.*)|*.*", // You can set different filters here
                Multiselect = false // Set to true if you want to allow multiple file selection
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file's path and display it in the TextBox
                string filePath = openFileDialog.FileName;
                textBox1.Text = filePath;
                ProcessFile(filePath);
                // Optionally, you can also use the file path to open the file or process it
                // e.g., ProcessFile(filePath);
            }
        }



        private void ProcessFile(string filePath)
        {
            try
            {
                // Assuming the JsonDataService and EmployeeProcessor are in the same project or referenced
                var jsonDataService = new JsonDataService(filePath);
                JsonData jsonData = jsonDataService.LoadJsonData();

                var employeeProcessor = new EmployeeProcessor(jsonData);
                var reportOutput = employeeProcessor.PrintEmployeeReports();
                richTextBox1.Text = reportOutput;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Optional: Handle changes to the text box if needed
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
    

