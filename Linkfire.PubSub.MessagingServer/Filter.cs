using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Linkfire.PubSub.MessagingServer
{
    class Filter
    {
        #region Members

        static readonly Dictionary<string, List<EndPoint>> _subscribersList = new Dictionary<string, List<EndPoint>>();

        #endregion

        #region Properties

        static public Dictionary<string, List<EndPoint>> SubscribersList
        {
            get
            {
                lock (typeof(Filter))
                {
                    return _subscribersList;
                }
            }
        }

        #endregion

        #region Methods

        static public List<EndPoint> GetSubscribers(string topicName)
        {
            lock (typeof(Filter))
            {
                if (SubscribersList.ContainsKey(topicName))
                {
                    return SubscribersList[topicName];
                }
                else
                    return null;
            }
        }

        static public void AddSubscriber(string topicName, EndPoint subscriberEndPoint)
        {
            lock (typeof(Filter))
            {
                if (SubscribersList.ContainsKey(topicName))
                {
                    if (!SubscribersList[topicName].Contains(subscriberEndPoint))
                    {
                        SubscribersList[topicName].Add(subscriberEndPoint);
                    }
                }
                else
                {
                    var newSubscribersList = new List<EndPoint>
                    {
                        subscriberEndPoint
                    };
                    SubscribersList.Add(topicName, newSubscribersList);
                }
            }
        }

        static public void RemoveSubscriber(string topicName, EndPoint subscriberEndPoint)
        {
            lock (typeof(Filter))
            {
                if (SubscribersList.ContainsKey(topicName))
                {
                    if (SubscribersList[topicName].Contains(subscriberEndPoint))
                    {
                        SubscribersList[topicName].Remove(subscriberEndPoint);
                    }
                }
            }
        }

        #endregion
    }
}
