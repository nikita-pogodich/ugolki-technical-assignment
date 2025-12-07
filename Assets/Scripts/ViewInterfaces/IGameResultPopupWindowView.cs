using Core.MVP;
using R3;

namespace ViewInterfaces
{
    public interface IGameResultPopupWindowView : IWindowView
    {
        Observable<Unit> BackToMenu { get; }
        Observable<Unit> RestartGame { get; }
        ReactiveProperty<string> GameResultLocalizationKey { get; }
    }
}