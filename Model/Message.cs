namespace OpenProjects.Model
{
    public class Message
    {
        public Message(object recordID, MessageType messageType, string title = "", string miniDesc = "",
            string desk = "", string file = "", Item selectItem = null, string errorText = "", string author = "",
            bool _isEdit = true)
        {
            RecordID = recordID;
            Title = title;
            miniDescription = miniDesc;
            Description = desk;
            Filepath = file;
            ErrorText = errorText;
            MessageType = messageType;
            SelectItem = selectItem;
            Author = author;
            IsEdit = _isEdit;
        }

        public MessageType MessageType { get; }
        public object RecordID { get; }
        public string Title { get; set; }
        public string miniDescription { get; set; }
        public string Description { get; set; }
        public string Filepath { get; set; }
        public Item SelectItem { get; set; }
        public string ErrorText { get; set; }
        public string Author { get; set; }
        public bool IsEdit { get; set; } = true;
    }
}