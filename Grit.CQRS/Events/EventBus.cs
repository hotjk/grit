﻿using Newtonsoft.Json;
using RabbitMQ.Client.Framing.v0_9_1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public class EventBus : IEventBus
    {
        private IEventHandlerFactory _eventHandlerFactory;
        private IList<Event> _events = new List<Event>();

        public EventBus(IEventHandlerFactory eventHandlerFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
        }

        public void Publish<T>(T @event) where T : Event
        {
            _events.Add(@event);
        }

        public void FlushAll()
        {
            foreach(Event @event in _events)
            {
                Flush(@event);
            }
            _events.Clear();
        }

        private void Flush<T>(T @event) where T : Event
        {
            string json = JsonConvert.SerializeObject(@event);
            log4net.LogManager.GetLogger("event.logger").Debug(
                string.Format("{0}{1}{2}",
                @event, Environment.NewLine,
                json));

            var channel = _eventHandlerFactory.GetChannel();

            channel.BasicPublish(_eventHandlerFactory.GetExchange(),
                @event.RoutingKey, 
                new BasicProperties { 
                    DeliveryMode = 2
                },
                Encoding.UTF8.GetBytes(json));

            var handlers = _eventHandlerFactory.GetHandlers<T>();
            if (handlers != null)
            {
                foreach (var handler in handlers)
                {
                    // handle event in current thread
                    // handler.Handle(@event);

                    // handle event in thread pool
                    ThreadPool.QueueUserWorkItem(x =>
                    {
                        try
                        {
                            handler.Handle(@event);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    });
                }
            }
        }
    }
}
