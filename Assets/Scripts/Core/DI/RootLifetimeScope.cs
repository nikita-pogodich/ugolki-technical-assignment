using Core.LocalizationManager;
using Core.Logger;
using Core.ResourcesManager;
using Core.ServiceInitialization;
using Core.SettingsHelper;
using Core.ViewProvider;
using Settings;
using UnityEngine;
using VContainer;
using ViewInterfaces;
using Views.LoadingScreen;
using ILogger = Core.Logger.ILogger;

namespace Core.DI
{
    public class RootLifetimeScope : BaseLifetimeScope
    {
        [SerializeField]
        private LocalSettings _localSettings;

        [SerializeField]
        private LoadingScreenView _loadingScreenView;

        private readonly UnityLogger _unityLogger = new();

        protected override void Configure(IContainerBuilder builder)
        {
            ILogger debugLogger = GetDebugLogger();
            var dualLogger = new DualLogger(_unityLogger, debugLogger);

            builder.Register<IServiceInitializer, ServiceInitializer>(Lifetime.Singleton);

            builder.RegisterInstance<ILoadingScreenView, LoadingScreenView>(_loadingScreenView);
            builder.RegisterInstance<IDualLogger>(dualLogger);
            builder.RegisterInstance<ILocalSettings>(_localSettings);
            builder.Register<ILocalizationManager, StubLocalizationManager>(Lifetime.Singleton);
            RegisterInitializableService<IResourcesManager, ResourcesManager.ResourcesManager>(builder);
            RegisterInitializableService<ISettingsHelper, SettingsHelper.SettingsHelper>(builder);
            RegisterInitializableService<IViewProvider, ViewProvider.ViewProvider>(builder);
        }

        protected override void Awake()
        {
            base.Awake();

            CreateChild<ViewLifetimeScope>(childScopeName: nameof(ViewLifetimeScope));
            var logicLifetimeScope = CreateChild<LogicLifetimeScope>(childScopeName: nameof(LogicLifetimeScope));

            logicLifetimeScope.CreateChild<ModelsLifetimeScope>(childScopeName: nameof(ModelsLifetimeScope));
            var presentersLifetimeScope = logicLifetimeScope.CreateChild<PresentersLifetimeScope>(
                childScopeName: nameof(PresentersLifetimeScope));

            presentersLifetimeScope.CreateChild<WindowManagerLifetimeScope>(
                childScopeName: nameof(WindowManagerLifetimeScope));

            presentersLifetimeScope.CreateChild<WorldObjectManagerLifetimeScope>(
                childScopeName: nameof(WorldObjectManagerLifetimeScope));
        }

        private ILogger GetDebugLogger()
        {
#if ENABLE_DEBUG_LOGS
            return _unityLogger;
#else
            return null;
#endif
        }
    }
}