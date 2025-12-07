using Core.MVP;
using ViewInterfaces;

namespace Features.GameResultPopup
{
    public interface IGameResultPopupWindowPresenter :
        IWindowPresenter<IGameResultPopupWindowView, IGameResultPopupModel>
    {
    }
}