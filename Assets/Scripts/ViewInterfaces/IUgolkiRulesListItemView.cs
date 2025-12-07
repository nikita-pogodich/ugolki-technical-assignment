using Core.MVP;
using R3;

namespace ViewInterfaces
{
    public interface IUgolkiRulesListItemView : IView
    {
        ReactiveProperty<string> TitleLocalizationKey { get; }
        Observable<Unit> Selected { get; }
        void SetSelected(bool isSelected);
    }
}