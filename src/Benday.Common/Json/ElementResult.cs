using System.Text.Json;

namespace Benday.Common.Json;

public class ElementResult
{
    public bool Found { get; set; }

    public JsonElement Element { get; set; }
}