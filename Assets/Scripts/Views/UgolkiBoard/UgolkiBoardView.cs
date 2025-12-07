using System;
using System.Collections.Generic;
using System.Threading;
using Core.Logger;
using Core.ResourcesManager;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using Settings;
using Tools;
using UnityEngine;
using VContainer;
using ViewInterfaces;

namespace Views.UgolkiBoard
{
    public class UgolkiBoardView : BaseView, IUgolkiBoardView
    {
        private const double JumpMinDistance = 2.0;

        [SerializeField]
        private Transform _piecesRoot;

        [SerializeField]
        private Transform _cellHighlight;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private BoxCollider _boardCollider;

        [SerializeField]
        private float _cellSize;

        [SerializeField]
        private string _boardTag;

        [SerializeField]
        private Ease _pieceMoveEasing = Ease.OutExpo;

        [SerializeField]
        private float _pieceMoveDuration = 0.2f;

        [SerializeField]
        private float _jumpHeight = 1.0f;

        private IDualLogger _dualLogger;
        private IResourcesManager _resourcesManager;
        private ILocalSettings _localSettings;

        private PieceInfo[,] _board;
        private int _boardSize;
        private bool _isGameStarted;
        private CancellationTokenSource _cancellationTokenSource = default!;
        private readonly ReactiveCommand<Coord> _trySelectCell = new();

        private Sequence _animation;
        public Observable<Coord> TrySelectCell => _trySelectCell;

        [Inject]
        public void InjectDependencies(
            IDualLogger dualLogger,
            IResourcesManager resourcesManager,
            ILocalSettings localSettings)
        {
            _dualLogger = dualLogger;
            _localSettings = localSettings;
            _resourcesManager = resourcesManager;
        }

        public async UniTask StartGame(BoardCellType[,] board)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            ClearBoard();
            SetCellHighlightShown(false);

            try
            {
                await CreatePieces(board);
            }
            catch (OperationCanceledException)
            {
            }

            _isGameStarted = true;
        }

        public void EndGame()
        {
            _cancellationTokenSource.Cancel();

            ClearBoard();
            _isGameStarted = false;
        }

        public void PauseGame()
        {
            _isGameStarted = false;
        }

        public void SelectPiece(Coord coord)
        {
            SetCellHighlightShown(true);
            _cellHighlight.localPosition = new Vector3(coord.Row, 0.0f, coord.Column);
        }

        public void DeselectPiece(Coord coord)
        {
            SetCellHighlightShown(false);
        }

        public void MovePiece(List<Coord> moves)
        {
            SetCellHighlightShown(false);
            PieceMoveAnimation(moves);
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _boardSize = _localSettings.GameSettings.BoardSize;
            _board = new PieceInfo[_boardSize, _boardSize];
        }

        protected override void OnDeinit()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        private void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (_isGameStarted == false)
            {
                return;
            }

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0) == true)
            {
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Transform objectHit = hit.transform;
                    if (objectHit.CompareTag(_boardTag) == true)
                    {
                        Vector3 position = SnapToGrid(hit.point);
                        Vector3 localPoint = _piecesRoot.InverseTransformPoint(position);
                        _trySelectCell.Execute(new Coord((int) localPoint.x, (int) localPoint.z));
                    }
                }
            }
        }

        private Vector3 SnapToGrid(Vector3 pos)
        {
            if (_boardSize == 0)
            {
                return Vector3.zero;
            }

            float gridSnap = _boardCollider.size.x / _boardSize;
            float cellCenter = gridSnap / 2.0f;

            Vector3 snapHits = new Vector3(
                Mathf.Round((pos.x - cellCenter) / gridSnap) * gridSnap + cellCenter,
                pos.y,
                Mathf.Round((pos.z - cellCenter) / gridSnap) * gridSnap + cellCenter);

            return snapHits;
        }

        private void ClearBoard()
        {
            for (int i = 0; i < _boardSize; i++)
            {
                for (int j = 0; j < _boardSize; j++)
                {
                    if (_board[i, j] != null)
                    {
                        _resourcesManager.ReleaseGameObject(_board[i, j].PieceResourceKey, _board[i, j].Piece);
                        _board[i, j] = null;
                    }
                }
            }
        }

        private async UniTask CreatePieces(BoardCellType[,] board)
        {
            for (int i = 0; i < _boardSize; i++)
            {
                for (int j = 0; j < _boardSize; j++)
                {
                    BoardCellType boardCellType = board[i, j];
                    switch (boardCellType)
                    {
                        case BoardCellType.Empty:
                            _board[i, j] = null;
                            break;
                        case BoardCellType.White:
                        case BoardCellType.Black:
                        {
                            PieceInfo piece = await CreatePiece(i, j, boardCellType);
                            _board[i, j] = piece;
                            break;
                        }
                        default:
                            _dualLogger.Mandatory.LogError($"{nameof(BoardCellType)} {boardCellType} is not supported");
                            break;
                    }
                }
            }
        }

        private async UniTask<PieceInfo> CreatePiece(int row, int column, BoardCellType cellType)
        {
            string resultResourceKey;
            if (cellType == BoardCellType.White)
            {
                resultResourceKey = _localSettings.ResourceNames.PieceWhite;
            }
            else
            {
                resultResourceKey = _localSettings.ResourceNames.PieceBlack;
            }

            GameObject pieceGameObject = await _resourcesManager.InstantiateAsync(
                resultResourceKey,
                _piecesRoot,
                _cancellationTokenSource.Token);

            Transform piece = pieceGameObject.transform;
            piece.localPosition = new Vector3(_cellSize * row, 0.0f, _cellSize * column);
            piece.localScale = Vector3.one;
            piece.localEulerAngles = Vector3.zero;

            var pieceInfo = new PieceInfo(resultResourceKey, pieceGameObject);
            return pieceInfo;
        }

        private void SetCellHighlightShown(bool isShown)
        {
            _cellHighlight.gameObject.SetActive(isShown);
        }

        private void PieceMoveAnimation(List<Coord> moves)
        {
            _animation?.Kill();
            _animation = DOTween.Sequence();

            Coord sourceCell = moves[0];
            PieceInfo piece = _board[sourceCell.Row, sourceCell.Column];

            for (int i = 1; i < moves.Count; i++)
            {
                float jumpHeight = 0.0f;
                int jumps = 0;

                int moveDirectionRow = Math.Abs(sourceCell.Row - moves[i].Row);
                int moveDirectionColumn = Math.Abs(sourceCell.Column - moves[i].Column);
                Coord moveDirection = new Coord(moveDirectionRow, moveDirectionColumn);

                double moveMagnitude = moveDirection.Magnitude();
                if (moveMagnitude >= JumpMinDistance)
                {
                    jumpHeight = _jumpHeight;
                    jumps = 1;
                }

                Vector3 destinationPosition = new Vector3(moves[i].Row, 0.0f, moves[i].Column);
                Sequence pieceMove = piece.Piece.transform
                    .DOLocalJump(destinationPosition, jumpHeight, jumps, _pieceMoveDuration)
                    .SetEase(_pieceMoveEasing);

                _animation.Append(pieceMove);

                Coord destinationCell = moves[^1];
                _board[sourceCell.Row, sourceCell.Column] = null;
                _board[destinationCell.Row, destinationCell.Column] = piece;
            }
        }
    }
}