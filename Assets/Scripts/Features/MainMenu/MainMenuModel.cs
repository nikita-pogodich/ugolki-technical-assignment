using Core.ModelProvider;
using Cysharp.Threading.Tasks;
using Features.MainMenu.UgolkiRulesList;
using VContainer;

namespace Features.MainMenu
{
    public class MainMenuModel : BaseModel, IMainMenuModel
    {
        private readonly IModelProvider _modelProvider;

        private IUgolkiRulesListModel _ugolkiRulesListModel;

        [Inject]
        public MainMenuModel(IModelProvider modelProvider) : base(modelProvider)
        {
            _modelProvider = modelProvider;
        }

        public IUgolkiRulesListModel UgolkiRulesListModel => _ugolkiRulesListModel;

        protected override async UniTask OnInit()
        {
            _ugolkiRulesListModel = await _modelProvider.GetAsync<IUgolkiRulesListModel>();
            await UniTask.CompletedTask;
        }

        protected override void OnDeinit()
        {
            _ugolkiRulesListModel.Deinit();
        }
    }
}