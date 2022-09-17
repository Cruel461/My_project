namespace ITWitor.Controllers
{
    public class Message
    {
        private string? text;
        public virtual string? Text { get => text; set => text = value; }
        public MessageType MessageType { get; set; }

        public Message(string text, MessageType messageType)
        {
            this.Text = text;
            MessageType = messageType;
        }
    }
}
