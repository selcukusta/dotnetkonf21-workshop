namespace selenium_api.Controllers
{
    public class Payload
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Phase { get; set; }
        public MetaData Metadata { get; set; }
    }

    public class MetaData
    {
        public long Port { get; set; }
    }
}