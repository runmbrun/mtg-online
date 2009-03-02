using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MTG;



namespace MTGClient
{
    class Game
    {
        MTGClientForm _form;

        public MTGDeck HomeDeck;
        public MTGDeck AwayDeck;

        public enum _gametype
        {
            Solitare,
            Multiplayer,
            None
        }

        public enum _gamestate
        {
            Started,
            Shuffle,
            ChooseSides,
            Playing,
            None
        };

        public enum _turnstate
        {
            Untap,
            Upkeep,
            Draw,
            MainFirst,
            BeginningCombat,
            DeclareAttackers,
            DeclareBlockers,
            CombatDamage,
            EndofCombat,
            MainSecond,
            EndofTurn,
            None
        }

        public _gamestate GameState;
        public _turnstate TurnState;
        public _gametype GameType;


        public Game()
        {
            GameState = _gamestate.None;
            TurnState = _turnstate.None;
            GameType = _gametype.None;
            HomeDeck = new MTGDeck();
            AwayDeck = new MTGDeck();
        }

        public Game(MTGClientForm form)
        {
            _form = form;
            GameState = _gamestate.None;
            TurnState = _turnstate.None;
            GameType = _gametype.None;
            HomeDeck = new MTGDeck();
            AwayDeck = new MTGDeck();
        }

        public void AddPlayerDeck(MTGDeck deck)
        {
            HomeDeck = deck;
        }

        public void StartSolitareGame()
        {
            GameState = _gamestate.Shuffle;
            TurnState = _turnstate.None;
            GameType = _gametype.Solitare;

            // shuffle deck
            HomeDeck.Shuffle();
            GameState = _gamestate.Playing;

            // mmb - start a new thread that will be the game's while loop
            

            // mmb - proof of concept... display a "random" draw of cards
            //for (Int32 i=0; i<7; i++)
            {
                // a normal card image is:  200 x 285
                Bitmap LargePic = new Bitmap(((MTGCard)HomeDeck.Cards[0]).Pic);
                Bitmap SmallPic = new Bitmap(((MTGCard)HomeDeck.Cards[0]).Pic, 100, 142);

                Graphics g = _form.pictureBoxGame.CreateGraphics();
                Rectangle r = _form.pictureBoxGame.ClientRectangle;
                g.DrawImage(LargePic, new Point(0, 0));
                g.DrawImage(SmallPic, new Point(100, 0));
                g.Save();
                g.Dispose();
                _form.Update();

                Graphics g2 = _form.panelTest.CreateGraphics();
                Rectangle r2 = _form.panelTest.ClientRectangle;
                g2.DrawImage(LargePic, new Point(0, 0));
                g2.DrawImage(SmallPic, new Point(100, 0));
                g2.Save();
                g2.Dispose();

                _form.Update();


                _form.pictureBoxGame.BackgroundImage = SmallPic;
                _form.panelTest.BackgroundImage = SmallPic;
            }
            
        }
    }
}
