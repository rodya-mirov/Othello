using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Reversi.AIs;
using System.Threading;

namespace Reversi
{
    public class GameBoardManager : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont font;

        static Texture2D boardTexture;

        static Texture2D highlightedSquareTexture, darkSquareTexture;
        static Texture2D whitePieceTexture, blackPieceTexture;

        static Texture2D newGameButtonTexture;

        static Texture2D greedyAItexture, edgeHogTexture, cornerHogTexture;
        static Texture2D selectedAItexture;

        GameBoard board;

        Thread aiThread;

        int selectedAI;
        AI[] ais;
        Rectangle[] aiBoxes;
        Texture2D[] aiTextures;

        const int borderWidth = 3;
        const int squareWidth = 40;

        int mouseX, mouseY, highlightedX, highlightedY;

        bool mouseHeld = false;

        const double waitTimeSeconds = .5;
        double lastPlayTime;

        public GameBoardManager(Game game)
            : base(game)
        {
            board = new GameBoard();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            font = Game.Content.Load<SpriteFont>("Fonts/SpriteFont");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            loadTextures();

            setupAI();
        }

        private void setupAI()
        {
            int numberOfAIs = 3;
            selectedAI = 1;

            ais = new AI[numberOfAIs];
            aiBoxes = new Rectangle[numberOfAIs];
            aiTextures = new Texture2D[numberOfAIs];

            ais[0] = new NPlyGreedyAI(3);
            ais[1] = new EdgeHog(3);
            ais[2] = new CornerAI(3);

            aiBoxes[0] = new Rectangle(345, 43, 46, 46);
            aiBoxes[1] = new Rectangle(345, 86, 46, 46);
            aiBoxes[2] = new Rectangle(345, 129, 46, 46);

            aiTextures[0] = greedyAItexture;
            aiTextures[1] = edgeHogTexture;
            aiTextures[2] = cornerHogTexture;
        }

        private void loadTextures()
        {
            if (boardTexture == null)
                boardTexture = Game.Content.Load<Texture2D>("Images/BoardImage");

            if (highlightedSquareTexture == null)
                highlightedSquareTexture = Game.Content.Load<Texture2D>("Images/HighlightedSquareImage");

            if (darkSquareTexture == null)
                darkSquareTexture = Game.Content.Load<Texture2D>("Images/DarkSquareImage");

            if (whitePieceTexture == null)
                whitePieceTexture = Game.Content.Load<Texture2D>("Images/WhitePiece");

            if (blackPieceTexture == null)
                blackPieceTexture = Game.Content.Load<Texture2D>("Images/BlackPiece");

            if (newGameButtonTexture == null)
                newGameButtonTexture = Game.Content.Load<Texture2D>("Images/NewGameButton");

            if (greedyAItexture == null)
                greedyAItexture = Game.Content.Load<Texture2D>("Images/GreedyAI");

            if (edgeHogTexture == null)
                edgeHogTexture = Game.Content.Load<Texture2D>("Images/EdgeHog");

            if (cornerHogTexture == null)
                cornerHogTexture = Game.Content.Load<Texture2D>("Images/CornerHog");

            if (selectedAItexture == null)
                selectedAItexture = Game.Content.Load<Texture2D>("Images/HighlightedAI");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            updateMousePosition();
            dealWithUserClicks(gameTime);

            if (timeForAIMove(gameTime))
            {
                if (aiThread == null)
                {
                    ais[selectedAI].storedBoard = board;
                    aiThread = new Thread(ais[selectedAI].getDesiredMove);
                    aiThread.Start();

                    this.lastPlayTime = gameTime.TotalGameTime.TotalSeconds;
                }
                else if (aiThread.ThreadState == ThreadState.Stopped && gameTime.TotalGameTime.TotalSeconds - lastPlayTime > waitTimeSeconds)
                {
                    board = board.makeMove(ais[selectedAI].desiredMove.X, ais[selectedAI].desiredMove.Y);
                    aiThread = null;
                }
            }
        }

        private bool timeForAIMove(GameTime gameTime)
        {
            if (board.gameover)
                return false;

            if (board.currentTurn != GameBoard.BLACK)
                return false;

            return true;
        }

        private void dealWithUserClicks(GameTime gameTime)
        {
            if (!Game.IsActive)
                return;

            if (mouseHeld == true && Mouse.GetState().LeftButton == ButtonState.Released)
            {
                mouseHeld = false;
            }
            else if (mouseHeld == false && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                mouseHeld = true;

                bool madePlay = false;
                Point mousePoint = new Point(mouseX, mouseY);

                if (board.currentTurn == GameBoard.WHITE)
                {
                    madePlay = attemptToMakePlay();

                    if (madePlay)
                    {
                        lastPlayTime = gameTime.TotalGameTime.TotalSeconds;
                    }
                }

                for (int aiIndex = 0; aiIndex < ais.Length; ++aiIndex)
                {
                    if (selectedAI != aiIndex && aiBoxes[aiIndex].Contains(mousePoint))
                    {
                        selectedAI = aiIndex;
                        newGame();
                    }
                }

                if (0 <= mouseX && mouseX <= 346 && 434 <= mouseY && mouseY <= 500)
                {
                    newGame();
                }
            }
        }

