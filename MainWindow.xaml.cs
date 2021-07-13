using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XML_Editor
{
    public partial class MainWindow : Window
    {
        Tree t = new Tree();
        string sourceDirectory;
        string inputFileName;
        public MainWindow()
        {
            InitializeComponent();
            //using scroll bar in input textbox
            input_text.AcceptsReturn = true;
            input_text.TextWrapping = TextWrapping.NoWrap;
            ScrollViewer.SetVerticalScrollBarVisibility(input_text, ScrollBarVisibility.Auto);
            ScrollViewer.SetHorizontalScrollBarVisibility(input_text, ScrollBarVisibility.Auto);

            //using scroll bar in output textbox
            output_text.AcceptsReturn = true;
            output_text.TextWrapping = TextWrapping.NoWrap;
            ScrollViewer.SetVerticalScrollBarVisibility(output_text, ScrollBarVisibility.Auto);
            ScrollViewer.SetHorizontalScrollBarVisibility(output_text, ScrollBarVisibility.Auto);
        }

        private void browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog input_file = new OpenFileDialog();

            //input_file.Filter = "txt files (*.txt)|*.txt";
            //input_file.Filter = "xml files (*.xml)|*.xml";
            input_file.FilterIndex = 2;
            input_file.RestoreDirectory = true;

            if (input_file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                output_text.Text = null;
                
                //Get the path of specified file
                var filePath = input_file.FileName;
                inputFileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

                //get the directory name the file is in
                sourceDirectory = System.IO.Path.GetDirectoryName(filePath);

                //Read the contents of the file into a stream
                var fileStream = input_file.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    var fileContent = reader.ReadToEnd();
                    input_text.Text = fileContent;

                    //read from the begginig of the file
                    reader.DiscardBufferedData();
                    reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                    
                    t.insertFile(reader);
                    reader.Close();
                }
            }

        }

        private void format_Click(object sender, RoutedEventArgs e)
        {
            output_text.Text = null;

            string fname = inputFileName + "_Output.xml";
            //get the new file name you want,the Combine method is strongly recommended
            string destFileName = System.IO.Path.Combine(sourceDirectory, fname);

            //Operations operation = new Operations(output_text, fname);
            Operations operation = new Operations(output_text, destFileName, 0);
            //if (File.Exists(destFileName))
            //{
            //    // If file found, delete it    
            //    File.Delete(destFileName);
            //}

            operation.Formating(t.getTreeRoot());
            show_output(destFileName);
        }

        private void convert_Click(object sender, RoutedEventArgs e)
        {
            output_text.Text = null;

            string fname = inputFileName + "_Output.json";
            //get the new file name you want,the Combine method is strongly recommended
            string destFileName = System.IO.Path.Combine(sourceDirectory, fname);

            //Operations operation = new Operations(output_text, fname);
            Operations operation = new Operations(output_text, destFileName, 0);
            operation.write("{", true);
            operation.Converting(t.getTreeRoot(), t.getTreeRoot());
            operation.write("}", true);

            show_output(destFileName);
        }

        void show_output(string destFileName)
        {
            output_text.Text = null;

            using (StreamReader reader = new StreamReader(destFileName))
            {
                var fileContent = reader.ReadToEnd();
                output_text.Text = fileContent;
                reader.Close();
            }
        }

        private void min_Click(object sender, RoutedEventArgs e)
        {
            output_text.Text = null;

            string fname = inputFileName + "_min.xml";
            //get the new file name you want,the Combine method is strongly recommended
            string destFileName = System.IO.Path.Combine(sourceDirectory, fname);

            //Operations operation = new Operations(output_text, fname);
            Operations operation = new Operations(output_text, destFileName, 0);
            operation.Minifying(t.getTreeRoot());
            show_output(destFileName);
        }

    }
}
