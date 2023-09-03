using System.Threading;
using DG.Tweening;

namespace Panzerdog.Test.Assignment.Utils
{
    public static class TweenExtension
    {
        public static Tween Play(this Tween tween, CancellationToken ct)
        {
            return tween.SetCancellationToken(ct).Play();
        }
        
        public static Tween SetCancellationToken(this Tween tween, CancellationToken ct, bool completeWhenCanceled = true)
        {
            tween.onUpdate += CheckCancellation;
            
            void CheckCancellation()
            {
                if (ct.IsCancellationRequested)
                {
                    tween.Kill(completeWhenCanceled);
                }
            }

            return tween;
        }
    }
}