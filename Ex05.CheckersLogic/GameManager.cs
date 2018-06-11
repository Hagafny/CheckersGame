using System;
using System.Collections.Generic;
using System.Linq;

namespace Ex05.CheckersLogic
{
    /// <summary>
    /// The heart of the game. Everything checkers related goes through the Game Manager. 
    /// We set up a game manager instance by providing it with a GameSettings object that is filled by the user from the console
    /// </summary>
    public class GameManager
    {
        private Board m_Board;
        private HumanPlayer m_BlackPlayer;
        private Player m_RedPlayer;
        private int m_TurnNumber;
        private Move m_LastExecutedMove;

        public Board Board
        {
            get
            {
                return m_Board;
            }

            set
            {
                m_Board = value;
            }
        }

        public HumanPlayer BlackPlayer
        {
            get
            {
                return m_BlackPlayer;
            }

            set
            {
                m_BlackPlayer = value;
            }
        }

        public Player RedPlayer
        {
            get
            {
                return m_RedPlayer;
            }

            set
            {
                m_RedPlayer = value;
            }
        }

        public int TurnNumber
        {
            get
            {
                return m_TurnNumber;
            }

            set
            {
                m_TurnNumber = value;
                OnTurnChange();
            }
        }

        public Move LastExecutedMove
        {
            get
            {
                return m_LastExecutedMove;
            }

            set
            {
                m_LastExecutedMove = value;
            }
        }

        public Player ActivePlayer
        {
            get
            {
                return TurnNumber % 2 == 0 ? RedPlayer : BlackPlayer;
            }
        }

        public GameState State
        {
            get
            {
                GameState state = GameState.Active;
                eCheckerColor squareCheckerColor;

                byte blackPlayerMovesAmount = (byte)getPlayerAvailableMoves(BlackPlayer.Color).Count,
                     whitePlayerMovesAmount = (byte)getPlayerAvailableMoves(RedPlayer.Color).Count,
                     blackPlayerCheckersAmount = 0,
                     whitePlayerCheckersAmount = 0;

                // Calculate checkers amount for each player
                for (int i = 0; i < Board.BoardSize; i++)
                {
                    for (int j = 0; j < Board.BoardSize; j++)
                    {
                        squareCheckerColor = Board.Matrix[j, i].Color;
                        if (squareCheckerColor == eCheckerColor.Red)
                        {
                            whitePlayerCheckersAmount++;
                        }
                        else if (squareCheckerColor == eCheckerColor.Black)
                        {
                            blackPlayerCheckersAmount++;
                        }
                    }
                }

                if (blackPlayerMovesAmount == 0 && whitePlayerMovesAmount == 0)
                {
                    state = GameState.Tie;
                }
                else if (blackPlayerCheckersAmount == 0 || blackPlayerMovesAmount == 0)
                {
                    state = GameState.WhiteWon;
                }
                else if (whitePlayerCheckersAmount == 0 || whitePlayerMovesAmount == 0)
                {
                    state = GameState.BlackWon;
                }

                return state;
            }
        }

        // Actions 
        public event Action BoardChanged;

        public event Action TurnChanged;

        public event Action<Player> GameEnded;
    
        public GameManager(GameSettings i_Settings)
        {
            setupGame(i_Settings);
        }

        public Move[] GetPossibleMovesForChecker(Point i_LocationOnMatrix)
        {
            Move[] possibleMoves = GetAllAvailableMovesOfChecker(i_LocationOnMatrix);
            Square chosenSquare = Board.Matrix[i_LocationOnMatrix.Y, i_LocationOnMatrix.X];

            // LastExecutedMove will only be null on the first play of the game. 
            if (LastExecutedMove != null) 
            {
                // If the lastExecutedMove was done by the same player, we are now playing an "extra turn"
                // This means that have to play the same checker that has made an eating move on the last turn.
                bool thisTurnIsAnExtraTurnAfterEating = Board.Matrix[LastExecutedMove.Destination.Y, LastExecutedMove.Destination.X].Color == chosenSquare.Color,
                    theEatingWasNotDoneByTheChosenChecker;

                if (thisTurnIsAnExtraTurnAfterEating)
                {
                    theEatingWasNotDoneByTheChosenChecker = i_LocationOnMatrix != LastExecutedMove.Destination;
                    if (theEatingWasNotDoneByTheChosenChecker)
                    {
                        return null;
                    }
                }
            }

            // Iterate over all of the player's checkers and map all the sources ot these moves.
            Point[] availableMSourcesovesThatEat1Checker = getAllAvailableSourcesOnBoardThatEat1Checker(chosenSquare.Color);
            int availableEatingMovesCount = availableMSourcesovesThatEat1Checker.Count();

            // Stop this turn if are about to perform a move that is not an "eating" move while there are eating moves available.
            if (!(availableEatingMovesCount == 0 || availableMSourcesovesThatEat1Checker.Contains(i_LocationOnMatrix)))
            {
                return null;
            }

            // If there are eating moves available, filter all the non eating moves
            possibleMoves = filterMovesToOnlyMovesThatEatOneChecker(possibleMoves);

            return possibleMoves.ToArray();
        }

