using Core.MVP;
using R3;

namespace Features.MessagePopup
{
    public interface IMessagePopupModel : IModel
    {
        ReactiveProperty<string> MessageLocalizationKey { get; }
    }
}