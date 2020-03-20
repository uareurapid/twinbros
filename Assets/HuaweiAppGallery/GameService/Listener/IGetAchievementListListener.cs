using System.Collections.Generic;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetAchievementListListener
    {
        void OnSuccess(List<Achievement> achievementList);

        void OnFailure(int code, string message);
    }
}