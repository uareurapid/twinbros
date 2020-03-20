using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface ISubmitScoreListener
    {
        void OnSuccess(ScoreSubmission scoreSubmission);
        
        void OnFailure(int code, string message);
    }
}