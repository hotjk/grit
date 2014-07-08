﻿using Grit.CQRS.Actions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.v0_9_1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public class ActionBus : IActionBus
    {
        private IActionHandlerFactory _actionHandlerFactory;
        private string _replyQueueName = null;
        private QueueingBasicConsumer _consumer = null;

        public ActionBus(IActionHandlerFactory ActionHandlerFactory)
        {
            _actionHandlerFactory = ActionHandlerFactory;
        }

        public void Invoke<T>(T action) where T : Action
        {
            var handler = _actionHandlerFactory.GetHandler<T>();
            if (handler != null)
            {
                handler.Invoke(action);
            }
        }

        public Type GetType(string name)
        {
            return _actionHandlerFactory.GetType(name);
        }

        public string GetQueue()
        {
            return _actionHandlerFactory.GetQueue();
        }

        private void DelcareReplyQueue()
        {
            if(_replyQueueName == null)
            {
                var channel = _actionHandlerFactory.GetChannel();
                string name = channel.QueueDeclare();
                _consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(name, true, _consumer);
                _replyQueueName = name;
            }
            else
            {
                BasicDeliverEventArgs result = null;
                while (_consumer.Queue.DequeueNoWait(result) != null)
                {
                    ;
                }
            }
        }

        public ActionResponse Send<T>(T action) where T : Action
        {
            string json = JsonConvert.SerializeObject(action);
            log4net.LogManager.GetLogger("action.logger").Info(
                string.Format("{0}{1}{2}",
                action, Environment.NewLine,
                json));
            
            var channel = _actionHandlerFactory.GetChannel();

            DelcareReplyQueue();

            var props = channel.CreateBasicProperties();
            props.ReplyTo = _replyQueueName;
            props.CorrelationId = action.Id.ToString();
            props.Type = action.Type;
           
            channel.BasicPublish(string.Empty,
                _actionHandlerFactory.GetQueue(),
                props,
                Encoding.UTF8.GetBytes(json));

            BasicDeliverEventArgs result;
            if (_consumer.Queue.Dequeue(10000, out result))
            {
                return JsonConvert.DeserializeObject<ActionResponse>(Encoding.UTF8.GetString(result.Body));
            }
            throw new ApplicationException();
        }
    }
}
