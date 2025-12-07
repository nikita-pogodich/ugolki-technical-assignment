using System;
using System.Collections.Generic;
using Core.MVP;
using R3;

namespace Features
{
    public abstract class BasePresenter<TView, TModel> : IPresenter<TView, TModel>
        where TView : IView
        where TModel : IModel
    {
        private TView _view;
        private TModel _model;
        private bool _hasView;
        private bool _hasModel;
        private bool _isShown;
        private IDisposable _reactiveDisposable;
        private readonly List<IPresenter> _childPresenters = new();

        public TView View => _view;
        public TModel Model => _model;
        public bool HasView => _hasView;
        public bool HasModel => _hasModel;
        public bool IsShown => _isShown;

        public void Init(TView view, TModel model)
        {
            SetView(view);
            SetModel(model);

            DisposableBuilder disposableBuilder = Disposable.CreateBuilder();
            OnInit(ref disposableBuilder);
            _reactiveDisposable = disposableBuilder.Build();
        }

        public void Deinit()
        {
            RemoveView();
            RemoveModel();
            OnDeinit();

            _reactiveDisposable.Dispose();

            foreach (IPresenter childPresenter in _childPresenters)
            {
                childPresenter.Deinit();
            }

            _childPresenters.Clear();
        }

        public virtual void SetShown(bool isShown)
        {
            _isShown = isShown;
            View.SetShown(isShown);

            if (_isShown)
            {
                OnShow();
            }
            else
            {
                OnHide();
            }

            foreach (IPresenter presenter in _childPresenters)
            {
                presenter.SetShown(isShown);
            }
        }

        protected virtual void OnInit(ref DisposableBuilder disposableBuilder)
        {
        }

        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }

        protected virtual void OnDeinit()
        {
        }

        protected void AddChildPresenter(IPresenter presenter)
        {
            _childPresenters.Add(presenter);
        }

        protected void RemoveChildPresenter(IPresenter presenter)
        {
            _childPresenters.Remove(presenter);
        }

        private void SetModel(TModel model)
        {
            _model = model;
            _hasModel = model != null;
        }

        private void RemoveModel()
        {
            _hasModel = false;
        }

        private void SetView(TView view)
        {
            _view = view;
            _hasView = view != null;
        }

        private void RemoveView()
        {
            _hasView = false;
        }
    }
}