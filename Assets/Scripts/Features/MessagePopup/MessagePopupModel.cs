using Core.ModelProvider;
using R3;

namespace Features.MessagePopup
{
    public class MessagePopupModel : BaseModel, IMessagePopupModel
    {
        public MessagePopupModel(IModelProvider modelProvider) : base(modelProvider)
        {
        }

        public ReactiveProperty<string> MessageLocalizationKey { get; } = new();
    }
}