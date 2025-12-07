using Core.MVP;
using Features.UgolkiLogic;
using ViewInterfaces;

namespace Features.GameHUDWindow
{
    public interface IGameHUDWindowPresenter : IWindowPresenter<IGameHUDWindowView, IUgolkiModel>
    {
    }
}