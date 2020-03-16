namespace ApiModels
{
    public class ObjectCreatedOutputModel<TKey>
    {
        public ObjectCreatedOutputModel()
        {
        }

        public ObjectCreatedOutputModel(TKey id, string location)
        {
            Id = id;
            Location = location;
        }

        public TKey Id { get; set; }

        public string Location { get; set; }
    }
}