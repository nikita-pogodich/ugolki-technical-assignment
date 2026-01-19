using System;
using System.Threading;
using Core.ModelProvider;
using Core.ServiceInitialization;
using Core.WindowManager;
using Core.WorldObjectManager;
using Cysharp.Threading.Tasks;
using Features.MainMenu;
using Features.UgolkiLogic;
using Settings;
using VContainer;
using VContainer.Unity;
using ViewInterfaces;

namespace Core.DI
{
    public class GameBootstrapEntryPoint : IAsyncStartable
    {
        private readonly ILocalSettings _localSettings;
        private readonly IWindowManager _windowManager;
        private readonly IUgolkiModel _ugolkiModel;
        private readonly IModelProvider _modelProvider;
        private readonly IWorldObjectManager _worldObjectManager;
        private readonly IServiceInitializer _serviceInitializer;
        private readonly ILoadingScreenView _loadingScreenView;

        [Inject]
        public GameBootstrapEntryPoint(
            ILocalSettings localSettings,
            IWindowManager windowManager,
            IUgolkiModel ugolkiModel,
            IModelProvider modelProvider,
            IWorldObjectManager worldObjectManager,
            IServiceInitializer serviceInitializer,
            ILoadingScreenView loadingScreenView)
        {
            _localSettings = localSettings;
            _windowManager = windowManager;
            _ugolkiModel = ugolkiModel;
            _modelProvider = modelProvider;
            _worldObjectManager = worldObjectManager;
            _serviceInitializer = serviceInitializer;
            _loadingScreenView = loadingScreenView;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            try
            {
                await InitializeAsync(cancellation);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async UniTask InitializeAsync(CancellationToken cancellation)
        {
            await _serviceInitializer.InitializeAsync(cancellation);
            await CreateUgolkiBoardAsync();
            await ShowStartWindowAsync();
        }

        private async UniTask CreateUgolkiBoardAsync()
        {
            await _modelProvider.InitModelAsync(_ugolkiModel);
            await _worldObjectManager.CreateWorldObjectAsync<IUgolkiBoardPresenter, IUgolkiBoardView, IUgolkiModel>(
                _localSettings.ViewNames.UgolkiBoard,
                _ugolkiModel);
        }

        private async UniTask ShowStartWindowAsync()
        {
            await _windowManager.ShowWindowAsync<IMainMenuWindowView, IMainMenuModel>(
                _localSettings.ViewNames.MainMenu);

            _loadingScreenView.FadeOut();
        }
    }
}