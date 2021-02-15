using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleConcentration.Presentation;

namespace SimpleConcentration.Models
{
    public class GameBoard : ObservableObject
    {
        //
        // The app needs to keep track of saved values because each player get's 2 turns so we need to know if their 1st card turned over
        // matches their second.
        //
        public const string Card = "18";
        public int SavedValue;
        public int SavedRow1;
        public int SavedColumn1;
        public int SavedRow2;
        public int SavedColumn2;
        public string SavedGameBoardState = "";
        public enum GameBoardState
        { 
            NewGame,
            Player1Turn1,
            Player1Turn2,
            Player2Turn1,
            Player2Turn2,
            NoMatch,
            Player1Win,
            Player2Win,
        }
        private string[][] _currentboard;

        public string[][] CurrentBoard
        { 
            get { return _currentboard; }
            set
            {
                _currentboard = value;
                OnPropertyChanged(nameof(CurrentBoard));
            }
        }
        public GameBoardState CurrentState { get; set; }
        public GameBoard()
        {
            CurrentBoard = new string[6][];
            CurrentBoard[0] = new string[5];
            CurrentBoard[1] = new string[5];
            CurrentBoard[2] = new string[5];
            CurrentBoard[3] = new string[5];
            CurrentBoard[4] = new string[5];
            CurrentBoard[5] = new string[5];
            InitializeGameboard();

        }
        public void InitializeGameboard()
        {
            CurrentState = GameBoardState.NewGame;

            //
            // Set all "Cards" on the game board to spaces to get ready for play 
            //
            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    CurrentBoard[row][column] = "";
                }
            }
        }
        public bool CardPositionAvailable(CardPosition cardPosition)
        {
            //
            // check to see if card has not been flipped over. This app will ignore a click on a card that is already turned over
            //
            if (CurrentBoard[cardPosition.Row][cardPosition.Column] == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void UpdateGameboardState()
        {
            
        }
    }
}
