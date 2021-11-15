using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
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
using TraciJsonWpfApp.Extentions;

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
            TextEditor avalonTextEditor = CreateTextEditor();
            avalonTextEditor.Text = fileContents;

            var fold = FoldingManager.Install(avalonTextEditor.TextArea);
            fold.CreateFolding(40, 60);
            

            /*RichTextBox richTextBox = new RichTextBox();
            richTextBox.TextChanged += RichTextBox_TextChanged;
            richTextBox.Selection.Text = fileContents;
            tab.Content = richTextBox;
*/
            tab.Content = avalonTextEditor;
            documentTabControl.Items.Add(tab);
            documentTabControl.SelectedIndex = documentTabControl.Items.Count - 1;
        }

        public TextEditor CreateTextEditor()
        {
            TextEditor avalonTextEditor = new TextEditor();
            avalonTextEditor.TextChanged += AvalonTextEditor_TextChanged;
            avalonTextEditor.FontFamily = new FontFamily("Arial");
            avalonTextEditor.FontSize = 15;
            avalonTextEditor.ShowLineNumbers = true;
            avalonTextEditor.Options.HighlightCurrentLine = true;

            return avalonTextEditor;
        }

        private void AvalonTextEditor_TextChanged(object sender, EventArgs e)
        {
            var avalonTxtEd = (TextEditor)sender; //casting** 
            string text = avalonTxtEd.Text;
            bool isValid = text.IsValidJSON();
            UpdateJsonLabel(isValid);
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var rtb = (RichTextBox)e.OriginalSource; //casting** 
            string text = rtb.GetText();
            bool isValid = text.IsValidJSON();
            UpdateJsonLabel(isValid);
        }

        private void UpdateJsonLabel(bool isJSONValid)
        {
            if (isJSONValid)
            {
                jsonValidLabel.Text = "JSON Valid: True";
                jsonValidLabel.Background = new SolidColorBrush(Color.FromRgb(149,241,149));
            }
            else
            {
                jsonValidLabel.Text = "JSON Valid: False";
                jsonValidLabel.Background = new SolidColorBrush(Color.FromRgb(251,10,10));
            }
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

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
