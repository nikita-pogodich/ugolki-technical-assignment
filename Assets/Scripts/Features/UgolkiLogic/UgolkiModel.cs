using System;
using System.Collections.Generic;
using Core.ModelProvider;
using Core.SettingsHelper;
using Cysharp.Threading.Tasks;
using Features.UgolkiLogic.UgolkiRules;
using R3;
using Settings;
using Tools;
using Tools.GraphSearch;
using VContainer;

namespace Features.UgolkiLogic
{
    public class UgolkiModel : BaseModel, IUgolkiModel, IDisposable
    {
        private readonly ILocalSettings _localSettings;
        private readonly ISettingsHelper _settingsHelper;

        private readonly Dictionary<string, BaseUgolkiRule> _availableMovesByRule = new();
        private readonly Dictionary<int, Node<Coord>> _availableMovesGraph = new();
        private readonly IGraphSearch _graphSearch = new BellmanFord();
        private readonly ReactiveProperty<int> _whiteMovesAmount = new();
        private readonly ReactiveProperty<int> _blackMovesAmount = new();
        private readonly ReactiveProperty<Player> _currentPlayer = new();
        private readonly List<string> _rules = new();

        private BoardCellType[,] _board;
        private int _boardSize;
        private BoardCellType[,] _defaultPositions;
        private List<Coord> _whiteWinConditions;
        private List<Coord> _blackWinConditions;
        private bool _isGameStarted;
        private string _currentRule;
        private Coord _selectedPiecePosition;
        private bool _hasSelectedPiece;
        private List<Coord> _currentAvailableMoves = new();

        private readonly ReactiveCommand<List<Coord>> _pieceMoved = new();
        private readonly ReactiveCommand<Coord> _pieceSelected = new();
        private readonly ReactiveCommand<Coord> _pieceDeselected = new();
        private readonly ReactiveCommand _gameStarted = new();
        private readonly ReactiveCommand _gameEnded = new();
        private readonly ReactiveCommand<Player> _gameWon = new();
        private readonly ReactiveCommand<string> _wrongMoveSelected = new();

        public BoardCellType[,] Board => _board;
        public Observable<List<Coord>> PieceMoved => _pieceMoved;
        public Observable<Coord> PieceSelected => _pieceSelected;
        public Observable<Coord> PieceDeselected => _pieceDeselected;
        public Observable<Unit> GameStarted => _gameStarted;
        public Observable<Unit> GameEnded => _gameEnded;
        public Observable<Player> GameWon => _gameWon;
        public Observable<string> WrongMoveSelected => _wrongMoveSelected;
        public ReadOnlyReactiveProperty<int> WhiteMovesAmount => _whiteMovesAmount;
        public ReadOnlyReactiveProperty<int> BlackMovesAmount => _blackMovesAmount;
        public ReadOnlyReactiveProperty<Player> CurrentPlayer => _currentPlayer;

        [Inject]
        public UgolkiModel(
            IModelProvider modelProvider,
            ILocalSettings localSettings,
            ISettingsHelper settingsHelper) : base(modelProvider)
        {
            _localSettings = localSettings;
            _settingsHelper = settingsHelper;
        }

        protected override async UniTask OnInit()
        {
            IGameSettings gameSettings = _localSettings.GameSettings;
            _boardSize = gameSettings.BoardSize;
            _board = new BoardCellType[_boardSize, _boardSize];

            _defaultPositions = await _settingsHelper.GetDefaultBoardPositionsAsync();
            _whiteWinConditions = await _settingsHelper.GetWhiteWinConditionsAsync();
            _blackWinConditions = await _settingsHelper.GetBlackWinConditionsAsync();

            var cannotJumpRule = new CannotJumpRule(_boardSize);
            var canJumpOrthogonallyRule = new CanJumpOrthogonallyRule(_boardSize);
            var canJumpDiagonallyRule = new CanJumpDiagonallyRule(_boardSize);

            IUgolkiRulesSettings ugolkiRulesSettings = _localSettings.UgolkiRulesSettings;
            _rules.Add(ugolkiRulesSettings.Rule1);
            _rules.Add(ugolkiRulesSettings.Rule2);
            _rules.Add(ugolkiRulesSettings.Rule3);

            _availableMovesByRule.Add(ugolkiRulesSettings.Rule1, canJumpDiagonallyRule);
            _availableMovesByRule.Add(ugolkiRulesSettings.Rule2, canJumpOrthogonallyRule);
            _availableMovesByRule.Add(ugolkiRulesSettings.Rule3, cannotJumpRule);
        }

        public void Dispose()
        {
            _availableMovesGraph.Clear();
            _currentAvailableMoves.Clear();
            _rules.Clear();
            _availableMovesByRule.Clear();
        }

        public List<string> GetRules()
        {
            return _rules;
        }

        public void SetRule(string rule)
        {
            _currentRule = rule;
        }

        public void StartGame()
        {
            _isGameStarted = true;
            ResetBoard();
            ResetMovesInfo();
            ResetPlayer();

            OnGameStarted();
        }

        public void EndGame()
        {
            _isGameStarted = false;
            ResetBoard();
            OnGameEnded();
        }

        private void ResetPlayer()
        {
            _currentPlayer.Value = Player.White;
        }

        private void ResetMovesInfo()
        {
            _whiteMovesAmount.Value = 0;
            _blackMovesAmount.Value = 0;
        }

        public void RestartGame()
        {
            EndGame();
            StartGame();
        }

