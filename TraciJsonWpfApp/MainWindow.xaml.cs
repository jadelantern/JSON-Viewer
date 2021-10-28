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
            OpenFile(@"C:\Users\natel\Google Drive\[i Development\C#\Mentor\Thomas and Nate\testOne.txt");
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                OpenFile(file);
            }
        }

        public void OpenFile(string filepath)
        {
            var fileInfo = new FileInfo(filepath);
            string fileName = fileInfo.Name;
            string fileContents = File.ReadAllText(filepath);

            TabItem tab = new TabItem();
            tab.Header = fileName;
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Selection.Text = fileContents;

            tab.Content = richTextBox;
            documentTabControl.Items.Add(tab);
            documentTabControl.SelectedIndex = documentTabControl.Items.Count - 1;
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
        public TabItem GetTabByHeader(string header)
        {
            foreach (TabItem tab in documentTabControl.Items)
            {
                if (tab.Header.ToString().ToLower() == header.ToLower())
                {
                    return tab;
                }
            }
            return null;
        }

        public TabItem GetOpenedTab()
        {
            return (TabItem)documentTabControl.SelectedItem;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                var tab = GetOpenedTab();
                var rtb = (RichTextBox)tab.Content;
                MessageBox.Show(rtb.Selection.Text);
            }
        }
    }
}
