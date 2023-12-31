
using RocketTeam.Sdk.Services.Interfaces;
using System;

namespace RocketTeam.Sdk.Services.Manager
{
    public class SdkManager : IServiceManager
    {
        private static SdkManager instance;
        public static SdkManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SdkManager();
                }
                return instance;
            }
        }

        private static string clientKey = string.Empty;
        private static bool isSandbox = true;

        // public IAuthenticationManager AuthenticationManager { get; private set; }

        public IAdsManager AdsManager { get; private set; }
        public IMoreGamesManager MoreGamesManager { get; private set; }
        public IBubbleAdManager BubbleAdManager { get; private set; }

        // public ILeaderboardManager LeaderboardManager { get; private set; }

#if PAYMENT_ENABLE
        public IPaymentManager PaymentManager { get; private set; }
#endif

        //public IRemoteDataManager RemoteDataManager { get; private set; }
        //public IPlayerDataManager PlayerDataManager { get; private set; }

        //public ITrackingManager TrackingManager { get; private set; }

        //public ILocationManager LocationManager { get; private set; }

        public SdkManager()
        {
        }

        public static void Init(string _clientKey, bool _isSandbox)
        {
            if (!string.IsNullOrEmpty(_clientKey))
            {
                clientKey = _clientKey;
            }

            isSandbox = _isSandbox;
            //DebugCustom.Log(string.Format("Init SdkManager success: client key: {0}, isSandbox: {1}", clientKey, isSandbox));
        }

        public static bool IsSandbox()
        {
            return isSandbox;
        }

        //public bool PrepareRequest(string route, HTTPMethods method, OnRequestFinishedDelegate callback, out HTTPRequest request)
        //{
        //    request = null;
        //    if (!string.IsNullOrEmpty(clientKey))
        //    {
        //        try
        //        {
        //            request = new HTTPRequest(new Uri(GetServerUrl() + route), method, callback);
        //            request.AddHeader(SdkManagerConstants.HEADER_CLIENT_KEY, clientKey);
        //            request.AddHeader(SdkManagerConstants.HEADER_DEVICE_ID, DeviceInfoUtils.GetDeviceId());

        //            return true;
        //        }
        //        catch (Exception e)
        //        {
        //            DebugCustom.Log(e.ToString());
        //        }
        //    }

        //    return false;
        //}

        private string GetServerUrl()
        {
            if (isSandbox)
            {
                return SdkManagerConstants.SERVER_SANDBOX_URL;
            }
            else
            {
                return SdkManagerConstants.SERVER_REAL_URL;
            }
        }

        #region Manage service manager
        public void RegisterService(ServiceType serviceType, object service)
        {
            if (service != null)
            {
                switch (serviceType)
                {


                    case ServiceType.ADVERTISEMENT:
                        if (AdsManager == null)
                        {
                            AdsManager = service as IAdsManager;
                        }
                        else
                        {
                            //DebugCustom.Log(string.Format("{0} is already registered", serviceType));
                        }
                        break;

                    case ServiceType.BUBBLE_ADVERTISEMENT:
                        if (BubbleAdManager == null)
                        {
                            BubbleAdManager = service as IBubbleAdManager;
                        }
                        else
                        {
                            //DebugCustom.Log(string.Format("{0} is already registered", serviceType));
                        }
                        break;

                    case ServiceType.MORE_GAME:
                        if (MoreGamesManager == null)
                        {
                            MoreGamesManager = service as IMoreGamesManager;
                        }
                        else
                        {
                            //DebugCustom.Log(string.Format("{0} is already registered", serviceType));
                        }
                        break;



#if PAYMENT_ENABLE
                    case ServiceType.PAYMENT:
                        if (PaymentManager == null)
                        {
                            PaymentManager = service as IPaymentManager;
                        }
                        else
                        {
                            //DebugCustom.Log(string.Format("{0} is already registered", serviceType));
                        }
                        break;
#endif
                    default:
                        //DebugCustom.Log(string.Format("{0} is not supported", serviceType));
                        break;
                }
            }
            else
            {
                //DebugCustom.Log(string.Format("Register for service {0} failed: input service is null", serviceType));
            }
        }

        public void UnregisterService(ServiceType serviceType, object service)
        {
            if (service != null)
            {
                switch (serviceType)
                {

                    case ServiceType.ADVERTISEMENT:
                        if (AdsManager == service)
                        {
                            AdsManager = null;
                        }
                        else
                        {
                            //DebugCustom.Log(string.Format("Can't unregister {0}, service is not same source", serviceType));
                        }
                        break;

                    case ServiceType.BUBBLE_ADVERTISEMENT:
                        if (BubbleAdManager == service)
                        {
                            BubbleAdManager = null;
                        }
                        else
                        {
                           // DebugCustom.Log(string.Format("Can't unregister {0}, service is not same source", serviceType));
                        }
                        break;

                    case ServiceType.MORE_GAME:
                        if (MoreGamesManager == service)
                        {
                            MoreGamesManager = null;
                        }
                        else
                        {
                           // DebugCustom.Log(string.Format("Can't unregister {0}, service is not same source", serviceType));
                        }
                        break;



#if PAYMENT_ENABLE
                    case ServiceType.PAYMENT:
                        if (PaymentManager == service)
                        {
                            PaymentManager = null;
                        }
                        else
                        {
                           // DebugCustom.Log(string.Format("Can't unregister {0}, service is not same source", serviceType));
                        }
                        break;
#endif


                    default:
                        //DebugCustom.Log(string.Format("{0} is not supported", serviceType));
                        break;
                }
            }
            else
            {
                //DebugCustom.Log(string.Format("Register for service {0} failed: input service is null", serviceType));
            }
        }
        #endregion
    }
}