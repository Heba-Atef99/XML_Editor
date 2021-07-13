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
        }

        private void browse_Click(object sender, RoutedEventArgs e)
        {
            success_msg.Visibility = Visibility.Hidden;
            error_msg.Visibility = Visibility.Hidden;
            num_errors.Visibility = Visibility.Hidden;
            OpenFileDialog input_file = new OpenFileDialog();

            //input_file.Filter = "txt files (*.txt)|*.txt";
            //input_file.Filter = "xml files (*.xml)|*.xml";

            input_file.FilterIndex = 2;
            input_file.RestoreDirectory = true;

            if (input_file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //remove any text writen before in output textbox when browsing new file
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
                    //reader.Close();

                    //call consistency 
                    string c_file = "consistent_file.txt";
                    Operations operation = new Operations(c_file, 0);
                    int error_num = operation.Consistency(reader);
                    if (error_num > 0)
                    {
                        error_msg.Visibility = Visibility.Visible;
                        num_errors.Text = error_num.ToString();
                    }

                    using (StreamReader reader2 = new StreamReader(c_file))
                    {
                        t.insertFile(reader2);
                        reader2.Close();
                        if (File.Exists(c_file))
                        {
                            // If file found, delete it    
                            File.Delete(c_file);
                        }
                    }

                    //read from the begginig of the file
                    //reader.DiscardBufferedData();
                    //reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

                    //t.insertFile(reader);
                    //reader.Close();
                }
            }
        }

        private void format_Click(object sender, RoutedEventArgs e)
        {
            //get the new file name you want
            string destFileName = System.IO.Path.Combine(sourceDirectory, inputFileName + "_Output.xml");

            Operations operation = new Operations(destFileName, 0);

            //format the tree to look well indented
            operation.Formating(t.getTreeRoot());

            //show the result in output textbox
            show_output(destFileName);
        }

        private void convert_Click(object sender, RoutedEventArgs e)
        {
            //get the new file name you want
            string destFileName = System.IO.Path.Combine(sourceDirectory, inputFileName + "_Output.json");

            Operations operation = new Operations(destFileName, 0);
            
            //convert the tree to a json file
            operation.write("{", true);
            operation.Converting(t.getTreeRoot(), t.getTreeRoot());
            operation.write("}", true);

            //show the result in output textbox
            show_output(destFileName);
        }


        private void min_Click(object sender, RoutedEventArgs e)
        {
            //get the new file name you want
            string destFileName = System.IO.Path.Combine(sourceDirectory, inputFileName + "_min.xml");

            Operations operation = new Operations(destFileName, 0);
            
            //minify the tree in one line
            operation.Minifying(t.getTreeRoot());
           
            //show the result in output textbox
            show_output(destFileName);
        }

        private void show_output(string destFileName)
        {
            //remove any text writen before in output textbox
            output_text.Text = null;


            using (StreamReader reader = new StreamReader(destFileName))
            {
                var fileContent = reader.ReadToEnd();
                output_text.Text = fileContent;
                reader.Close();
            }

            success_msg.Visibility = Visibility.Visible;
        }
    }
}
