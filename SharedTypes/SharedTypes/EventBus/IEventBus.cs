namespace SharedTypes.EventBus
{
    public interface IEventBus
    {
        /// <summary>
        /// Publish any <see cref="IntegrationEvent"/> to the queue.
        /// </summary>
        /// <param name="event">Event to publish</param>
        void Publish(IntegrationEvent @event);

        /// <summary>
        /// Subscribe to an event of a specific type. Note: this will listen to all events the <see cref="IntegrationEvent"/> type specified.
        /// </summary>
        /// <typeparam name="T"><see cref="IntegrationEvent"/> to subscribe to.</typeparam>
        /// <typeparam name="TH">Callback method to handle the event when it is triggered.</typeparam>
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Unsubscribe from a specific type of element no matter its name.
        /// </summary>
        /// <typeparam name="T">The <see cref="IntegrationEvent"/> not to listen for anymore.</typeparam>
        /// <typeparam name="TH">The handler to not use anymore.</typeparam>
        void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Subscribe to any event with a given name.
        /// </summary>
        /// <typeparam name="TH">The event to listen for.</typeparam>
        /// <param name="eventName">The name of the event to listen for.</param>
        void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;

        /// <summary>
        /// Unsubscribe to any event with a given name.
        /// </summary>
        /// <typeparam name="TH">The event to not listen for anymore.</typeparam>
        /// <param name="eventName">The name of the event to not listen for anymore.</param>
        void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;
    }
}
