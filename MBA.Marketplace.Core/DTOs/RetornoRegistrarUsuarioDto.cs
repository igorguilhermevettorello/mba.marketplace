using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBA.Marketplace.Core.DTOs
{
    public class RetornoRegistrarUsuarioDto
    {
        public bool Status { get; set; }
        public List<KeyValuePair<string, string>> Error { get; set; } = new List<KeyValuePair<string, string>>();
    }
}