        /// <summary>
        /// This method is used on a new game.
        /// </summary>
        public void Reset()
        {
            TurnNumber = 1;
            LastExecutedMove = null;
            Board.InitializeBoard();

            OnBoardChange();
        }

        public void HandleMove(Move i_Move)
        {
            playMove(i_Move);
     
            if (State == GameState.Active)
            {
                Move lastExecutedMove = LastExecutedMove ?? new Move(-1, -1, -1, -1);

                // If the turn that was played was played by a human, and in that turn a checker was eaten and from our new location we have an option to eat another checker, get another turn.
                bool playerHasAnotherTurn = ActivePlayer is HumanPlayer && lastExecutedMove.CheckersEaten.Count == 1 && canACheckerEatFromPoint(lastExecutedMove.Destination);

                if (playerHasAnotherTurn == false)
                {
                    TurnNumber++;
                }
            }
            else
            {
                // The game has ended
                endGame();
            }
        }
        public int CalculatePlayerWinningPoints(Player i_Player)
        {
            Player opponent = i_Player.Color == BlackPlayer.Color ? RedPlayer : BlackPlayer;

            byte playerPoints = getPlayerBoardPoints(i_Player),
                 opponentPoints = getPlayerBoardPoints(opponent),
                 winningPoints = (byte)(playerPoints - opponentPoints);

            return winningPoints;
        }

        #region Event Listeners
        protected virtual void OnBoardChange()
        {
            if (BoardChanged != null)
            {
                BoardChanged();
            }
        }

        protected virtual void OnTurnChange()
        {
            if (TurnChanged != null)
            {
                TurnChanged();
            }
        }

        protected virtual void OnGameEnded(Player i_WinningPlayer)
        {
            if (GameEnded != null)
            {
                GameEnded(i_WinningPlayer);
            }
        }

        #endregion

        /// <summary>
        /// This is the method that actually makes moves on the board. It is called by both Computer and AI.
        /// </summary>
        /// <param name="i_Player">The player currently performing the move</param>
        /// <param name="i_Move">The move itself</param>
        private void playMove(Move i_Move)
        {
            LastExecutedMove = i_Move; // Keep track of our LastExecutedMove
            Board.Matrix[i_Move.Destination.Y, i_Move.Destination.X].Color = Board.Matrix[i_Move.Source.Y, i_Move.Source.X].Color;
            Board.Matrix[i_Move.Destination.Y, i_Move.Destination.X].King = Board.Matrix[i_Move.Source.Y, i_Move.Source.X].King;
            resetSquare(i_Move.Source);

            foreach (Point point in i_Move.CheckersEaten)
            {
                resetSquare(point);
            }

            // Kinging
            if ((i_Move.Destination.Y == (Board.BoardSize - 1) && Board.Matrix[i_Move.Destination.Y, i_Move.Destination.X].Color == eCheckerColor.Red)
                || (i_Move.Destination.Y == 0 && Board.Matrix[i_Move.Destination.Y, i_Move.Destination.X].Color == eCheckerColor.Black))
            {
                Board.Matrix[i_Move.Destination.Y, i_Move.Destination.X].King = true;
            }

            OnBoardChange();
        }

