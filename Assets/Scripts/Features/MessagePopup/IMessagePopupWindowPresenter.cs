using Core.MVP;
using ViewInterfaces;

namespace Features.MessagePopup
{
    public interface IMessagePopupWindowPresenter : IWindowPresenter<IMessagePopupView, IMessagePopupModel>
    {
    }
}