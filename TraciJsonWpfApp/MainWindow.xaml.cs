using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
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

        private async void Window_Drop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                await OpenFile(file);
            }
        }
        public async Task OpenFile(string filepath)
        {
            if (IsFileOpen(filepath))
            {
                MessageBox.Show("");
                int index = GetTabIndex(filepath);
                documentTabControl.SelectedIndex = index;
                return;
            }

            var fileInfo = new FileInfo(filepath);
            string fileName = fileInfo.Name;
            string fileContents = File.ReadAllText(filepath);

            JsonTab tab = new JsonTab();
            tab.Header = fileName;
            tab.filepath = filepath;
            TextEditor avalonTextEditor = CreateTextEditor();
            avalonTextEditor.Text = fileContents;
            var fold = FoldingManager.Install(avalonTextEditor.TextArea);

            BracketPairFinder bpFinder = new BracketPairFinder(fileContents, '{', '}');
            var brackets = await bpFinder.GetBracketPairsAsync();
            foreach (var bracket in brackets)
            {
                fold.CreateFolding(bracket.Key, bracket.Value);
            }


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

            var highlight =  ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("Json");
            avalonTextEditor.SyntaxHighlighting = highlight;

            return avalonTextEditor;
        }

        private void AvalonTextEditor_TextChanged(object sender, EventArgs e)
        {
            var avalonTxtEd = (TextEditor)sender; //casting** 
            string text = avalonTxtEd.Text;
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

        public JsonTab GetTabByPath(string filepath)
        {
            int index = GetTabIndex(filepath);
            if (index != -1)
                return (JsonTab) documentTabControl.Items.GetItemAt(index);

            return null;
        }
        

        public JsonTab GetOpenedTab()
        {
            return (JsonTab)documentTabControl.SelectedItem;
        }

        public int GetTabIndex(string filepath)
        {
            for (int i = 0; i < documentTabControl.Items.Count; i++)
            {
                if (((JsonTab)documentTabControl.Items.GetItemAt(i)).filepath.ToLower() == filepath.ToLower())
                {
                    return i;
                }
            }
            return -1;
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

        private void fileButton_Click(object sender, RoutedEventArgs e)
        {
            fileMenu.IsSubmenuOpen = !fileMenu.IsSubmenuOpen;
        }

        private async void browseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Text files (*.txt;*.json)|*.txt;*.json|All files (*.*)|*.*";
            if (ofd.ShowDialog()==true)
            {
                foreach (var filename in ofd.FileNames)
                {
                    await OpenFile(filename);
                }
            }
        }

        private void documentTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (documentTabControl.SelectedIndex == -1)
                return;

            string json = GetJsonFromTab(documentTabControl.SelectedIndex);
            if (json == null)
                return;

            UpdateJsonLabel(json.IsValidJSON());
        }

        public string GetJsonFromTab(int tabIndex)
        {
            JsonTab tab = (JsonTab)documentTabControl.Items.GetItemAt(tabIndex);
            if (tab == null)
                return null;

            TextEditor jsonEditor = (TextEditor)tab.Content;
            if (jsonEditor == null)
                return null;

            return jsonEditor.Text;
        }
    }
}
