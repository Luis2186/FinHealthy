namespace FinHealthAPI.Authentication
{
    public class JwtOpciones
    {
        public string? Editor { get; set; }
        public string? Audiencia { get; set; }
        public string? ClaveSecreta { get; set; }

        public JwtOpciones()
        {
            ClaveSecreta = "c2VjdXJlX3N0cmluZ19mY2VpdG5vZl93a3VvV1lCZFZNRG9VdXpCMnNJd0cxMlhZ5Pa0TkffVb52lS7iZ0h74c2VjdXJlX3N0cmluZ19mY2VpdG5vZl93a3VvV1lCZFZNRG9VdXpCMnNJd0cxMlhZ5Pa0TkffVb52lS7iZ0h74";
            Audiencia = "FinHealthAPI";
            Editor = "FinHealthAPI_Audience";
        }
    }
}
