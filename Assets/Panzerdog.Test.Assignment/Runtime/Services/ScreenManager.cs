using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Panzerdog.Test.Assignment.Attributes;
using Panzerdog.Test.Assignment.UI;
using Panzerdog.Test.Assignment.ViewModels;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Panzerdog.Test.Assignment.Services
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private Transform _mainLayer;
        
        private Dictionary<Type, ScreenBase> _shownScreens = new(5);

        public async Task<T> Show<T>(IViewModel viewModel, CancellationToken ct) where T : ScreenBase
        {
            var type = typeof(T);
            
            var attribute = type.GetCustomAttribute(typeof(AddressableScreenAttribute)) as AddressableScreenAttribute;
            var address = attribute.Address;

            var screenPrefab = await Addressables.LoadAssetAsync<GameObject>(address).Task;
            
            var screen = Instantiate(screenPrefab.GetComponent<T>(), _mainLayer);
            await screen.Show(viewModel, ct);

            _shownScreens.Add(type, screen);
            return screen;
        }

        public async Task Hide<T>(CancellationToken ct) where T : ScreenBase
        {
            var screenType = typeof(T);
            
            if (_shownScreens.TryGetValue(screenType, out var screen))
            {
                await screen.Hide(ct);
                Destroy(screen.gameObject);
                _shownScreens.Remove(screenType);
            }
            else
            {
                Debug.LogError($"Screen with type {screenType} isn't active now.");
            }
        }
    }
}