using Core.LocalizationManager;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using ViewInterfaces;

namespace Views.MainMenu
{
    public class UgolkiRulesListItemView : BaseView, IUgolkiRulesListItemView
    {
        [SerializeField]
        private TextMeshProUGUI _title;

        [SerializeField]
        private Button _selectButton;

        [SerializeField]
        private CanvasGroup _selectedBackground;

        private ILocalizationManager _localizationManager;
        private readonly ReactiveProperty<string> _titleLocalizationKey = new();
        private readonly ReactiveCommand _selected = new();

        public ReactiveProperty<string> TitleLocalizationKey => _titleLocalizationKey;
        public Observable<Unit> Selected => _selected;

        [Inject]
        public void InjectDependencies(ILocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            _titleLocalizationKey.Subscribe(UpdateTitle).AddTo(ref disposableBuilder);
            _localizationManager.CurrentLanguage.Subscribe(OnLocalizationChanged).AddTo(ref disposableBuilder);
        }

        public void SetSelected(bool isSelected)
        {
            _selectedBackground.alpha = isSelected ? 1.0f : 0.0f;
        }

        private void Start()
        {
            _selectButton.onClick.AddListener(OnSelected);
        }

        private void OnDestroy()
        {
            _selectButton.onClick.RemoveListener(OnSelected);
        }

        private void OnLocalizationChanged(LanguageInfo _) => UpdateTitle(_titleLocalizationKey.Value);

        private void UpdateTitle(string _)
        {
            if (string.IsNullOrEmpty(_titleLocalizationKey.Value))
            {
                return;
            }

            string title = _localizationManager.GetText(_titleLocalizationKey.Value);
            _title.text = title;
        }

        private void OnSelected()
        {
            _selected.Execute(Unit.Default);
        }
    }
}