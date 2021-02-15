using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using SimpleConcentration.Models;

namespace SimpleConcentration.Presentation
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    //
    // This file has code to handle clicks on the GameBoard. 
    //
    public partial class GameView : Window
    {
        GameViewModel _gameViewModel;
        public GameView(GameViewModel gameViewModel)
        {
            _gameViewModel = gameViewModel;
            
            InitializeComponent();
        }

        //
        // A click on any card on the Game Board runs Button_Click. The clicked botton has a tag value (see the xaml code) which is used
        // to determine the exact row & column clicked and then execution passes to CardClicked.
        //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button boardcard = sender as Button;
            int row = int.Parse(boardcard.Tag.ToString().Substring(0, 1));
            int column = int.Parse(boardcard.Tag.ToString().Substring(1, 1));

            _gameViewModel.CardClicked(row, column);
        }
        //
        // The Game Board has a few buttons for handling game level functions that don't involve clicking a card
        //
        private void WindowButton_Click(object sender, RoutedEventArgs e)
        {
            Button windowButton = sender as Button;

            switch (windowButton.Name)
            {
                case "HowToPlay":
                    HowToPlay howtoplay = new HowToPlay();
                    howtoplay.ShowDialog();
                    break;
                case "GimmeaHint":
                    _gameViewModel.GameCommand(windowButton.Name);
                    break;
                case "NewGame":
                    _gameViewModel.GameCommand(windowButton.Name);
                    break;
                case "SaveGame":
                    //
                    // Sorry, this feature is not yet implemented
                    //
                    MessageBox.Show("Sorry, Feature is still in Development");
                    break;
                case "Quit":
                    Close();
                    _gameViewModel.GameCommand(windowButton.Name);
                    break;
            }
        }
    }
}
