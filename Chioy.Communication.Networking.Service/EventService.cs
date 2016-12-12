using Chioy.Communication.Networking.Interface;
using Chioy.Communication.Networking.Service.ProductService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Chioy.Communication.Networking.Service
{
    public delegate void NewClientSubscribedEventHandler(SubscribeArg arg);
    public delegate void ClientLostEventHandler(SubscribeArg arg);

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class EventService : KRService, IEventService
    {

        public static readonly ConcurrentDictionary<string, SubscribeContext> _Subscribers = new ConcurrentDictionary<string, SubscribeContext>();

        public static event NewClientSubscribedEventHandler NewClientSubscribedEvent;

        public static event ClientLostEventHandler ClientLostEvent;
        private Tuple<string, int> GetClientInfo()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties properties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return new Tuple<string, int>(endpoint.Address, endpoint.Port);
        }

        public void Subscribe(SubscribeArg arg)
        {
            var newClientInfo = GetClientInfo();

            var callback = OperationContext.Current.GetCallbackChannel<IEventCallback>();
            arg.Username = arg.Username.ToLower();
            if (!_Subscribers.ContainsKey(arg.Username))
            {
                _Subscribers[arg.Username] = new SubscribeContext() { Arg = arg, Callback = callback, Address = newClientInfo.Item1, Port = newClientInfo.Item2 };
            }
            else
            {
                if (_Subscribers[arg.Username].Address != newClientInfo.Item1 || _Subscribers[arg.Username].Port != newClientInfo.Item2)
                {
                    _Subscribers[arg.Username] = new SubscribeContext() { Arg = arg, Callback = callback, Address = newClientInfo.Item1, Port = newClientInfo.Item2 };
                }
            }
            ICommunicationObject obj = (ICommunicationObject)callback;
            obj.Closed += (sender, args) =>
            {
                //current client closed, do nothing.
            };
            obj.Faulted += (sender, args) =>
            {
                //To do:
                //Record error
            };
            obj.Closing += (sender, args) =>
            {
                var currentCallback = sender as IEventCallback;
                _Subscribers.ToList().ForEach(context =>
                {
                    if (context.Value.Callback == currentCallback)
                    {
                        RemoveSubscriber(context.Value.Arg.Username);
                        ClientLostEvent?.Invoke(arg);
                    }
                });

            };
            NewClientSubscribedEvent?.Invoke(arg);
        }

        private static void RemoveSubscriber(string username)
        {
            username = username.ToLower();
            if (_Subscribers.ContainsKey(username))
            {
                SubscribeContext outObj = null;
                _Subscribers.TryRemove(username, out outObj);
            }
        }

        public void Unsubscribe(ArgumentBase<string> arg)
        {
            RemoveSubscriber(arg.Model);
        }

        public DateTime Ping()
        {
            return DateTime.Now;
        }

        public static void BroadcastEvent(ArgumentBase<string> arg)
        {
            _Subscribers.ToList().ForEach(subscriber =>
            {
                ICommunicationObject obj = subscriber.Value.Callback as ICommunicationObject;
                if (obj != null)
                {
                    if (obj.State == CommunicationState.Opened)
                    {
                        try
                        {
                            subscriber.Value.Callback.OnEventFired(arg);
                        }
                        catch (Exception ex)
                        {
                            RemoveSubscriber(subscriber.Value.Arg.Username);
                            //Record error.
                        }
                    }
                    else
                    {
                        //Current client closed
                        RemoveSubscriber(subscriber.Value.Arg.Username);
                    }
                }
            });
        }

        public static void NotifySpecClient(string key, ArgumentBase<string> arg)
        {
            if (_Subscribers.ContainsKey(key))
            {
                ICommunicationObject obj = _Subscribers[key].Callback as ICommunicationObject;
                if (obj.State == CommunicationState.Opened)
                {
                    try
                    {
                        _Subscribers[key].Callback.OnEventFired(arg);
                    }
                    catch (Exception ex)
                    {
                        RemoveSubscriber(key);
                        //Record error.
                    }
                }
            }
        }
    }
}
