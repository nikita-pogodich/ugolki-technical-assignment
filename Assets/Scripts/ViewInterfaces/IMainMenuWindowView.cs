using Core.MVP;
using R3;

namespace ViewInterfaces
{
    public interface IMainMenuWindowView : IWindowView
    {
        Observable<Unit> StartGame { get; }
        Observable<Unit> ExitGame { get; }
        IUgolkiRulesListView UgolkiRulesListView { get; }
    }
}