        private void newGame()
        {
            board = new GameBoard();

            if (aiThread != null)
            {
                aiThread.Abort();
                aiThread = null;
            }
        }

        private bool attemptToMakePlay()
        {
            if (board.validPosition(highlightedX, highlightedY) &&
                board.canPlayAtPosition(highlightedX, highlightedY))
            {
                board = board.makeMove(highlightedX, highlightedY);
                return true;
            }
            else
                return false;
        }

        private void updateMousePosition()
        {
            if (!Game.IsActive)
                return;

            int mouseXMod, mouseYMod, mouseXDiv, mouseYDiv;

            mouseX = Mouse.GetState().X;
            mouseY = Mouse.GetState().Y;

            mouseXMod = mouseX % (borderWidth + squareWidth);
            mouseYMod = mouseY % (borderWidth + squareWidth);

            mouseXDiv = mouseX / (borderWidth + squareWidth);
            mouseYDiv = mouseY / (borderWidth + squareWidth);

            if (mouseXMod >= borderWidth && mouseYMod >= borderWidth)
            {
                highlightedX = mouseXDiv;
                highlightedY = mouseYDiv;
            }
            else
            {
                highlightedX = -1;
                highlightedY = -1;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();

            spriteBatch.Draw(boardTexture, Vector2.Zero, Color.White);

            drawHighlights();
            drawPieces();
            drawScoreText();
            drawNewGameButton();

            drawAIButtons();

            spriteBatch.End();
        }

        private void drawAIButtons()
        {
            for (int i = 0; i < ais.Length; ++i)
            {
                spriteBatch.Draw(aiTextures[i], new Vector2(aiBoxes[i].X, aiBoxes[i].Y), Color.White);
            }

            spriteBatch.Draw(selectedAItexture, new Vector2(aiBoxes[selectedAI].X, aiBoxes[selectedAI].Y), Color.White);
        }

        private void drawNewGameButton()
        {
            spriteBatch.Draw(newGameButtonTexture, new Vector2(0, 434), Color.White);
        }

        private void drawScoreText()
        {
            spriteBatch.DrawString(font, "White: " + board.whiteScore, new Vector2(20, 355), Color.White);
            spriteBatch.DrawString(font, "Black: " + board.blackScore, new Vector2(180, 355), Color.White);

            if (board.gameover)
            {
                if (board.whiteScore > board.blackScore)
                    spriteBatch.DrawString(font, "Game over!  White wins!", new Vector2(20, 385), Color.White);
                else if (board.blackScore > board.whiteScore)
                    spriteBatch.DrawString(font, "Game over!  Black wins!", new Vector2(20, 385), Color.White);
                else
                    spriteBatch.DrawString(font, "Game over!  Tie!", new Vector2(20, 385), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font,
                    ((board.currentTurn == GameBoard.WHITE) ? "White's Turn" : "Black is thinking!"),
                    new Vector2(20, 385),
                    Color.White);
            }
        }

        private void drawHighlights()
        {
            if (0 <= highlightedX && highlightedX < 8 && 0 <= highlightedY && highlightedY < 8)
            {
                spriteBatch.Draw(board.currentTurn == GameBoard.WHITE ? highlightedSquareTexture : darkSquareTexture,
                    new Vector2(highlightedX * (borderWidth + squareWidth), highlightedY * (borderWidth + squareWidth)),
                    Color.White);

                if (board.currentTurn == GameBoard.WHITE && board.canPlayAtPosition(highlightedX, highlightedY))
                {
                    spriteBatch.Draw(whitePieceTexture,
                        new Vector2(highlightedX * (borderWidth + squareWidth), highlightedY * (borderWidth + squareWidth)),
                        Color.White);
                }
            }
        }

        private void drawPieces()
        {
            for (int x = 0; x < 8; ++x)
            {
                for (int y = 0; y < 8; ++y)
                {
                    if (board.cells[x, y] == GameBoard.BLACK)
                        spriteBatch.Draw(blackPieceTexture,
                            new Vector2(x * (borderWidth + squareWidth), y * (borderWidth + squareWidth)),
                            Color.White);

                    if (board.cells[x, y] == GameBoard.WHITE)
                        spriteBatch.Draw(whitePieceTexture,
                            new Vector2(x * (borderWidth + squareWidth), y * (borderWidth + squareWidth)),
                            Color.White);
                }
            }
        }
    }
}
