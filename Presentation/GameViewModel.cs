using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using SimpleConcentration.Models;

namespace SimpleConcentration.Presentation
{
    public class GameViewModel : ObservableObject
    {
        private Player1 _player1;
        private Player2 _player2;
        private Random _random;
        private GameBoard _gameboard;
        private string _messageboxcontent;
        public int[] HiddenCards = new int[30];

        public Player1 Player1
        {
            get { return _player1; }
            set
            {
                _player1 = value;
                OnPropertyChanged(nameof(Player1));
            }
        }
        public Player2 Player2
        {
            get { return _player2; }
            set
            {
                _player2 = value;
                OnPropertyChanged(nameof(Player2));
            }
        }
        public GameBoard GameBoard
        {
            get { return _gameboard; }
            set
            {
                _gameboard = value;
                OnPropertyChanged(nameof(GameBoard));
            }
        }
        public string MessageBoxContent
        {
            get { return _messageboxcontent; }
            set
            {
                _messageboxcontent = value;
                OnPropertyChanged(nameof(MessageBoxContent));
            }
        }
        public GameViewModel()
        {
            InitializeGame();
        }
        private void InitializeGame()
        {
            //
            // initialize players and their data along with the Game Board and the saved values to handle multiple turns per player 
            // that either creates a match or not. 
            //
            _player1 = new Player1();
            Player1.Name = "Bill";
            Player1.Pairs = 0;
            Player1.Wins = 0;
            _player2 = new Player2();
            Player2.Name = "Ted";
            Player2.Pairs = 0;
            Player2.Wins = 0;
            MessageBoxContent = "Player 1's 1st Turn";

            _gameboard = new GameBoard();
            _gameboard.CurrentState = GameBoard.GameBoardState.Player1Turn1;
            _gameboard.SavedValue = 0;
            _gameboard.SavedRow1 = 99;
            _gameboard.SavedColumn1 = 99;
            _gameboard.SavedRow2 = 99;
            _gameboard.SavedColumn2 = 99;

            InitializeCards();
        }
        public void InitializeCards()
        {
            //
            // Lots of fun code here. Ultimately, I need a random list of 30 numbers from 1 to 15 where each is repeated twice to represent pairs. 
            // This list will be mapped to the screen grid that has 6 rows of 5 cards (30 total) and for the game to work, the hidden cards must have exactly 
            // one pair of each number from 1 to 30. 
            // To do this:
            // I'm building an array that holds 30 random values from 1 to 30. Note that I have to keep looping through 
            // generating randoms until I'm sure that I've found the exact values from 1 to 30 becasue repeats are possible. 
            //
            _random = new Random();
            int cardvalue = _random.Next(1, 31);
            bool arrayfull = false;
            bool matchfound = false;
            bool keepchecking = true;
            int arrayidx = 0;
            do
            {
                cardvalue = _random.Next(1, 31);
                //
                // check to see if it has already been used
                //
                int i = 0;
                matchfound = false;
                keepchecking = true;
                do
                {
                    if (HiddenCards[i] == cardvalue)
                    {
                        matchfound = true;
                        keepchecking = false;
                    }
                    if (i == 29)
                    {
                        keepchecking = false;
                    }
                    ++i;
                } while (keepchecking);
                if (!matchfound)
                {
                    HiddenCards[arrayidx] = cardvalue;
                    ++arrayidx;
                }
                if (arrayidx > 29)
                {
                    arrayfull = true;
                }
            } while (!arrayfull);
            //
            // now that the array is full with random numbers from 1-30, I need to make sure that there are 2 of each 
            // number from 1 to 15 so that I have 15 pairs. Just subtract 15 from all the values > 15.
            //
            for (arrayidx = 0; arrayidx < 30; arrayidx++)
            {
                if (HiddenCards[arrayidx] > 15)
                {
                    HiddenCards[arrayidx] = HiddenCards[arrayidx] - 15;
                }
            }
        }
        public void CardClicked(int row, int column)
        {
            if (_gameboard.CurrentState == GameBoard.GameBoardState.NoMatch)
            {
                NoMatch();
            }
            else
            {
                if (_gameboard.CardPositionAvailable(new CardPosition(row, column)))
                {

                    if (_gameboard.CurrentState == GameBoard.GameBoardState.Player1Turn1 || _gameboard.CurrentState == GameBoard.GameBoardState.Player2Turn1)
                    {
                        FirstTurn(row, column);
                    }
                    else
                    {
                        SecondTurn(row, column);
                    }
                }
            }
            UpdateCurrentState();
        }
        public void FirstTurn(int row, int column)
        {
            //
            // The Game Board is laid out as 6 rows of 5 cards each. We set up the values for those in the 
            // single dimension array of 30 elements called HiddenCards. Remembering that arrays start at index=0, you
            // can map the Game Board array to the Hidden Cards value by taking the row, multiplying it by 5 and adding the column.
            // so Game Board row 0, column 0 is HiddenCard index 0, row 1, column 2 is index 7
            // and the last position (row=5, column=4) is the last value in the HiddenCard array - 29.
            //
            int arrayidx = (row * 5) + column;
            //
            // on the first turn for either player, you expose the card value and save it along with the position
            // to use on the second turn to to handle a match/no match condition
            //
            GameBoard.CurrentBoard[row][column] = HiddenCards[arrayidx].ToString();
            OnPropertyChanged(nameof(GameBoard));
            _gameboard.SavedRow1 = row;
            _gameboard.SavedColumn1 = column;
            _gameboard.SavedValue = HiddenCards[arrayidx];

            if (_gameboard.CurrentState == GameBoard.GameBoardState.Player1Turn1)
            {
                MessageBoxContent = "Player 1's Next Turn";
                _gameboard.CurrentState = GameBoard.GameBoardState.Player1Turn2;
            }
            else
            {
                MessageBoxContent = "Player 2's Next Turn";
                _gameboard.CurrentState = GameBoard.GameBoardState.Player2Turn2;
            }
        }
        
