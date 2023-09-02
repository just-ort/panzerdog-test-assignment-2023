using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Panzerdog.Test.Assignment.Services;
using Panzerdog.Test.Assignment.Utils;
using Panzerdog.Test.Assignment.ViewModels;
using UnityEngine;

namespace Panzerdog.Test.Assignment.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ScreenBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 1f;
        
        public async Task Show(IViewModel viewModel, CancellationToken ct)
        {
            Init(viewModel);
            _canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            await _canvasGroup.DOFade(1f, _fadeDuration).Play(ct).AsyncWaitForCompletion();
            await OnShow();
        }
        
        public async Task Hide(CancellationToken ct)
        {
            await OnHide();
            await _canvasGroup.DOFade(0f, _fadeDuration).Play(ct).AsyncWaitForCompletion();
            Dispose();
            gameObject.SetActive(false);
        }

        protected abstract void Init(IViewModel viewModel);
        
        protected abstract void Dispose();

        protected virtual Task OnShow()
        {
            return Task.CompletedTask;
        }
        
        protected virtual Task OnHide()
        {
            return Task.CompletedTask;
        }
    }
}