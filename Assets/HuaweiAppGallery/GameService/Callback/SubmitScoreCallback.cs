using System.Collections.Generic;
using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class SubmitScoreCallback : AndroidJavaProxy
    {
        private ISubmitScoreListener _listener;

        public SubmitScoreCallback(ISubmitScoreListener listener) : base(
            "com.unity.udp.extension.sdk.games.leaderboard.LeaderboardsCallback$OnSubmitScore")
        {
            _listener = listener;
        }

        public void onSuccess(AndroidJavaObject jo)
        {
            if (jo == null)
            {
                return;
            }

            if (_listener != null)
            {
                ScoreSubmission submission = new ScoreSubmission();
                submission.LeaderboardId = jo.Call<string>("getLeaderboardId");
                submission.PlayerId = jo.Call<string>("getPlayerId");
                Dictionary<int, ScoreSubmission.Result> dictionary = new Dictionary<int, ScoreSubmission.Result>();
                AndroidJavaObject result0 = jo.Call<AndroidJavaObject>("getScoreResult", 0);
                if (result0 != null)
                {
                    dictionary.Add(0, convertResult(result0));
                }

                AndroidJavaObject result1 = jo.Call<AndroidJavaObject>("getScoreResult", 1);
                if (result1 != null)
                {
                    dictionary.Add(1, convertResult(result1));
                }

                AndroidJavaObject result2 = jo.Call<AndroidJavaObject>("getScoreResult", 2);
                if (result2 != null)
                {
                    dictionary.Add(2, convertResult(result2));
                }
                submission.ScoreResults = dictionary;
                _listener.OnSuccess(submission);
            }
        }

        public void onFailure(AndroidJavaObject exception, AndroidJavaObject result)
        {
            if (_listener != null && result != null)
            {
                _listener.OnFailure(result.Call<int>("getCode"), result.Call<string>("getMessage"));
            }
        }

        private ScoreSubmission.Result convertResult(AndroidJavaObject jo)
        {
            ScoreSubmission.Result result = new ScoreSubmission.Result();
            result.RawScore = jo.Get<long>("rawScore");
            result.FormattedScore = jo.Get<string>("formattedScore");
            result.ScoreTag = jo.Get<string>("scoreTag");
            result.IsBest = jo.Get<bool>("isBest");
            return result;
        }
    }
}