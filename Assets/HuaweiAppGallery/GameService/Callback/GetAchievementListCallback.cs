using System.Collections.Generic;
using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetAchievementListCallback : AndroidJavaProxy
    {
        private IGetAchievementListListener _listener;

        public GetAchievementListCallback(IGetAchievementListListener listener) : base(
            "com.unity.udp.extension.sdk.games.achievement.AchievementsCallback$OnGetAchievementList")
        {
            _listener = listener;
        }

        public void onSuccess(AndroidJavaObject achievementList)
        {
            if (achievementList == null)
            {
                return;
            }

            if (_listener != null)
            {
                int size = achievementList.Call<int>("size");
                List<Achievement> achievements = new List<Achievement>();
                for (int i = 0; i < size; i++)
                {
                    AndroidJavaObject jo = achievementList.Call<AndroidJavaObject>("get", i);
                    Achievement achievement = new Achievement();
                    achievement.AchievementId = jo.Call<string>("getAchievementId");
                    achievement.Type = jo.Call<int>("getType");
                    achievement.Name = jo.Call<string>("getName");
                    achievement.Description = jo.Call<string>("getDescription");
                    AndroidJavaObject unlockedJo = jo.Call<AndroidJavaObject>("getUnlockedImageUrl");
                    if (unlockedJo != null)
                    {
                        achievement.UnlockedImageUrl = unlockedJo.Call<string>("toString");
                    }

                    AndroidJavaObject revealedJo = jo.Call<AndroidJavaObject>("getRevealedImageUrl");
                    if (revealedJo != null)
                    {
                        achievement.RevealedImageUrl = revealedJo.Call<string>("toString");
                    }

                    achievement.TotalSteps = jo.Call<int>("getTotalSteps");
                    achievement.CurrentSteps = jo.Call<int>("getCurrentSteps");
                    achievement.State = jo.Call<int>("getState");
                    achievement.LastUpdatedTimestamp = jo.Call<long>("getLastUpdatedTimestamp");
                    achievements.Add(achievement);
                }

                _listener.OnSuccess(achievements);
            }
        }

        public void onFailure(AndroidJavaObject exception, AndroidJavaObject result)
        {
            if (_listener != null && result != null)
            {
                _listener.OnFailure(result.Call<int>("getCode"), result.Call<string>("getMessage"));
            }
        }
    }
}