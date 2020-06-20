namespace TheGramProfile.Properties
{
    public static class Constants
    {
        public const string FirebaseSignUpUrl = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=";
        public const string FirebaseApiKey = "AIzaSyBgAq89XA31cyg_o9LMAD5YaxEC0K3re9M";

        public const string RabbitMQHost = "localhost"; //rabbitmq-service for k8s, localhost for local
        public const int RabbitMQPort = 5672; //7000 for k8s, 5672 local
    }
}