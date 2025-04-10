namespace MBA.Marketplace.Data.DTOs
{
    public class RetornoRegistrarUsuarioDto
    {
        public bool Status { get; set; }
        public List<KeyValuePair<string, string>> Error { get; set; } = new List<KeyValuePair<string, string>>();
    }
}