        public void TrySelectCell(Coord cell)
        {
            if (_isGameStarted == false)
            {
                return;
            }

            if (_hasSelectedPiece == false && _board[cell.Row, cell.Column] == BoardCellType.Empty)
            {
                OnNeedToSelectPiece();
                return;
            }

            if (_hasSelectedPiece == true && _selectedPiecePosition == cell)
            {
                _hasSelectedPiece = false;
                OnPieceDeselected(cell);
                return;
            }

            if (_hasSelectedPiece == true && _board[cell.Row, cell.Column] == BoardCellType.Empty)
            {
                MovePiece(cell);
                return;
            }

            if (_board[cell.Row, cell.Column] == BoardCellType.White && _currentPlayer.Value == Player.White ||
                _board[cell.Row, cell.Column] == BoardCellType.Black && _currentPlayer.Value == Player.Black)
            {
                _hasSelectedPiece = true;
                _selectedPiecePosition = cell;

                _currentAvailableMoves = GetAvailableMoves(cell);
                OnPieceSelected(cell);
            }
            else
            {
                OnWrongPieceTypeSelected();
            }
        }

        private Player? CheckWinner()
        {
            bool isWhiteWins = CheckWinConditions(_whiteWinConditions, BoardCellType.White);
            bool isBlackWins = CheckWinConditions(_blackWinConditions, BoardCellType.Black);

            if (isWhiteWins)
            {
                return Player.White;
            }

            if (isBlackWins)
            {
                return Player.Black;
            }

            return null;
        }

        private bool CheckWinConditions(List<Coord> winConditions, BoardCellType boardCellType)
        {
            bool isWin = true;

            foreach (Coord winCondition in winConditions)
            {
                if (winCondition.Row > _boardSize - 1 ||
                    winCondition.Row < 0 ||
                    winCondition.Column > _boardSize - 1 ||
                    winCondition.Column < 0)
                {
                    continue;
                }

                if (_board[winCondition.Row, winCondition.Column] != boardCellType)
                {
                    isWin = false;
                    break;
                }
            }

            return isWin;
        }

        private void ResetBoard()
        {
            if (_defaultPositions.GetLength(0) > _boardSize ||
                _defaultPositions.GetLength(1) > _boardSize)
            {
                return;
            }

            for (int i = 0; i < _boardSize; i++)
            {
                for (int j = 0; j < _boardSize; j++)
                {
                    _board[i, j] = _defaultPositions[i, j];
                }
            }
        }

        private List<Coord> GetAvailableMoves(Coord fromCell)
        {
            var toCheck = new Queue<Coord>();
            var canJump = new List<Coord> {fromCell};
            var fromNode = new Node<Coord>(0, fromCell);

            _availableMovesGraph.Clear();
            _availableMovesGraph.Add(fromNode.Id, fromNode);
            toCheck.Enqueue(fromCell);

            _availableMovesByRule[_currentRule].TryAddAvailableMoves(
                _board,
                fromCell,
                _availableMovesGraph,
                toCheck,
                canJump);

            canJump.Remove(fromCell);
            return canJump;
        }

        private void MovePiece(Coord cell)
        {
            if (_currentAvailableMoves.Contains(cell) == false)
            {
                OnUnreachableMoveSelected();
                return;
            }

            _board[_selectedPiecePosition.Row, _selectedPiecePosition.Column] = BoardCellType.Empty;

            BoardCellType resultCellType =
                _currentPlayer.Value == Player.White ? BoardCellType.White : BoardCellType.Black;

            _hasSelectedPiece = false;

            var moves = new List<Coord>();

            int sourceIndex = 0;
            int destinationIndex = 0;

            foreach (Node<Coord> value in _availableMovesGraph.Values)
            {
                if (value.Value == _selectedPiecePosition)
                {
                    sourceIndex = value.Id;
                }

                if (value.Value == cell)
                {
                    destinationIndex = value.Id;
                }
            }

            List<int> shortestPath = _graphSearch.GetPath(_availableMovesGraph, sourceIndex, destinationIndex);

            for (int i = shortestPath.Count - 1; i >= 0; i--)
            {
                moves.Add(_availableMovesGraph[shortestPath[i]].Value);
            }

            _board[cell.Row, cell.Column] = resultCellType;

            OnPieceMoved(moves);

            switch (_currentPlayer.Value)
            {
                case Player.White:
                    _whiteMovesAmount.Value++;
                    break;
                case Player.Black:
                    _blackMovesAmount.Value++;
                    break;
            }

            _currentPlayer.Value = _currentPlayer.Value == Player.White ? Player.Black : Player.White;

            Player? winner = CheckWinner();
            if (winner != null)
            {
                OnGameWon(winner.Value);
            }
        }

        private void OnPieceSelected(Coord cell)
        {
            _pieceSelected.Execute(cell);
        }

        private void OnPieceDeselected(Coord cell)
        {
            _pieceDeselected.Execute(cell);
        }

        private void OnPieceMoved(List<Coord> moves)
        {
            _pieceMoved.Execute(moves);
        }

        private void OnUnreachableMoveSelected()
        {
            _wrongMoveSelected.Execute(
                _localSettings.LocalizationKeys.MessagePopupLocalizationKeys.MoveUnreachableMessage);
        }

        private void OnWrongPieceTypeSelected()
        {
            _wrongMoveSelected.Execute(_localSettings.LocalizationKeys.MessagePopupLocalizationKeys.NotYourMoveMessage);
        }

        private void OnNeedToSelectPiece()
        {
            _wrongMoveSelected.Execute(_localSettings.LocalizationKeys.MessagePopupLocalizationKeys.SelectPieceMessage);
        }

        private void OnGameWon(Player player)
        {
            _gameWon.Execute(player);
        }

        private void OnGameStarted()
        {
            _gameStarted.Execute(Unit.Default);
        }

        private void OnGameEnded()
        {
            _gameEnded.Execute(Unit.Default);
        }
    }
}