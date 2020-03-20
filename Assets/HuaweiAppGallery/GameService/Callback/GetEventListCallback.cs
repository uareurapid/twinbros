using System.Collections.Generic;
using UnityEngine.HuaweiAppGallery.Listener;
using UnityEngine.HuaweiAppGallery.Model;

namespace UnityEngine.HuaweiAppGallery.Callback
{
    public class GetEventListCallback : AndroidJavaProxy
    {
        private IGetEventListListener _listener;

        public GetEventListCallback(IGetEventListListener listener) : base(
            "com.unity.udp.extension.sdk.games.event.EventsCallback$OnGetEventList")
        {
            _listener = listener;
        }

        public void onSuccess(AndroidJavaObject eventList)
        {
            if (eventList == null)
            {
                return;
            }

            if (_listener != null)
            {
                int size = eventList.Call<int>("size");
                List<EventProxy> list = new List<EventProxy>();
                for (int i = 0; i < size; i++)
                {
                    AndroidJavaObject jo = eventList.Call<AndroidJavaObject>("get", i);
                    EventProxy eventProxy = new EventProxy();
                    eventProxy.EventId = jo.Call<string>("getEventId");
                    eventProxy.Name = jo.Call<string>("getName");
                    eventProxy.Description = jo.Call<string>("getDescription");
                    AndroidJavaObject thumbnailJo = jo.Call<AndroidJavaObject>("getThumbnailUri");
                    if (thumbnailJo != null)
                    {
                        eventProxy.ThumbnailUri = thumbnailJo.Call<string>("toString");
                    }

                    eventProxy.Value = jo.Call<long>("getValue");
                    eventProxy.LocaleValue = jo.Call<string>("getLocaleValue");
                    eventProxy.IsVisible = jo.Call<bool>("isVisible");
                    AndroidJavaObject playerJo = jo.Call<AndroidJavaObject>("getPlayer");
                    if (playerJo != null)
                    {
                        Player player = new Player();
                        player.DisplayName = playerJo.Call<string>("getDisplayName");
                        AndroidJavaObject hiResImageJo = playerJo.Call<AndroidJavaObject>("getHiResImageUri");
                        if (hiResImageJo != null)
                        {
                            player.HiResImageUri = hiResImageJo.Call<string>("toString");
                        }

                        AndroidJavaObject iconImageJo = playerJo.Call<AndroidJavaObject>("getIconImageUri");
                        if (iconImageJo != null)
                        {
                            player.IconImageUri = iconImageJo.Call<string>("toString");
                        }

                        player.PlayerId = playerJo.Call<string>("getPlayerId");
                        player.SignTimestamp = playerJo.Call<string>("getSignTimestamp");
                        player.PlayerSign = playerJo.Call<string>("getPlayerSign");
                        player.Level = playerJo.Call<int>("getLevel");
                        eventProxy.Player = player;
                    }

                    list.Add(eventProxy);
                }

                _listener.OnSuccess(list);
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