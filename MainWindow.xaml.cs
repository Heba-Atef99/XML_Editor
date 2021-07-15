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
        string sourceDirectory = null;
        string inputFileName = null;
        string c_file = null;
        string last_output = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        ~MainWindow()
        {
            if (File.Exists(c_file))
            {
                // If file found, delete it    
                File.Delete(c_file);
            }
        }

        private void browse_Click(object sender, RoutedEventArgs e)
        {
            success_msg.Visibility = Visibility.Hidden;
            error_msg.Visibility = Visibility.Hidden;
            num_errors.Visibility = Visibility.Hidden;
            OpenFileDialog input_file = new OpenFileDialog();

            input_file.Filter = "xml files (*.xml)|*.xml";
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

                    //read from the begginig of the file
                    reader.DiscardBufferedData();
                    reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

                    //call consistency 
                    c_file = "consistent_file.txt";
                    last_output = c_file;
                    Consistency operation = new Consistency(c_file, 0);
                    int error_num = operation.Consistent(reader);
                    if (error_num > 0)
                    {
                        error_msg.Visibility = Visibility.Visible;
                        num_errors.Visibility = Visibility.Visible;
                        num_errors.Text = error_num.ToString();
                    }
                    reader.Close();
                }

                using (StreamReader reader2 = new StreamReader(c_file))
                {
                    t.insertFile(reader2);
                    reader2.Close();
                }
            }
        }

        private void format_Click(object sender, RoutedEventArgs e)
        {
            if(vaildate_clicking())
            {
                //get the new file name you want
                string destFileName = System.IO.Path.Combine(sourceDirectory, inputFileName + "_Format_Output.xml");
                last_output = destFileName;
                Formatting operation = new Formatting(destFileName, 0);

                //format the tree to look well indented
                operation.Format(t.getTreeRoot());

                //show the result in output textbox
                show_output(destFileName);
            }
        }

        private void convert_Click(object sender, RoutedEventArgs e)
        {
            if (vaildate_clicking())
            {
                //get the new file name you want
                string destFileName = System.IO.Path.Combine(sourceDirectory, inputFileName + "_JSON_Output.json");
                last_output = destFileName;
                Converting_To_JSON operation = new Converting_To_JSON(destFileName, 0);

                //convert the tree to a json file
                operation.write("{", true);
                operation.Convert(t.getTreeRoot(), t.getTreeRoot());
                operation.write("}", true);

                //show the result in output textbox
                show_output(destFileName);
            }
        }


        private void min_Click(object sender, RoutedEventArgs e)
        {
            if (vaildate_clicking())
            {
                //get the new file name you want
                string destFileName = System.IO.Path.Combine(sourceDirectory, inputFileName + "_min.xml");
                last_output = destFileName;
                Minifying operation = new Minifying(destFileName, 0);

                //minify the tree in one line
                operation.Minify(t.getTreeRoot());

                //show the result in output textbox
                show_output(destFileName);
            }
        }

        private void compress_Click(object sender, RoutedEventArgs e)
        {
            if (vaildate_clicking())
            {

                string extension = System.IO.Path.GetExtension(last_output);
                //get the new file name you want
                string destFileName = System.IO.Path.Combine(sourceDirectory, inputFileName + "_Compressed_Output" + extension);

                Compressing comp = new Compressing(last_output, destFileName);
                comp.encoding();
                show_output(destFileName);
            }
        }

        private void decompress_Click(object sender, RoutedEventArgs e)
        {
            success_msg.Visibility = Visibility.Hidden;
            error_msg.Visibility = Visibility.Hidden;
            num_errors.Visibility = Visibility.Hidden;
            OpenFileDialog input_file = new OpenFileDialog();

            input_file.Filter = "xml files (*.xml)|*.xml" + "|json files (*.json)|*.json";
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

                    //read from the begginig of the file
                    reader.DiscardBufferedData();
                    reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

                    string extension = System.IO.Path.GetExtension(last_output);
                    string destFileName = System.IO.Path.Combine(sourceDirectory, inputFileName + "_Decompressed_Output" + extension);
                    Compressing comp = new Compressing(filePath, destFileName);
                    comp.decoding();
                    reader.Close();
                    show_output(destFileName);
                }
            }
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

        private bool vaildate_clicking()
        {
            if(c_file == null)
            {
                string message = "You have not chosen a file to do this operation on it. Press browse to select an XML file first.";
                string caption = "Error Detected in Input";

                System.Windows.MessageBox.Show(message, caption);
                return false;
            }

            return true;
        }
    }
}
