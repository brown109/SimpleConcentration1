using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConcentration.Models
{
    //
    // each player has the same properties. You'll see them here. both player1 and player2 are inherited from this Gamer class
    // If I had more time, I could handle Player1 and Player2 without needing to define them separately within Models
    //
    
    public class Gamer : ObservableObject
    {
        private string _name;
        private int _pairs;
        private int _wins;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int Pairs
        {
            get { return _pairs; }
            set
            {
                _pairs = value;
                OnPropertyChanged(nameof(Pairs));
            }
        }
        public int Wins
        {
            get { return _wins; }
            set
            {
                _wins = value;
                OnPropertyChanged(nameof(Wins));
            }
        }
    }
}
