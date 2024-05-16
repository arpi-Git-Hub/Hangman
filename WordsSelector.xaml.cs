using System.IO;
using System.Windows;

namespace Hangman
{
    public partial class WordsSelector : Window
    {
        public WordsSelector()
        {
            InitializeComponent();
            DirectoryInfo d = new DirectoryInfo("../../../words");
            FileInfo[] Files = d.GetFiles("*.txt", SearchOption.AllDirectories);
            foreach (FileInfo file in Files)
            {
                Topics.Items.Add(file.Name.Remove(file.Name.Length-4));
            }
        }

        public static List<string> wordList = new();
        public static string WordNow;
        string fileName;
        public static bool isReady = false;

        private void StartGame(object sender, RoutedEventArgs e)
        {

            if (Topics.SelectedItem.ToString() == "Hungarian")
            {
                MainWindow.AllLetters = "AÁBCDEÉFGHIÍJKLMNOÓÖŐPQRSTUÚÜŰVWXYZ".ToCharArray().ToList();
            }
            else
            {
                MainWindow.AllLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();
            }

            fileName = Topics.SelectedItem.ToString();

            wordList.Clear();
            StreamReader sr = new StreamReader($"../../../words/{fileName}.txt");
            while (!sr.EndOfStream)
            {
                wordList.Add(sr.ReadLine());
            }

            Random r = new Random();
            int num = r.Next(0, wordList.Count);
            WordNow = wordList[num];

            isReady = true;
            Close();

        }
    }
}
