using Core.MVP;
using ViewInterfaces;

namespace Features.MainMenu
{
    public interface IMainMenuWindowPresenter : IWindowPresenter<IMainMenuWindowView, IMainMenuModel>
    {
    }
}