        public void SecondTurn(int row, int column)
        {
            //
            // calculate the index to the card value - see comments in FirstTurn for more explanation 
            //
            int arrayidx = (row * 5) + column;

            GameBoard.CurrentBoard[row][column] = HiddenCards[arrayidx].ToString();
            OnPropertyChanged(nameof(GameBoard));
            if (_gameboard.SavedValue == HiddenCards[arrayidx])
            {
                //
                // its a match
                //
                GotaMatch();
            }
            else
            {
                //
                // No match found so change the game state, save the 2nd card position and save the player
                //
                _gameboard.SavedGameBoardState = _gameboard.CurrentState.ToString();
                _gameboard.CurrentState = GameBoard.GameBoardState.NoMatch;
                _gameboard.SavedRow2 = row;
                _gameboard.SavedColumn2 = column;
                MessageBoxContent = "No Match, Click any card to Continue";
            }
        }
        public void UpdateCurrentState()
        {
            _gameboard.UpdateGameboardState();
        }
        public void NoMatch()
        {
            //
            // No match so reset/blank out both exposed cards and all the save values and switch to the other player's turn 
            //
            GameBoard.CurrentBoard[_gameboard.SavedRow1][_gameboard.SavedColumn1] = "";
            GameBoard.CurrentBoard[_gameboard.SavedRow2][_gameboard.SavedColumn2] = "";
            OnPropertyChanged(nameof(GameBoard));
            _gameboard.SavedRow1 = 99;
            _gameboard.SavedColumn1 = 99;
            _gameboard.SavedRow2 = 99;
            _gameboard.SavedColumn2 = 99;
            _gameboard.SavedValue = 0;
            if (_gameboard.SavedGameBoardState == "Player1Turn2")
            {
                _gameboard.CurrentState = GameBoard.GameBoardState.Player2Turn1;
                MessageBoxContent = "Player 2's First Pick";
            }
            else
            {
                _gameboard.CurrentState = GameBoard.GameBoardState.Player1Turn1;
                MessageBoxContent = "Player 1's First Pick";
            }
        }
            //
            // before I added a NoMatch state, I gave the players a messagebox to see what the 2nd card turned over was. But now,
            // both cards are shown on a no match and any card position must be clicked to continue.
            //MessageBox.Show("No Match, Last Card Was " + HiddenCards[arrayidx].ToString() + " at row " + (row + 1).ToString() + " and column " + (column + 1).ToString());
            //
        public void GotaMatch()
        {
            // 
            // update the pairs count of the winner and let the same player go again
            //
            if (_gameboard.CurrentState == GameBoard.GameBoardState.Player1Turn2)
            {
                MessageBoxContent = "Player 1 Matched, Go Again";
                _gameboard.CurrentState = GameBoard.GameBoardState.Player1Turn1;
                ++Player1.Pairs;
            }
            else
            {
                MessageBoxContent = "Player 2 Matched, Go Again";
                _gameboard.CurrentState = GameBoard.GameBoardState.Player2Turn1;
                ++Player2.Pairs;
            }
            //
            // Find out if game is over (total pairs must be 15)
            //
            if (Player1.Pairs + Player2.Pairs == 15)
            {
                GotaWinner();
            }
            else
            {
                //
                // reset for 1st turn
                //
                _gameboard.SavedRow1 = 99;
                _gameboard.SavedColumn1 = 99;
                _gameboard.SavedRow2 = 99;
                _gameboard.SavedColumn2 = 99; 
                _gameboard.SavedValue = 0;
             }
        }
        public void GotaWinner()
        {
            string diagboxmessage;
            if (Player1.Pairs > Player2.Pairs)
            {
                MessageBoxContent = "Player 1 Won the Game";
                diagboxmessage = "Player 1 Won the Game";
                _gameboard.CurrentState = GameBoard.GameBoardState.Player1Turn1;
                ++Player1.Wins;
            }
            else
            {
                MessageBoxContent = "Player 2 Won the Game";
                diagboxmessage = "Player 2 Won the Game";
                _gameboard.CurrentState = GameBoard.GameBoardState.Player1Turn1;
                ++Player2.Wins;
            }
            Player1.Pairs = 0;
            Player2.Pairs = 0;
            //
            // I threw this in to make it more obvious that the player got a win
            //
            MessageBox.Show(diagboxmessage);
        }
        internal void GameCommand(string commandName)
        {
            //
            // handle the game state
            //            
            switch (commandName)
            {
                case "HowToPlay":
                    break;
                case "GimmeaHint":
                    FindaPair();
                    break;
                case "NewGame":
                    _gameboard.InitializeGameboard();
                    OnPropertyChanged(nameof(GameBoard));
                    _gameboard.CurrentState = GameBoard.GameBoardState.Player1Turn1;
                    break;
                case "SaveGame":
                    break;
                case "Quit":
                    // add code to quit
                    break;
                default:
                    // add code to handle exception
                    MessageBox.Show("Got a command name that the code doesn't handle: " + commandName);
                    break;
            }
        }
        public void FindaPair()
        {
            //
            // More fun code. I gave the user a hint feature. First, I find the first card that hasn't been turned over yet (Top down, left to right) and get it's value.
            // Then I need to find the position (row, column) of it's match. Earlier comments have explained how the card positions are mapped to the card values.
            // So first: find the first card not yet turned over 
            //
            int i = 0;
            int j = 0;
            bool emptyspotfound = false;
            int valuetomatch = 0;
            int arrayidx = 0;
            string row1 = "";
            string col1 = "";

            do
            {
                if (GameBoard.CurrentBoard[i][j] == "")
                {
                    arrayidx = (i * 5) + j;
                    valuetomatch = HiddenCards[arrayidx];
                    row1 = (i + 1).ToString();
                    col1 = (j + 1).ToString();
                    emptyspotfound = true;
                }
                //
                // you are cycling through a 6x5 2-dimension array (i,j) so increment to next position 
                //
                ++j;
                if (j > 4)
                {
                    ++i;
                    j = 0;
                }
                if (i > 5)
                {
                    //
                    // this should never happen
                    //
                    MessageBox.Show("Board is fully exposed, can't find an empty spot to match with");
                    valuetomatch = 99;
                    emptyspotfound=true;
                }
            } while(!emptyspotfound);

            if (valuetomatch != 99)
            {
                //
                // now find the matching value in the hidden cards starting with the next value in the array
                //
                ++arrayidx;
                bool matchfound = false;
                do
                {
                    if (HiddenCards[arrayidx] == valuetomatch)
                    {
                        //
                        // convert position in hidden cards to position on board. Note that 1 is added to 
                        // make it easier on the user. They may not know that arrays start at index=0 so
                        // if the card they should flip is array position 0,2 they should click on the card
                        // in the 1st row and the 3rd column.
                        //
                        string row2 = ((arrayidx / 5) + 1).ToString();
                        string col2 = ((arrayidx % 5) + 1).ToString();
                        matchfound = true;
                        MessageBox.Show("Try Row " + row1 + " Column " + col1 + " Row " + row2 + " Column " + col2);
                    }
                    ++arrayidx;
                    if (arrayidx > 29)
                    {
                        //
                        // this should never happen
                        //
                        MessageBox.Show("Couldn't Find a match");
                        matchfound = true;
                    }
                } while (!matchfound);
            }
        }
    }
}