        /// <summary>
        /// This method is in charge of setting up all the fields
        /// </summary>
        /// <param name="i_Settings">The settings that the user has passed from the console</param>
        /// <param name="i_OnPlayerTurn">An action that is passed from the console. It is used by Human Player's PlayTurn</param>
        /// <param name="i_OnGameEnd">An action that is passed from the console. Will be fired during the game manager's own EndGame</param>
        private void setupGame(GameSettings i_Settings)
        {
            TurnNumber = 1;
            Board = new Board(i_Settings.BoardSize);
            LastExecutedMove = null;

            // Player1 will always be a human player, player 2 is dependent on the secenrio the player has chosen
            BlackPlayer = new HumanPlayer(i_Settings.Player1Name, eCheckerColor.Black);

            switch (i_Settings.Mode)
            {
                case GameMode.PVP:
                    this.RedPlayer = new HumanPlayer(i_Settings.Player2Name, eCheckerColor.Red);
                    break;
                case GameMode.PVC:
                    this.RedPlayer = new ComputerPlayer(OnComputerTurn);
                    break;
                default:
                    throw new Exception("Invalid Game Mode");
            }
        }

        /// <summary>
        /// This is the AI behvior. We explicitly implement our algorithem to return a List<Point> of checkers eaten.
        /// We get all the moves that is available to the AI but we order them based on the amount of eaten checkers available.
        /// </summary>
        /// <param name="i_Player">The computer player</param>
        private void OnComputerTurn()
        {
            List<Move> computerMoves = getPlayerAvailableMoves(eCheckerColor.Red);
            Move greedyMove = computerMoves.OrderByDescending(move => move.CheckersEaten.Count).First();

            HandleMove(greedyMove);
        }

        private Player getWinningPlayer()
        {
            Player winningPlayer = null;

            switch (State)
            {
                case GameState.Active:
                    throw new Exception("Impossible Scenerio");
                case GameState.WhiteWon:
                    winningPlayer = RedPlayer;
                    break;
                case GameState.BlackWon:
                    winningPlayer = BlackPlayer;
                    break;
                case GameState.Tie:
                default:
                    break;
            }

            return winningPlayer;
        }

        /// <summary>
        /// Ends the game
        /// </summary>
        /// <param name="i_WiningPlayer"></param>
        private void endGame()
        {
            Player winningPlayer = getWinningPlayer();         
            if (State != GameState.Tie)
            {
                int winningPlayerPoints = CalculatePlayerWinningPoints(winningPlayer);
                winningPlayer.Points += winningPlayerPoints;
            }

            OnGameEnded(winningPlayer);
        }

        private Point[] getAllAvailableSourcesOnBoardThatEat1Checker(eCheckerColor i_PlayerColor)
        {
            // We get all the available moves of the player, we filter them to those with only 1 eaten checker and we fetch the source
            Point[] sourcesOnBoardThatEat1Checker = getPlayerAvailableMoves(i_PlayerColor).Where(move => move.CheckersEaten.Count == 1).Select(move => move.Source).ToArray();
            return sourcesOnBoardThatEat1Checker;
        }

        private void resetSquare(Point i_Square)
        {
            // Resets the square and the selected checker
            Board.Matrix[i_Square.Y, i_Square.X].Color = eCheckerColor.Empty;
            Board.Matrix[i_Square.Y, i_Square.X].King = false;
        }

        /// <summary>
        /// We check if we have more to eat after we moved the checker to a new location following an "eating" move
        /// </summary>
        /// <param name="i_Source">The new source</param>
        /// <returns></returns>
        private bool canACheckerEatFromPoint(Point i_Source)
        {
            Move[] availableMovesAfterEating = GetAllAvailableMovesOfChecker(i_Source);
            bool playerHasAnotherTurn = availableMovesAfterEating.Any(moveAfterEating => moveAfterEating.CheckersEaten.Count > 0);

            return playerHasAnotherTurn;
        }

