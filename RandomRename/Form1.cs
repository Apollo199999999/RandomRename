using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace RandomRename
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public bool canStart = false;
        private void button1_Click(object sender, EventArgs e)
        {
            //init a new fbd
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult result = fbd.ShowDialog();

            //get the result
            if (result == DialogResult.OK)
            {
                //set the text to the dir
                directorylabel.Text = fbd.SelectedPath;

                //change the canStart var
                canStart = true;
            }
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            if (canStart == true)
            {
                //create a new list to store the filenames
                List<string> filenamelist = new List<string> { };

                //iterate through files in dir, getting the filename of each file
                foreach (string filename in Directory.GetFiles(directorylabel.Text))
                {
                    //add the filename to the list
                    filenamelist.Add(Path.GetFileName(filename));
                }

                //do the same thing, but this time, rename the files
                foreach (string filename in Directory.GetFiles(directorylabel.Text))
                {
                    //copy the file to another dir
                    Directory.CreateDirectory("C:\\output\\temp");
                    File.Copy(filename, Path.Combine("C:\\output\\temp", Path.GetFileName(filename)));

                    //pick a random item from the filenamelist
                    var random = new Random();
                    int index = random.Next(filenamelist.Count);

                    //init two locations
                    var location1 = Path.Combine("C:\\output\\temp", Path.GetFileName(filename));

                    //rename
                    System.Diagnostics.Process.Start("cmd.exe", "/C rename " + location1 + " " + filenamelist[index]);

                    await Task.Delay(500);
                    
                    //Move to original dir
                    File.Move(Path.Combine("C:\\output\\temp", Path.GetFileName(filenamelist[index])), Path.Combine("C:\\output\\", Path.GetFileName(filenamelist[index])));

                    //remove the name from list
                    filenamelist.RemoveAt(index);
                    


                }

            }

        }
    }
}
