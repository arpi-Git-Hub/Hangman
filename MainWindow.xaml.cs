using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Hangman
{
    public partial class MainWindow : Window
    {
        private List<Label> ListofLabel;
        private List<Canvas> ListofBar;

        private List<UIElement> TheHangMan;
        private readonly TextBlock WrongBox;
        private int WrongGuesses;
        private readonly int MaximumGuess = 10;
        public static List<char> AllLetters;

        public MainWindow()
        {
            InitializeComponent();
            WrongBox = new TextBlock
            {
                Margin = new Thickness(250, 30, 0, 0),
                FontSize = 20,
                TextWrapping = TextWrapping.Wrap
            };
            Grid.Children.Add(WrongBox);
            GuessBtn.IsEnabled = false;

        }

        private void StartBtnClick(object sender, RoutedEventArgs e)
        {
            WordsSelector wordsSelector = new();
            wordsSelector.ShowDialog();

            
            if(WordsSelector.isReady)
            {
                ListofLabel = new();
                ListofBar = new();
                SwapButtonState();

                CharList.Items.Clear();
                foreach (char i in AllLetters)
                {
                    CharList.Items.Add(i);
                }
                Grid.SetRow(WrongBox, 0);
                Grid.SetColumn(WrongBox, 1);
                WrongBox.Text = "";

                int Space = 0;
                foreach (char c in WordsSelector.WordNow)
                {
                    Label lbl = new();
                    Canvas can = new Canvas
                    {
                        Background = Brushes.Black,
                        Width = 15,
                        Height = 3,
                        Margin = new Thickness(-220 + Space, 0, 0, 0)
                    };
                    Grid.SetRow(can, 1);
                    Grid.SetColumn(can, 1);
                    Grid.Children.Add(can);

                    lbl.Margin = new Thickness(-220 + Space, 28, 0, 0);
                    lbl.Visibility = Visibility.Hidden;
                    lbl.Width = 60;
                    lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                    lbl.FontSize = 20;
                    lbl.Content = c;

                    Grid.SetRow(lbl, 1);
                    Grid.SetColumn(lbl, 1);
                    Grid.Children.Add(lbl);

                    ListofLabel.Add(lbl);
                    ListofBar.Add(can);
                    Space += 60;
                }

                TheHangMan = new() { Ground, Stand, Top, Rope, Head, Body, LeftHand, RightHand, LeftLeg, RightLeg };
                foreach (UIElement ele in TheHangMan)
                {
                    ele.Visibility = Visibility.Hidden;
                }
                WrongGuesses = 0;
                ShowWrongGuesses.Content = MaximumGuess - WrongGuesses;
            }
            
        }

        private bool HasWon(List<Label> Lol)
        {
            bool HasHidden = false;
            foreach (Label l in Lol)
            {
                if (l.Visibility == Visibility.Hidden)
                {
                    HasHidden = true;
                }
            }
            return !HasHidden;
        }

        private void SwapButtonState()
        {
            StartBtn.IsEnabled = !StartBtn.IsEnabled;
            GuessBtn.IsEnabled = !GuessBtn.IsEnabled;
        }

        private void ClearTable()
        {
            foreach (Label u in ListofLabel)
            {
                u.Content = "";
            }

            foreach (Canvas c in ListofBar)
            {
                c.Visibility = Visibility.Hidden;
            }
        }

        private void GuessBtnClick(object sender, RoutedEventArgs e)
        {
            bool flag = false;

            if (CharList.SelectedItem != null && WrongGuesses < MaximumGuess)
            {
                foreach (Label l in ListofLabel)
                {
                    if ((char)l.Content == (char)CharList.SelectedItem)
                    {
                        l.Visibility = Visibility.Visible;
                        flag = true;
                    }
                }

                if (!flag)
                {
                    WrongBox.Text += " " + (char)CharList.SelectedItem;
                    DrawOneStep();
                }
                if (CharList.SelectedItem != null)
                {
                    CharList.Items.Remove(CharList.SelectedItem);
                }
            }

            if (HasWon(ListofLabel))
            {
                MessageBox.Show("You won!");
                SwapButtonState();
                ClearTable();
            }
        }


        private void DrawOneStep()
        {
            if (WrongGuesses < MaximumGuess)
            {
                if (TheHangMan[WrongGuesses].Visibility == Visibility.Hidden)
                {
                    TheHangMan[WrongGuesses].Visibility = Visibility.Visible;
                    WrongGuesses++;
                    ShowWrongGuesses.Content = MaximumGuess - WrongGuesses;
                    if (WrongGuesses >= MaximumGuess)
                    {
                        MessageBox.Show("Game Over!");
                        MessageBox.Show("The correct word is " + WordsSelector.WordNow);
                        SwapButtonState();
                        ClearTable();
                    }
                }
            }
        }

    }
}