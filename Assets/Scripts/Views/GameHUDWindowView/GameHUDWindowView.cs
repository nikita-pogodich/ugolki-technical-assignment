using Core.LocalizationManager;
using R3;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using ViewInterfaces;

namespace Views.GameHUDWindowView
{
    public class GameHUDWindowView : BaseWindowView, IGameHUDWindowView
    {
        private readonly ReactiveCommand _back = new();

        public Observable<Unit> Back => _back;

        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private TextMeshProUGUI _whiteMovesCount;

        [SerializeField]
        private TextMeshProUGUI _blackMovesCount;

        [SerializeField]
        private TextMeshProUGUI _currentPlayer;

        private ILocalizationManager _localizationManager;
        private ILocalSettings _localSettings;

        [Inject]
        public void InjectDependencies(ILocalizationManager localizationManager, ILocalSettings localSettings)
        {
            _localizationManager = localizationManager;
            _localSettings = localSettings;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            _backButton.OnClickAsObservable().Subscribe(OnBack).AddTo(ref disposableBuilder);
        }

        public void SetWhiteMovesAmount(int amount)
        {
            string amountText = GetMovesText(
                amount,
                _localSettings.LocalizationKeys.UgolkiGameLocalizationKeys.WhiteMovesCount);

            _whiteMovesCount.text = amountText;
        }

        public void SetBlackMovesAmount(int amount)
        {
            string amountText = GetMovesText(
                amount,
                _localSettings.LocalizationKeys.UgolkiGameLocalizationKeys.BlackMovesCount);

            _blackMovesCount.text = amountText;
        }

        public void ChangeCurrentPlayer(string currentPlayer)
        {
            _currentPlayer.text = currentPlayer;
        }

        public override void SetShown(bool isShown)
        {
            base.SetShown(isShown);
            SetCanvasEnabled(isShown);
        }

        private string GetMovesText(int count, string localizationKey)
        {
            string movesCountText = _localizationManager.GetText(
                key: localizationKey,
                keyToReplace: _localSettings.LocalizationKeys.UgolkiGameLocalizationKeys.MovesCountValue,
                valueToReplace: count.ToString());

            return movesCountText;
        }

        private void OnBack(Unit _)
        {
            _back.Execute(Unit.Default);
        }
    }
}