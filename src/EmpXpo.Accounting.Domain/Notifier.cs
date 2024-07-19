namespace EmpXpo.Accounting.Domain
{
    public class Notifier
    {
        public string Key { get; }
        public string Message { get; }

        public Notifier(string key, string message)
        {
            Key = key;
            Message = message;
        }
    }
}
