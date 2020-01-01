namespace Foundation.HtmlCache.Models
{
    public class PublishOperation
    {
        public enum PublishOperationEnum
        {
            Create,
            Update,
            Delete,
            Ignore // this is used to prevent the queuing operation
        }
    }
}