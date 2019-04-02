using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace hangman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<char> userGuesses = new List<char>();
        private int numberOfGuesses = 0;
        private int wrongGuesses = 0;
        
        // Contains path to the image file corresponding to the current hanging status
        public string HangmanImage
        {
            get
            {
                return "/images/stage" + wrongGuesses.ToString() + ".png";
            }
            set
            {

            }
        }

        public string UserGuesses
        {
            get
            {
                string result = "";
                foreach (char letter in userGuesses)
                {
                    result += " " + letter;
                }
                return result;
            }
            set
            {

            }
        }
        private string hangmanWord;

        //Censors non-guessed letters apart from the symbols (' - , . : ; and space)
        public string HangmanWord
        {
            get
            {
                string result="";
                char[] symbols = { '\'', '-', ':', ',', ' ' };
                foreach (char element in hangmanWord)
                {
                    if (userGuesses.Contains(Char.ToLower(element)) || symbols.Contains(element))
                    {
                        result += element;
                    }
                    else
                    {
                        result += "*";
                    }
                        
                }

                return result;
            }
            set
            {
            }
        }

        #region init&load
        public MainWindow()
        {
            string[] terms = File.ReadAllLines(@"~/../../../resources/terms.txt");
            Random randomNumber = new Random();
            hangmanWord = terms[randomNumber.Next(0, terms.Length)];
            InitializeComponent();
            DataContext = this;
        }
        #endregion

        private void ReadUserInput(object sender, TextChangedEventArgs e)
        {
            if (enterLetterTextbox.Text != "")
            {
                //Take care of entering same letter multiple times
                if (userGuesses.Contains(enterLetterTextbox.Text.ToLower()[0]))
                {
                    MessageBox.Show(String.Format("You have already tried {0}", enterLetterTextbox.Text.ToLower())); }
                else
                {
                    //Add entry to already entered letters and increase number of guesses
                    userGuesses.Add(enterLetterTextbox.Text.ToLower()[0]);
                    enteredGuessesContentLabel.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    numberOfGuesses++;
                    
                    //Wrong letter
                    if (!hangmanWord.ToLower().Contains(enterLetterTextbox.Text.ToLower()[0]))
                    {
                        wrongGuesses++;
                        hangingStatus.GetBindingExpression(Image.SourceProperty).UpdateTarget();
                    }
                }
                //If not game over
                if (wrongGuesses < 10)
                {
                    enterLetterTextbox.Clear();
                    currentGuessLabel.GetBindingExpression(Label.ContentProperty).UpdateTarget();
                    if (hangmanWord.ToLower() == HangmanWord.ToLower())
                    {
                        MessageBox.Show(String.Format("Congratulations! You have guessed {0} in {1} entries with {2} errors!", hangmanWord, numberOfGuesses, wrongGuesses));
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("You lost!");
                    this.Close();
                }
            }

        }

        //For tracking changes in the "enter letter" textbox
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}
