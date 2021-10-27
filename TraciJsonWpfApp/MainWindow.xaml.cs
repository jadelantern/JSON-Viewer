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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TraciJsonWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tab1.Content = new RichTextBox();
        }

        public void OpenFile(string filepath)
        {
            var fileInfo = new FileInfo(filepath);
            string fileName = fileInfo.Name;
            string fileContents = File.ReadAllText(filepath);

            TabItem tab = new TabItem();
            tab.Header = fileName;
            RichTextBox richTextBox = new RichTextBox();

            tab.Content = richTextBox;
            documentTabControl.Items.Add(tab);
        }

        public bool IsFileOpen(string filepath)
        {
            return GetTabByPath(filepath) != null;
        }

        public TabItem GetTabByPath(string filepath)
        {
            var fileInfo = new FileInfo(filepath);
            string fileName = fileInfo.Name;

            foreach (TabItem tab in documentTabControl.Items)
            {
                if (tab.Header.ToString().ToLower() == fileName.ToLower())
                {
                    return tab;
                }
            }
            return null;
        }
    }
}
