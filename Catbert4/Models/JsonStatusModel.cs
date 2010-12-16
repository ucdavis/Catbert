namespace Catbert4.Models
{
    public class JsonStatusModel
    {
        public JsonStatusModel(bool success)
        {
            Success = success;
        }
        public bool Success { get; set; }
        public string Comment { get; set; }
        public object Identifier { get; set; }
    }
}