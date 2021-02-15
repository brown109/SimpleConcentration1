The code has plenty of comments which should help explain some of the trickier aspects of this application. 

Initialization: To play concentration, you need to “hide” a number of pairs under “cards”. I decided to use an odd number 
of pairs so that there would always be a winner. The “cards” are laid out in 6 rows of 5 and the values for the cards are 
stored in an array (HiddenCards) that holds the card values from 1 to 15 with each value showing up twice (thus making pairs). 
The comments explain how to map one to the other. Remember that arrays start at row 0 and/or column 0.

Play: Each player gets 2 turns so you need to keep track of their 1st turn to see if their second turn produces a match. 
You’ll see a “NoMatch” state that allows both players to see both turned over cards until any card is clicked on. This covers 
both the 1st and 2nd pick cards back up and gets ready for the other player’s 1st turn.

I could have ended the game as soon as either player gets 8 pairs, but decided to have all pairs exposed before declaring a winner.

Any player can get help (Gimme a Hint) without being penalized. Again, the code explains this in the comments but when a hint is 
requested, a message box is displayed showing the row/column of 2 buttons that make a pair. By the way, the row/column gets 1 added 
to it to make it easier on the player (they may not know that the first row in the code has an index of 0).

If I had more time, I’d build in a Save Game feature and a button to Reset wins. Also, more could be done with graphics, 
but I’m happy enough with this for now.


Gary Brown 
