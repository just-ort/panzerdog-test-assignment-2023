using System.Threading;
using DG.Tweening;

namespace Panzerdog.Test.Assignment.Utils
{
    public static class TweenExtension
    {
        public static Tween Play(this Tween tween, CancellationToken ct)
        {
            tween.SetCancellationToken(ct);
            return tween.Play();
        }
        
        public static Tween SetCancellationToken(this Tween tween, CancellationToken ct)
        {
            tween.onUpdate += CheckCancellation;
            
            void CheckCancellation()
            {
                if (ct.IsCancellationRequested)
                {
                    tween.Kill(true);
                }
            }

            return tween;
        }
    }
}