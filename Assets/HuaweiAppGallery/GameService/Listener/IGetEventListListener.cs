using System.Collections.Generic;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Listener
{
    public interface IGetEventListListener
    {
        void OnSuccess(List<EventProxy> eventList);

        void OnFailure(int code, string message);
    }
}