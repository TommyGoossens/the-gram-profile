namespace TheGramProfile.Services
{
    public interface IEventBusService<out T>
    {
        public T MakeRemoteCall(string message);
        public string PublishMessage();
    }
}