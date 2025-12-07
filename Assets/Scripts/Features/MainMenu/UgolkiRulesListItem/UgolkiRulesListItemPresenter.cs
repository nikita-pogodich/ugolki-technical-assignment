using ViewInterfaces;
using R3;

namespace Features.MainMenu.UgolkiRulesListItem
{
    public class UgolkiRulesListItemPresenter :
        BasePresenter<IUgolkiRulesListItemView, IUgolkiRulesListItemModel>,
        IUgolkiRulesListItemPresenter
    {
        private bool _isSelected;

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            View.TitleLocalizationKey.Value = Model.TitleLocalizationKey;

            View.Selected.Subscribe(OnRuleSelected).AddTo(ref disposableBuilder);
            Model.IsSelected.Subscribe(View.SetSelected).AddTo(ref disposableBuilder);
        }

        private void OnRuleSelected(Unit _)
        {
            if (IsShown == false)
            {
                return;
            }

            Model.SelectRule();
        }
    }
}