        /// <summary>
        /// After we got the checker's moves, we want to filter to only those moves that eat a checker.
        /// </summary>
        /// <param name="i_Moves">The moveset by the selected checker</param>
        /// <returns>A moveset that is filtered to only those moves that will cause an "eating move"</returns>
        private Move[] filterMovesToOnlyMovesThatEatOneChecker(Move[] i_Moves)
        {
            List<Move> filteredMoves = i_Moves.ToList();
            Move[] newMoves = i_Moves;
            bool filteredMovesToOnlyMovesThatEatOneChecker = i_Moves.Any(x => x.CheckersEaten.Count == 1);

            if (filteredMovesToOnlyMovesThatEatOneChecker)
            {
                filteredMoves.RemoveAll(move => move.CheckersEaten.Count != 1);
                newMoves = filteredMoves.ToArray();
            }

            return newMoves;
        }

        /// <summary>
        /// Iterate over all the checkers that the users has and accumulate all the moves available.
        /// eventually, returned the distinct moves.
        /// </summary>
        /// <param name="i_Player">The player in question</param>
        /// <returns>All available moves of said player</returns>
        private List<Move> getPlayerAvailableMoves(eCheckerColor i_PlayerColor)
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < Board.BoardSize; i++)
            {
                for (int j = 0; j < Board.BoardSize; j++)
                {
                    if (Board.Matrix[j, i].Color == i_PlayerColor)
                    {
                        moves.AddRange(GetAllAvailableMovesOfChecker(new Point(i, j)));
                    }
                }
            }

            return moves.Distinct().ToList();
        }

        /// <summary>
        /// Calculated the board points. Checker = 1 point, King = 4 points
        /// </summary>
        /// <param name="i_Player"></param>
        /// <returns></returns>
        private byte getPlayerBoardPoints(Player i_Player)
        {
            byte points = 0;
            for (int i = 0; i < Board.BoardSize; i++)
            {
                for (int j = 0; j < Board.BoardSize; j++)
                {
                    if (Board.Matrix[j, i].Color == i_Player.Color)
                    {
                        points++;
                        if (Board.Matrix[j, i].King)
                        {
                            points += 3;
                        }
                    }
                }
            }

            return points;
        }

        /// <summary>
        /// This big recursive method is in charge of giving us a list of moves available to a checker at a specific point
        /// We hold a List<Point> of checkers eaten for each move so that we will know how many eaten checkers are going to happen if we
        /// were to choose that move.
        /// 
        /// We decided to implement it this way that allows more than 1 checker eaten because of our computer AI implementation
        /// </summary>
        /// <param name="i_Checker">The checker we wish to check his amount of moves.</param>
        /// <returns>The list of moves the checker in question can apply</returns>
        #region GetAllAvailableMoves
        public Move[] GetAllAvailableMovesOfChecker(Point i_Checker)
        {
            return getAllAvailableMovesOfChecker(i_Checker, new Move(-1, -1, -1, -1), null);
        }

        /// <summary>
        /// Auxilery function for the GetAllAvailableMovesOfChecker
        /// </summary>
        /// <param name="i_Checker">The checker we wish to check his amount of moves.</param>
        /// <param name="i_LastMove">Reference to the lastMove</param>
        /// <param name="i_PriorPositions">reference to prior positions</param>
        /// <returns>The list of moves the checker in question can apply</returns>
        private Move[] getAllAvailableMovesOfChecker(Point i_Checker, Move i_LastMove, List<Point> i_PriorPositions)
        {
            if (i_PriorPositions == null)
            {
                i_PriorPositions = new List<Point>();
                i_PriorPositions.Add(i_Checker);
            }

            List<Move> availableMoves = new List<Move>();

            // Top Board
            // Stop regular red pieces from moving backwards
            if (Board.Matrix[i_PriorPositions[0].Y, i_PriorPositions[0].X].Color != eCheckerColor.Red || Board.Matrix[i_PriorPositions[0].Y, i_PriorPositions[0].X].King)
            {
                if (isValidPoint(i_Checker.X - 1, i_Checker.Y - 1))
                {
                    // Allow immediate empty spaces if it's the first jump
                    if (Board.Matrix[i_Checker.Y - 1, i_Checker.X - 1].Color == eCheckerColor.Empty && i_LastMove.Destination.X == -1)
                    {
                        availableMoves.Add(new Move(i_PriorPositions[0], i_Checker.X - 1, i_Checker.Y - 1));
                    }
                    else if (isValidPoint(i_Checker.X - 2, i_Checker.Y - 2)
                        && ((i_Checker.X - 2) != i_LastMove.Destination.X || (i_Checker.Y - 2) != i_LastMove.Destination.Y)
                        && ((i_Checker.X - 2) != i_PriorPositions[0].X || (i_Checker.Y - 2) != i_PriorPositions[0].Y)
                        && Board.Matrix[i_Checker.Y - 1, i_Checker.X - 1].Color != Board.Matrix[i_Checker.Y, i_Checker.X].Color
                        && Board.Matrix[i_Checker.Y - 2, i_Checker.X - 2].Color == eCheckerColor.Empty)
                    {
                        Point newDest = new Point(i_Checker.X - 2, i_Checker.Y - 2);
                        if (!i_PriorPositions.Contains(newDest))
                        {
                            Move move = new Move(i_PriorPositions[0], newDest);
                            move.CheckersEaten.Add(new Point(i_Checker.X - 1, i_Checker.Y - 1));
                            move.CheckersEaten.AddRange(i_LastMove.CheckersEaten);
                            availableMoves.Add(move);

                            i_PriorPositions.Add(newDest);

                            // Use recursion to find multiple checkers eatern                        
                            availableMoves.AddRange(getAllAvailableMovesOfChecker(new Point(i_Checker.X - 2, i_Checker.Y - 2), move, i_PriorPositions));
                        }
                    }
                }
            }

            // Top Right
            if (Board.Matrix[i_PriorPositions[0].Y, i_PriorPositions[0].X].Color != eCheckerColor.Red || Board.Matrix[i_PriorPositions[0].Y, i_PriorPositions[0].X].King)
            {
                if (isValidPoint(i_Checker.X + 1, i_Checker.Y - 1))
                {
                    // Check for a checker we can eat
                    if (Board.Matrix[i_Checker.Y - 1, i_Checker.X + 1].Color == eCheckerColor.Empty && i_LastMove.Destination.X == -1)
                    {
                        availableMoves.Add(new Move(i_PriorPositions[0], i_Checker.X + 1, i_Checker.Y - 1));
                    }
                    else if (isValidPoint(i_Checker.X + 2, i_Checker.Y - 2)
                        && ((i_Checker.X + 2) != i_LastMove.Destination.X || (i_Checker.Y - 2) != i_LastMove.Destination.Y)
                        && ((i_Checker.X + 2) != i_PriorPositions[0].X || (i_Checker.Y - 2) != i_PriorPositions[0].Y)
                        && Board.Matrix[i_Checker.Y - 1, i_Checker.X + 1].Color != Board.Matrix[i_Checker.Y, i_Checker.X].Color
                        && Board.Matrix[i_Checker.Y - 2, i_Checker.X + 2].Color == eCheckerColor.Empty)
                    {
                        Point newDest = new Point(i_Checker.X + 2, i_Checker.Y - 2);
                        if (!i_PriorPositions.Contains(new Point(i_Checker.X + 2, i_Checker.Y - 2)))
                        {
                            Move move = new Move(i_PriorPositions[0], newDest);
                            move.CheckersEaten.Add(new Point(i_Checker.X + 1, i_Checker.Y - 1));
                            move.CheckersEaten.AddRange(i_LastMove.CheckersEaten);
                            availableMoves.Add(move);

                            i_PriorPositions.Add(newDest);

                            // Use recursion to find multiple checkers eaten
                            availableMoves.AddRange(getAllAvailableMovesOfChecker(new Point(i_Checker.X + 2, i_Checker.Y - 2), move, i_PriorPositions));
                        }
                    }
                }
            }

            // Bottom Left
            if (Board.Matrix[i_PriorPositions[0].Y, i_PriorPositions[0].X].Color != eCheckerColor.Black || Board.Matrix[i_PriorPositions[0].Y, i_PriorPositions[0].X].King)
            {
                if (isValidPoint(i_Checker.X - 1, i_Checker.Y + 1))
                {
                    // Check for a checker we can eat
                    if (Board.Matrix[i_Checker.Y + 1, i_Checker.X - 1].Color == eCheckerColor.Empty && i_LastMove.Destination.X == -1)
                    {
                        availableMoves.Add(new Move(i_PriorPositions[0], i_Checker.X - 1, i_Checker.Y + 1));
                    }
                    else if (isValidPoint(i_Checker.X - 2, i_Checker.Y + 2)
                        && ((i_Checker.X - 2) != i_LastMove.Destination.X || (i_Checker.Y + 2) != i_LastMove.Destination.Y)
                        && ((i_Checker.X - 2) != i_PriorPositions[0].X || (i_Checker.Y + 2) != i_PriorPositions[0].Y)
                        && Board.Matrix[i_Checker.Y + 1, i_Checker.X - 1].Color != Board.Matrix[i_Checker.Y, i_Checker.X].Color
                        && Board.Matrix[i_Checker.Y + 2, i_Checker.X - 2].Color == eCheckerColor.Empty)
                    {
                        Point newDest = new Point(i_Checker.X - 2, i_Checker.Y + 2);
                        if (!i_PriorPositions.Contains(newDest))
                        {
                            Move move = new Move(i_PriorPositions[0], newDest);
                            move.CheckersEaten.Add(new Point(i_Checker.X - 1, i_Checker.Y + 1));
                            move.CheckersEaten.AddRange(i_LastMove.CheckersEaten);
                            availableMoves.Add(move);

                            i_PriorPositions.Add(newDest);

                            // Use recursion to find multiple checkers eaten
                            availableMoves.AddRange(getAllAvailableMovesOfChecker(new Point(i_Checker.X - 2, i_Checker.Y + 2), move, i_PriorPositions));
                        }
                    }
                }
            }

            // Bottom Right
            if (Board.Matrix[i_PriorPositions[0].Y, i_PriorPositions[0].X].Color != eCheckerColor.Black || Board.Matrix[i_PriorPositions[0].Y, i_PriorPositions[0].X].King)
            {
                if (isValidPoint(i_Checker.X + 1, i_Checker.Y + 1))
                {
                    // Check for a checker we can eat
                    if (Board.Matrix[i_Checker.Y + 1, i_Checker.X + 1].Color == eCheckerColor.Empty && i_LastMove.Destination.X == -1)
                    {
                        availableMoves.Add(new Move(i_PriorPositions[0], i_Checker.X + 1, i_Checker.Y + 1));
                    }
                    else if (isValidPoint(i_Checker.X + 2, i_Checker.Y + 2)
                        && ((i_Checker.X + 2) != i_LastMove.Destination.X || (i_Checker.Y + 2) != i_LastMove.Destination.Y)
                        && ((i_Checker.X + 2) != i_PriorPositions[0].X || (i_Checker.Y + 2) != i_PriorPositions[0].Y)
                        && Board.Matrix[i_Checker.Y + 1, i_Checker.X + 1].Color != Board.Matrix[i_Checker.Y, i_Checker.X].Color
                        && Board.Matrix[i_Checker.Y + 2, i_Checker.X + 2].Color == eCheckerColor.Empty)
                    {
                        Point newDest = new Point(i_Checker.X + 2, i_Checker.Y + 2);
                        if (!i_PriorPositions.Contains(newDest))
                        {
                            Move move = new Move(i_PriorPositions[0], newDest);
                            move.CheckersEaten.Add(new Point(i_Checker.X + 1, i_Checker.Y + 1));
                            move.CheckersEaten.AddRange(i_LastMove.CheckersEaten);
                            availableMoves.Add(move);

                            i_PriorPositions.Add(newDest);

                            // Use recursion to find multiple checkers eaten
                            availableMoves.AddRange(getAllAvailableMovesOfChecker(new Point(i_Checker.X + 2, i_Checker.Y + 2), move, i_PriorPositions));
                        }
                    }
                }
            }

            return availableMoves.ToArray();
        }

        /// <summary>
        /// Checks if we did not choose a points that is outside of the board scope
        /// </summary>
        /// <param name="x"> x coordinate</param>
        /// <param name="y"> y coordinate</param>
        /// <returns>Whether or not we are still playing on the board</returns>
        private bool isValidPoint(int x, int y)
        {
            bool isValidPoint = x >= 0 && x < Board.BoardSize && y >= 0 && y < Board.BoardSize;
            return isValidPoint;
        }

        #endregion
    }

    public enum GameState
    {
        Active,
        WhiteWon,
        BlackWon,
        Tie
    }
}
